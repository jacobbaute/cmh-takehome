namespace Interview.Agent;

public class InterviewAgent
{
    private readonly HttpClient HttpClient;

    public InterviewAgent(HttpClient httpClient)
    {
        HttpClient = httpClient;
    }

    /// <summary>
    /// Performs a call to load scheduled interviews.
    /// </summary>
    /// <returns>A list of scheduled interviews. The list will not be null but may be empty.</returns>
    /// <exception cref="HttpRequestException">When the dependent API encounters a failure.</exception>
    public async Task<List<ScheduledInterview>> GetScheduledInterviews(String url)
    {
        var response = await HttpClient.GetAsync(url);
        if (response.IsSuccessStatusCode)
        {
            var scheduledInterviews = await response.Content.ReadFromJsonAsync<List<ScheduledInterview>>();
            return scheduledInterviews ?? [];
        }
        throw new HttpRequestException(String.Format("An error occurred while retrieving scheduled candidates. Status code: {0}", response.StatusCode),
            null, System.Net.HttpStatusCode.BadGateway);
    }

    /// <summary>
    /// Calculates the number of ScheduledInterviews that are on the same day as the dateToCheck.
    /// </summary>
    /// <param name="scheduledInterviews">A list of scheduled interviews.</param>
    /// <param name="dateToCheck">The date to compare against.</param>
    /// <returns>The number of interviews on the given day.</returns>
    public long CalculateInterviewsScheduledForDay(List<ScheduledInterview> scheduledInterviews, DateTimeOffset dateToCheck)
    {
        long scheduledInterviewsCount = 0;
        foreach (ScheduledInterview scheduledInterview in scheduledInterviews)
        {
            if (scheduledInterview.DateOfInterview.Date == dateToCheck.Date)
            {
                scheduledInterviewsCount++;
            }
        }
        return scheduledInterviewsCount;
    }
}