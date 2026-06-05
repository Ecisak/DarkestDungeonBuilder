namespace DarkestDungeonBuilder.Services;

public class AdvisorAnalysis
{
    public List<string> CriticalIssues { get; } = [];
    public List<string> Suggestions { get; } = [];

    public bool HasAnyMessages => CriticalIssues.Count > 0 || Suggestions.Count > 0;

    public void AddCritical(string message)
    {
        if (!string.IsNullOrWhiteSpace(message) && !CriticalIssues.Contains(message))
        {
            CriticalIssues.Add(message);
        }
    }

    public void AddSuggestion(string message)
    {
        if (!string.IsNullOrWhiteSpace(message) && !Suggestions.Contains(message))
        {
            Suggestions.Add(message);
        }
    }

    public void Merge(AdvisorAnalysis other)
    {
        foreach (var message in other.CriticalIssues)
        {
            AddCritical(message);
        }

        foreach (var message in other.Suggestions)
        {
            AddSuggestion(message);
        }
    }
}
