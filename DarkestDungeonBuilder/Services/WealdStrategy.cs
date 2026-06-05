using DarkestDungeonBuilder.Models;

namespace DarkestDungeonBuilder.Services;

public class WealdStrategy : ILocationStrategy
{
    public AdvisorAnalysis AnalyzeTeamForLocation(Team team)
    {
        var analysis = new AdvisorAnalysis();

        var blightHeroes = team.Slots.Values
            .Where(h => h != null)
            .Where(h => h!.SelectedSkills.Any(s => s.EffectsBitfield.HasFlag(Skill.SkillEffect.Blight)))
            .Select(h => h!.Name)
            .Distinct()
            .ToList();

        if (blightHeroes.Count > 0)
        {
            analysis.AddSuggestion($"Blight is weaker in Weald, so these heroes may have less efficient loadouts there: {string.Join(", ", blightHeroes)}.");
        }

        return analysis;
    }
}
