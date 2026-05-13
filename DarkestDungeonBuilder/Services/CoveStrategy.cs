using DarkestDungeonBuilder.Models;

namespace DarkestDungeonBuilder.Services;

public class CoveStrategy : ILocationStrategy
{
    public List<string> AnalyzeTeamForLocation(Team team)
    {
        var warnings = new List<string>();

        var hasBleed = team.Slots.Values
            .Where(h => h != null)
            .Where(h => h!.SelectedSkills.Any(s => s.EffectsBitfield.HasFlag(Skill.SkillEffect.Bleed)))
            .ToList();
        if (hasBleed.Count != 0)
        {
            var heroNames = string.Join(", ", hasBleed.Select(h => h?.Name));
            warnings.Add("Bleed has no effect against monsters in Cove. Change skills of or remove them: " + heroNames);
        }

        var armorPiercing = team.Slots.Values
            .Where(h => h != null)
            .Where(h => h!.SelectedSkills.Any(s => s.EffectsBitfield.HasFlag(Skill.SkillEffect.ArmorPiercing)))
            .ToList();
        if (armorPiercing.Count > 0) return warnings;
        {
            warnings.Add("Cove has high protection enemies. Your team has no armor piercing skills.");
        }
        return warnings;
    }
}