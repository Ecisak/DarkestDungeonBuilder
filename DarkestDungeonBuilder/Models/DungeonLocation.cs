namespace DarkestDungeonBuilder.Models;

public class DungeonLocation
{
    public required string Name { get; set; }
    public required string Image { get; set; }
    public string? Strengths { get; set; }
    public string? Weaknesses { get; set; }

    public List<Hero>? RecommendedHeroes { get; set; }

    public Skill.BonusTarget DominantEnemyTypes { get; set; } = Skill.BonusTarget.None;


    public static List<Hero> GetRecommendedHeroes(List<Hero> heroes, DungeonLocation location)
    {
        return (from hero in heroes let hasEffectiveSkill = hero.Skills.Any(skill => (skill.BonusAgainst & location.DominantEnemyTypes) != 0) where hasEffectiveSkill select hero).ToList();
    }
}
