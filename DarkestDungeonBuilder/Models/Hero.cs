namespace DarkestDungeonBuilder.Models;

public class Hero : IPrototype<Hero>
{
    public required string Name { get; init; }
    public required string Portrait { get; init; }
    public required string Sprite { get; init; }
    public required string Role { get; init; }
    public int Health { get; init; }

    public List<int> PreferredPositions { get; set; } = [];
    
    public Trinket?[] EquippedTrinkets { get; set; } = new Trinket?[2];    
    public List<Skill> Skills { get; init; } = [];
    public List<Skill> SelectedSkills { get; set; } = [];

    public Hero Clone()
    {
        var newHero = new Hero
        {
            Name = this.Name,
            Portrait = this.Portrait,
            Sprite = this.Sprite,
            Role = this.Role,
            Health = this.Health,
            Skills = [],
            SelectedSkills = [],
            EquippedTrinkets = new Trinket?[2]
        };

        foreach (var skill in this.Skills)
        {
            var skillClone = (skill.Clone());
            newHero.Skills.Add(skillClone);
            if (this.SelectedSkills.Contains(skill))
            {
                newHero.SelectedSkills.Add(skillClone);
            }
        }
        return newHero;
    }
    
    
    public List<int> GetPreferredPositions()
    {
        List<int> positionsScore = [0, 0, 0, 0];
        if (SelectedSkills.Count == 0) return positionsScore;

        foreach (var position in SelectedSkills
                     .Where(s => s.Category == Skill.SkillCategory.Combat)
                     .SelectMany(skill => skill.CastablePositions))
        {
            if (position is >= 1 and <= 4)
            {
                positionsScore[position - 1]++;
            }
        }

        return positionsScore;
    }
    

}