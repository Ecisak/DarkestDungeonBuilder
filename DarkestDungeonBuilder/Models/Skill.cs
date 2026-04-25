namespace DarkestDungeonBuilder.Models;

public class Skill
{
    public required string Name { get; init; }
    public required string SkillImage { get; init; }
    public required List<int> CastablePositions { get; init; }
    public required List<int> Targets { get; init; }
    public bool IsFriendly { get; init; }
    public required string Effects { get; init; }

    public SkillCategory Category { get; init; }
    public enum SkillCategory
    {
        Combat,
        Camping
    }

    public Skill Clone()
    {
        return new Skill
        {
            Name = this.Name,
            SkillImage = this.SkillImage,
            CastablePositions = this.CastablePositions,
            Targets = this.Targets,
            IsFriendly = this.IsFriendly,
            Effects = this.Effects, 
            Category = this.Category
        };
    }
}