namespace Interview;

/// <summary>
/// Output for the check interviews service 
/// </summary>
public class CheckInterviewsResponse
{
    public long NumberOfInterviews { get; set; }

    public required List<Interview> Interviews { get; set; }
}
