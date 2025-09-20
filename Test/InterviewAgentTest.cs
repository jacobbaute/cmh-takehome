using System.Threading.Tasks;
using Interview;
using Moq;
using Moq.Protected;

namespace Test;

public class InterviewAgentTest
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestCalculateInterviewsScheduledForDay()
    {
        Interview.Agent.InterviewAgent agent = new Interview.Agent.InterviewAgent(null);
        DateTimeOffset today = DateTimeOffset.Now;
        DateTimeOffset tomorrow = today.AddDays(1);
        List<ScheduledInterview> scheduledInterviews = [new ScheduledInterview(today, "Jim"),
            new ScheduledInterview(today, "James"), new ScheduledInterview(tomorrow, "John")];
        List<ScheduledInterview> expectedInterviewsToday = [new ScheduledInterview(today, "Jim"),
            new ScheduledInterview(today, "James")];
        List<ScheduledInterview> expectedInterviewsTomorrow = [new ScheduledInterview(tomorrow, "John")];

        Assert.That(agent.FilterInterviewsOnDay(scheduledInterviews, today), Is.EqualTo(expectedInterviewsToday));
        Assert.That(agent.FilterInterviewsOnDay(scheduledInterviews, tomorrow), Is.EqualTo(expectedInterviewsTomorrow));
    }

    [Test]
    public async Task TestGetScheduledInterviews()
    {
        var mockMessageHandler = new Mock<HttpMessageHandler>();
        mockMessageHandler.Protected().Setup<Task<HttpResponseMessage>>(
            "SendAsync",
            ItExpr.IsAny<HttpRequestMessage>(),
            ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Content = new StringContent("[]")
            });

        Interview.Agent.InterviewAgent agent = new Interview.Agent.InterviewAgent(new HttpClient(mockMessageHandler.Object));
        List<ScheduledInterview> scheduledInterviews = await agent.GetScheduledInterviews("https://notawebsite.fake");
        List<ScheduledInterview> expectedInterviews = [];

        Assert.That(scheduledInterviews, Is.EqualTo(expectedInterviews));
    }
    
    [Test]
    public async Task TestGetScheduledInterviews_ExternalApiFailure()
    {
        var mockMessageHandler = new Mock<HttpMessageHandler>();
        mockMessageHandler.Protected().Setup<Task<HttpResponseMessage>>(
            "SendAsync",
            ItExpr.IsAny<HttpRequestMessage>(),
            ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage {
                        StatusCode = System.Net.HttpStatusCode.BadGateway,
                        Content = new StringContent("[]")
                    });

        Interview.Agent.InterviewAgent agent = new Interview.Agent.InterviewAgent(new HttpClient(mockMessageHandler.Object));
        List<ScheduledInterview> expectedInterviews = [];
        Assert.ThrowsAsync<HttpRequestException>(async delegate { await agent.GetScheduledInterviews("https://notawebsite.fake"); });
    }
}
