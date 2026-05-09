namespace DarkestDungeonBuilder.Models;
using DarkestDungeonBuilder.Components;

public class TeamAdvisorService
{
    public List<string> EvaluateTeam(Team team, ILocationStrategy? locationStrategy)
    {
        var warnings = new List<string>();
        
        // basic analysis

        if (team.Slots.Values.Any(h => h == null))
        {
            warnings.Add("Team is not complete, add heroes!");
        }

        if (!HasHealer(team))
        {
            warnings.Add("Team does not have a healer!");
        }
        
        // position analysis
        CheckHeroPositions(team, warnings);
        
        // specific location analysis

        if (locationStrategy != null)
        {
            warnings.AddRange(locationStrategy.AnalyzeTeamForLocation(team));
            
        }

        return warnings;
    }

    private static bool HasHealer(Team team)
    {
        return team.Slots.Values
            .Where(hero => hero != null)
            .Any(hero => hero != null && hero.SelectedSkills
                .Any(skill => skill.EffectsBitfield.HasFlag(Skill.SkillEffect.Heal)));
    }

    private void CheckHeroPositions(Team team, List<string> warnings)
    {
        //TODO: LUKASI Z BUDOUCNOSTI (V DOBE KDY TO BUDES CIST, LUKASI Z PRITOMNOSTI), OPRAV TOHLE PROSIM, LUKAS PRITOMNOSTI (LUKAS MINULOSTI) NA TO DNES UZ NEMA
        
        // check all occupied slots
        foreach (var slot in team.Slots.Where(s => s.Value != null))
        {
            var hero = slot.Value!;
            var currentRank = slot.Key;

            var scores = hero.GetPreferredPositions();
            
            var currentScore = scores[currentRank - 1];
            
            int maxScore = scores.Max();

            if (maxScore > 0)
            {
                if (currentScore == 0)
                {
                    warnings.Add($"{hero.Name} on position {currentRank} cannot use ANY of the selected skills!");
                }
                else if (currentScore < maxScore)
                {
                    // okayish position
                    // check which positions are better
                    var betterRanks = new List<int>();
                    for (int i = 0; i < 4; i++)
                    {
                        if (scores[i] == maxScore) betterRanks.Add(i + 1);
                    }

                    string betterRanksString = string.Join(" or ", betterRanks);
                    warnings.Add($"Tip: {hero.Name} on position {currentRank} can  be better on {betterRanksString} positions.");
                }
            }
            
        }
    }
}



