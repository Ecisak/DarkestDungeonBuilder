namespace DarkestDungeonBuilder.Models;

public class DungeonLocation
{
    public required string Name { get; set; }
    public required string Image { get; set; }
    public string? Strengths { get; set; }
    public string? Weaknesses { get; set; }
    
    public List<Hero>? RecommendedHeroes { get; set; }

    public Skill.BonusTarget DominantEnemyTypes { get; set; } = Skill.BonusTarget.None;


    public List<Hero> GetRecommendedHeroes(List<Hero> heroes, DungeonLocation location)
    {
        List<Hero> recHeroes = new List<Hero>();

        // Projdeme všechny hrdiny jednoho po druhém
        foreach (var hero in heroes)
        {
            var hasEffectiveSkill = hero.Skills.Any(skill => (skill.BonusAgainst & location.DominantEnemyTypes) != 0);
            
            if (hasEffectiveSkill)
            {
                recHeroes.Add(hero);
            }
        }

        return recHeroes;
    }
}