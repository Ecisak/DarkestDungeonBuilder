namespace DarkestDungeonBuilder.Models;

public class Hero
{
    public required string Name { get; set; }
    public required string Portrait { get; set; }
    public required string Sprite { get; set; }
    public required string Role { get; set; }
    public int Health { get; set; }

    public List<int> PreferredPositions { get; set; } = [];
    
    public List<Trinket> Trinkets { get; set; } = [];
    
    public List<Skill> Skills { get; set; } = [];
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