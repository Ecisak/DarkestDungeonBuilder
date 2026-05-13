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

        var heroesMissingSkills = GetHeroesWithMissingSkills(team);

        if (heroesMissingSkills.Count != 0)
        {
            var formattedNames = heroesMissingSkills.Select(x => $"{x.Hero.Name} (Rank {x.Rank})");

            warnings.Add("These heroes don't have 8 skills selected (4 fighting and 4 camping): " + string.Join(", ", formattedNames) + ".");
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

    private static List<(int Rank, Hero Hero)> GetHeroesWithMissingSkills(Team team)
    {
        return team.Slots
            .Where(slot => slot.Value != null)
            .Where(slot => slot.Value!.SelectedSkills.Count < 8)
            .Select(slot => (Rank: slot.Key, Hero: slot.Value!)) 
            .ToList();
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
        // check all occupied slots
        foreach (var (currentRank, hero) in team.Slots.Where(s => s.Value != null))
        {
            var scores = hero?.GetPreferredPositions();

            if (scores == null) continue;
            var currentScore = scores[currentRank - 1];
            
            var maxScore = scores.Max();

            if (maxScore <= 0) continue;
            if (currentScore == 0)
            {
                warnings.Add($"{hero?.Name} on position {currentRank} cannot use ANY of the selected skills!");
            }
            else if (currentScore < maxScore)
            {
                // okayish position
                // check which positions are better
                var betterRanks = new List<int>();
                for (int i = 0; i < 4; i++)
                {
                    var targetRank = i + 1;
                    var heroScoreIfMoved = scores[i];

                    if (heroScoreIfMoved <= currentScore) continue;
                    var heroInTargetSlot = team.Slots[targetRank];
                    if (heroInTargetSlot == null)
                    {
                        betterRanks.Add(targetRank);
                    }
                    else
                    {
                        var otherHeroScore = heroInTargetSlot.GetPreferredPositions();
                        var currentCombinedScore = currentScore + otherHeroScore[targetRank - 1];
                                
                        var swappedCombinedScore = heroScoreIfMoved + otherHeroScore[currentRank - 1];

                        if (swappedCombinedScore > currentCombinedScore)
                        {
                            betterRanks.Add(targetRank);
                        }
                    }
                }

                if (betterRanks.Count == 0) continue;
                var betterRanksString = string.Join(" or ", betterRanks);
                warnings.Add($"Tip: {hero?.Name} on position {currentRank} can  be better on {betterRanksString} positions.");
            }
        }
    }
}



