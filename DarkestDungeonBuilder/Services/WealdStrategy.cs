using DarkestDungeonBuilder.Models;

namespace DarkestDungeonBuilder.Services;

public class WealdStrategy : ILocationStrategy
{
    public List<string> AnalyzeTeamForLocation(Team team)
    {
        var warnings = new List<string>();

        var hasBlight = team.Slots.Values
            .Where(h => h != null)
            .Where(h => h!.SelectedSkills.Any(s => s.EffectsBitfield.HasFlag(Skill.SkillEffect.Blight)))
            .ToList();
        if (hasBlight.Count <= 0) return warnings;
        {
            var heroNames = string.Join(", ", hasBlight.Select(h => h?.Name));
            warnings.Add("Blight has no effect against monsters in Weald. Change skills of or remove them: " + heroNames);
        }

        return warnings;
    }
}