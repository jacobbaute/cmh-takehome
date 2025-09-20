namespace Interview;

/// <summary>
/// Input for the check interviews service
/// </summary>
public class InterviewTranslator
{
    public static List<Interview> ToInterviews(List<ScheduledInterview> scheduledInterviews)
    {
        List<Interview> interviews = new List<Interview>();
        foreach (ScheduledInterview scheduledInterview in scheduledInterviews)
        {
            interviews.Add(new Interview(scheduledInterview.Name));
        }
        return interviews;
    }
}
