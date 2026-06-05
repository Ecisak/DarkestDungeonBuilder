using DarkestDungeonBuilder.Models;
using Team = DarkestDungeonBuilder.Models.Team;

namespace DarkestDungeonBuilder.Services;

public class TeamAdvisorService
{
    public AdvisorAnalysis EvaluateTeam(Team team, ILocationStrategy? locationStrategy)
    {
        var analysis = new AdvisorAnalysis();

        if (team.Slots.Values.Any(h => h == null))
        {
            analysis.AddCritical("The team is not complete. Fill all four ranks for a full analysis.");
        }

        if (!HasHealer(team))
        {
            analysis.AddCritical("The team has no reliable healing.");
        }

        var heroesMissingSkills = GetHeroesWithMissingSkills(team);
        if (heroesMissingSkills.Count != 0)
        {
            var formattedNames = heroesMissingSkills.Select(x => $"{x.Hero.Name} (Rank {x.Rank})");
            analysis.AddSuggestion("Some heroes have incomplete loadouts, so the analysis may be inaccurate: " + string.Join(", ", formattedNames) + ".");
        }

        CheckHeroPositions(team, analysis);

        if (locationStrategy != null)
        {
            analysis.Merge(locationStrategy.AnalyzeTeamForLocation(team));
        }

        return analysis;
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
            .Any(hero => hero!.SelectedSkills.Any(skill => skill.EffectsBitfield.HasFlag(Skill.SkillEffect.Heal)));
    }

    private static void CheckHeroPositions(Team team, AdvisorAnalysis analysis)
    {
        foreach (var (currentRank, hero) in team.Slots.Where(s => s.Value != null))
        {
            var scores = hero?.GetPreferredPositions();
            if (scores == null) continue;

            var currentScore = scores[currentRank - 1];
            var maxScore = scores.Max();

            if (maxScore <= 0) continue;

            if (currentScore == 0)
            {
                analysis.AddCritical($"{hero!.Name} cannot use any selected combat skill effectively from rank {currentRank}.");
            }
            else if (currentScore < maxScore)
            {
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

                analysis.AddSuggestion($"{hero!.Name} would perform better in rank {string.Join(" or ", betterRanks)}.");
            }
        }
    }
}
