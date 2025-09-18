namespace Interview;

/// <summary>
/// Represents a scheduled interview for a candidate.
/// </summary>
public record class ScheduledInterview
{
    public DateTimeOffset DateOfInterview { get; set; }

    public string Name { get; set; }

    public ScheduledInterview(DateTimeOffset dateOfInterview, string name)
    {
        DateOfInterview = dateOfInterview;
        Name = name;
    }
}
