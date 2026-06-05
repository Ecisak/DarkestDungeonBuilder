using DarkestDungeonBuilder.Models;

namespace DarkestDungeonBuilder.Services;

public class WarrensStrategy : ILocationStrategy
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
            analysis.AddSuggestion($"Blight is less effective in Warrens, so these heroes may be underperforming there: {string.Join(", ", blightHeroes)}.");
        }

        var hasCure = team.Slots.Values
            .Where(h => h != null)
            .Any(h => h!.SelectedSkills.Any(s => s.EffectsBitfield.HasFlag(Skill.SkillEffect.Cure)));

        if (!hasCure)
        {
            analysis.AddSuggestion("Warrens often inflicts disease. Bringing at least one cure skill would make the team safer.");
        }

        return analysis;
    }
}
