namespace Interview;

/// <summary>
/// Represents an interview for a candidate.
/// </summary>
public record class Interview
{
    public string Name { get; set; }

    public Interview(string name)
    {
        Name = name;
    }
}
