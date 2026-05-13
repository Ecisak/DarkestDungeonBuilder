using DarkestDungeonBuilder.Models;

namespace DarkestDungeonBuilder.Services;

public class RuinsStrategy : ILocationStrategy
{
    public List<string> AnalyzeTeamForLocation(Team team)
    {
        var warnings = new List<string>();

        var hasBleed = team.Slots.Values
            .Where(h => h != null)
            .Any(h => h != null && h.SelectedSkills.Any(s => s.EffectsBitfield.HasFlag(Skill.SkillEffect.Bleed)));

        if (hasBleed)
        {
            warnings.Add("Most of the enemies are unholy, bleed doesn't work on them");
        }

        return warnings;
    }
}