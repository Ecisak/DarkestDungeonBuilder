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
            analysis.AddCritical("The team is incomplete. Fill all four ranks.");
        }

        if (!HasHealer(team))
        {
            analysis.AddCritical("The team has no reliable healer.");
        }

        if (!HasStressHeal(team))
        {
            analysis.AddSuggestion("The team has no stress heal.");
        }

        if (!HasControl(team))
        {
            analysis.AddSuggestion("The team lacks control skills (stun, move, guard).");
        }

        if (!HasBacklineReach(team))
        {
            analysis.AddCritical("Poor reach to enemy back ranks (3-4).");
        }

        if (!HasFrontlinePresence(team))
        {
            analysis.AddCritical("Ranks 1-2 lack a frontline hero.");
        }

        if (!HasEnoughDamageProfiles(team))
        {
            analysis.AddSuggestion("The team has too little direct damage pressure.");
        }

        var heroesMissingSkills = GetHeroesWithMissingSkills(team);
        if (heroesMissingSkills.Count != 0)
        {
            var formattedNames = heroesMissingSkills.Select(x => $"{x.Hero.Name} (Rank {x.Rank})");
            analysis.AddSuggestion("Incomplete loadouts: " + string.Join(", ", formattedNames) + ".");
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

    private static bool HasStressHeal(Team team)
    {
        return team.Slots.Values
            .Where(hero => hero != null)
            .Any(hero => hero!.SelectedSkills.Any(skill => skill.EffectsBitfield.HasFlag(Skill.SkillEffect.StressHeal)));
    }

    private static bool HasControl(Team team)
    {
        return team.Slots.Values
            .Where(hero => hero != null)
            .Any(hero => hero!.SelectedSkills.Any(skill =>
                skill.EffectsBitfield.HasFlag(Skill.SkillEffect.Stun) ||
                skill.EffectsBitfield.HasFlag(Skill.SkillEffect.Move) ||
                skill.EffectsBitfield.HasFlag(Skill.SkillEffect.Guard)));
    }

    private static bool HasBacklineReach(Team team)
    {
        return team.Slots.Values
            .Where(hero => hero != null)
            .Any(hero => hero!.SelectedSkills.Any(skill =>
                skill.Category == Skill.SkillCategory.Combat &&
                !skill.IsFriendly &&
                skill.Targets.Any(target => target is 3 or 4)));
    }

    private static bool HasFrontlinePresence(Team team)
    {
        return team.Slots
            .Where(slot => slot.Key is 1 or 2)
            .Select(slot => slot.Value)
            .Where(hero => hero != null)
            .Any(hero => string.Equals(hero!.Role, "Frontline", StringComparison.OrdinalIgnoreCase));
    }

    private static bool HasEnoughDamageProfiles(Team team)
    {
        var damageCapableHeroes = team.Slots.Values
            .Where(hero => hero != null)
            .Count(hero => hero!.SelectedSkills.Any(skill =>
                skill.Category == Skill.SkillCategory.Combat &&
                (skill.EffectsBitfield.HasFlag(Skill.SkillEffect.Damage) ||
                 skill.EffectsBitfield.HasFlag(Skill.SkillEffect.Bleed) ||
                 skill.EffectsBitfield.HasFlag(Skill.SkillEffect.Blight) ||
                 skill.EffectsBitfield.HasFlag(Skill.SkillEffect.ArmorPiercing))));

        return damageCapableHeroes >= 2;
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
                analysis.AddCritical($"{hero!.Name} cannot use selected combat skills from rank {currentRank}.");
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

                analysis.AddSuggestion($"{hero!.Name} would work better in rank {string.Join(" or ", betterRanks)}.");
            }
        }
    }
}
