namespace DarkestDungeonBuilder.Models;

public class Skill
{
    public string Name { get; set; }
    public string SkillImage { get; set; }
    public List<int> CastablePositions { get; set; }
    public List<int> Targets { get; set; }
    public bool IsFriendly { get; set; }
    public string Effects { get; set; }

    public Skill Clone()
    {
        return new Skill
        {
            Name = this.Name,
            SkillImage = this.SkillImage,
            CastablePositions = this.CastablePositions,
            Targets = this.Targets,
            IsFriendly = this.IsFriendly,
            Effects = this.Effects
        };
    }
}