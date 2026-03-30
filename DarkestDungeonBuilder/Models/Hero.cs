namespace DarkestDungeonBuilder.Models;

public class Hero
{
    public required string Name { get; set; }
    public required string Portrait { get; set; }
    public required string Role { get; set; }
    public int Health { get; set; }
    
    public List<Trinket> Trinkets { get; set; } = [];

    public static List<int> PreferredPositions =>
        //todo logic for preferred position calculation
        [];
    public List<Skill> Skills { get; set; } = [];
    public List<Skill> SelectedSkills { get; init; } = [];

    public Hero Clone()
    {
        var newHero = new Hero
        {
            Name = this.Name,
            Portrait = this.Portrait,
            Role = this.Role,
            Health = this.Health,
            Skills = [],
            SelectedSkills = []
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
}