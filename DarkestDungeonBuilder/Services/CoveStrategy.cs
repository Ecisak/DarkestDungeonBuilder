using DarkestDungeonBuilder.Models;

namespace DarkestDungeonBuilder.Services;

public class CoveStrategy : ILocationStrategy
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
            analysis.AddSuggestion($"Bleed-focused skills are low value in Cove. Consider blight or armor piercing instead: {string.Join(", ", bleedHeroes)}.");
        }

        var hasArmorPiercing = team.Slots.Values
            .Where(h => h != null)
            .Any(h => h!.SelectedSkills.Any(s => s.EffectsBitfield.HasFlag(Skill.SkillEffect.ArmorPiercing)));

        if (!hasArmorPiercing)
        {
            analysis.AddCritical("Cove has high protection enemies, but the team has no armor piercing skill.");
        }

        return analysis;
    }
}
