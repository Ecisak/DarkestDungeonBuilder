using DarkestDungeonBuilder.Models;
namespace DarkestDungeonBuilder.Services;

public class DungeonLocationDatabase
{
    public List<DungeonLocation> GetInitialDungeonLocations()
    {
        var dungeonLocations = new List<DungeonLocation>();

        var ruins = new DungeonLocation
        {
            Name = "Ruins",
            Image = "/images/mock/bg_ruins.png",
            Strengths = "High Bleed Resist, Heavy Stress Damage",
            Weaknesses = "Vulnerable to Blight, Weak to +Unholy Damage"
        };
        dungeonLocations.Add(ruins);
        var warrens = new DungeonLocation
        {
            Name = "Warrens",
            Image = "/images/mock/bg_warrens.png",
            Strengths = "High Blight Resist, High Disease Chance",
            Weaknesses = "Vulnerable to Bleed, Weak to +Beast Damage"
        };
        dungeonLocations.Add(warrens);
        var weald = new DungeonLocation
        {
            Name = "Weald",
            Image = "/images/mock/bg_weald.png",
            Strengths = "High Blight Resist, High Enemy Dodge & PROT",
            Weaknesses = "Vulnerable to Bleed, Weak to +Human Damage"
        };
        dungeonLocations.Add(weald);
        var cove = new DungeonLocation
        {
            Name = "Cove",
            Image = "/images/mock/bg_cove.png",
            Strengths = "High Bleed Resist, Heavy Enemy Damage & PROT",
            Weaknesses = "Vulnerable to Blight, Weak to +Eldritch Damage"
        };
        dungeonLocations.Add(cove);

        return dungeonLocations;
    }
}