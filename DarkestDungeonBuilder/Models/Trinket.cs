namespace DarkestDungeonBuilder.Models;

public class Trinket
{
    public required string Name { get; set; }
    public required string TrinketImage { get; set; }
    public required string Rarity { get; set; }
    public required string Effects { get; set; }
    public string? ClassTrinket { get; set; }
}