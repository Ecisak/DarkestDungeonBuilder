using DarkestDungeonBuilder.Models;

namespace DarkestDungeonBuilder.Services;

public class WarrensStrategy : ILocationStrategy
{
    public List<string> AnalyzeTeamForLocation(Team team)
    {
        var warnings = new List<string>();

        var hasBlight = team.Slots.Values
            .Where(h => h != null)
            .Where(h => h!.SelectedSkills.Any(s => s.EffectsBitfield.HasFlag(Skill.SkillEffect.Blight)))
            .ToList();
        if (hasBlight.Count != 0)
        {
            var heroNames = string.Join(", ", hasBlight.Select(h => h?.Name));
            warnings.Add("Blight has no effect against monsters in Warrens. Change skills of or remove them: " + heroNames);
        }
        
        var hasCure = team.Slots.Values
            .Where(h => h != null)
            .Any(h => h!.SelectedSkills.Any(s => s.EffectsBitfield.HasFlag(Skill.SkillEffect.Disease)));
        if (!hasCure)
        {
            warnings.Add("Warrens has high disease rates. Your team has no cure skills.");
        }
        return warnings;
    }
}