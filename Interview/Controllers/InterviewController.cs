using System.Threading.Tasks;
using Interview.Agent;
using Microsoft.AspNetCore.Mvc;

namespace Interview.Controllers;

/// <summary>
/// Controller to handle checking scheduled interviews.
/// </summary>
[ApiController]
[Route("api/CheckInterviews")]
public class InterviewController : ControllerBase
{
    private readonly ILogger<InterviewController> _logger;
    private static HttpClient httpClient = new HttpClient();

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="logger">An instance of ILogger to use while logging</param>
    public InterviewController(ILogger<InterviewController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Loads scheduled interview candidates from an external api and computes the number of scheduled interviews for the provided day.
    /// </summary>
    /// <param name="checkInterviewsRequest">The input containing a date to compare against.</param>
    /// <returns>
    /// * A 200 when the operation is successful.
    /// * A 500 if an error occurs and no additional information is available.
    /// * A 502 if an unsuccessful status is returned while loading interviews.
    /// * A 504 is there is a timeout while loading scheduled interviews.
    /// </returns>
    [HttpPost(Name = "CheckInterviews")]
    public async Task<ActionResult<CheckInterviewsResponse>> Post(CheckInterviewsRequest checkInterviewsRequest)
    {
        List<ScheduledInterview> scheduledInterviews;
        InterviewAgent interviewAgent = new(httpClient);

        try
        {
            scheduledInterviews = await interviewAgent.GetScheduledInterviews("https://cmricandidates.azurewebsites.net/api/getcandidates");
        }
        catch (OperationCanceledException)
        {
            _logger.Log(LogLevel.Error, "Timeout while loading scheduled interviews");
            return new StatusCodeResult(504);
        }
        catch (HttpRequestException httpRequestException)
        {
            _logger.Log(LogLevel.Error, httpRequestException.Message);

            int statusCode;
            if (httpRequestException.StatusCode == null)
            {
                statusCode = 500;
            }
            else
            {
                statusCode = (int)httpRequestException.StatusCode;
            }

            return new StatusCodeResult(statusCode);
        }

        List<ScheduledInterview> filteredInterviews = interviewAgent.FilterInterviewsOnDay(scheduledInterviews, checkInterviewsRequest.DateOfInterview);

        return Ok(new CheckInterviewsResponse
        {
            NumberOfInterviews = filteredInterviews.Count,
            Interviews = InterviewTranslator.ToInterviews(filteredInterviews)
        });
    }
}
