namespace DarkestDungeonBuilder.Models;

public class Hero
{
    public string Name { get; set; }
    public string Portrait { get; set; }
    public string Role { get; set; }
    public int Health { get; set; }

    public List<int> PreferredPositions
    {
        get
        {
            //todo logic for preferred position calculation
            return new List<int>();
        }
    }
    public List<Skill> Skills { get; set; } = new List<Skill>();
    public List<Skill> SelectedSkills { get; set; } = new List<Skill>();

    public Hero Clone()
    {
        Hero newHero = new Hero();
        newHero.Name = this.Name;
        newHero.Portrait = this.Portrait;
        newHero.Role = this.Role;
        newHero.Health = this.Health;
        newHero.Skills = new List<Skill>();
        newHero.SelectedSkills = new List<Skill>();

        foreach (Skill skill in this.Skills)
        {
            Skill skillClone = (skill.Clone());
            newHero.Skills.Add(skillClone);
            if (this.SelectedSkills.Contains(skill))
            {
                newHero.SelectedSkills.Add(skillClone);
            }
        }
        return newHero;
    }
}