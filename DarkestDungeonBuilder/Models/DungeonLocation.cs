namespace DarkestDungeonBuilder.Models;

public class DungeonLocation
{
    public required string Name { get; set; }
    public required string Image { get; set; }
    public string? Strengths { get; set; }
    public string? Weaknesses { get; set; }
}