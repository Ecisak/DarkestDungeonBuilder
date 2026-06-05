using DarkestDungeonBuilder.Models;

namespace DarkestDungeonBuilder.Services;

public class RuinsStrategy : ILocationStrategy
{
    public AdvisorAnalysis AnalyzeTeamForLocation(Team team)
    {
        var analysis = new AdvisorAnalysis();

        var bleedHeroes = team.Slots.Values
            .Where(h => h != null)
            .Where(h => h!.SelectedSkills.Any(s => s.EffectsBitfield.HasFlag(Skill.SkillEffect.Bleed)))
            .Select(h => h!.Name)
            .Distinct()
            .ToList();

        if (bleedHeroes.Count > 0)
        {
            analysis.AddSuggestion($"Bleed-focused skills are low value in Ruins because many enemies are unholy: {string.Join(", ", bleedHeroes)}.");
        }

        return analysis;
    }
}
