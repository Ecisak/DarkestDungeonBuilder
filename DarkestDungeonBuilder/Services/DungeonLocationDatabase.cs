using DarkestDungeonBuilder.Models;
namespace DarkestDungeonBuilder.Services;

public class DungeonLocationDatabase : IDungeonLocationDatabase
{
    public List<DungeonLocation> GetInitialDungeonLocations()
    {
        var dungeonLocations = new List<DungeonLocation>();

        var ruins = new DungeonLocation
        {
            Name = "Ruins",
            Image = "/images/locations/ruins.webp",
            Strengths = "High Bleed Resist, Heavy Stress Damage",
            Weaknesses = "Vulnerable to Blight, Weak to +Unholy Damage",
            DominantEnemyTypes = Skill.BonusTarget.Unholy | Skill.BonusTarget.Human
        };
        dungeonLocations.Add(ruins);
        var warrens = new DungeonLocation
        {
            Name = "Warrens",
            Image = "/images/locations/warrens.webp",
            Strengths = "High Blight Resist, High Disease Chance",
            Weaknesses = "Vulnerable to Bleed, Weak to +Beast Damage",
            DominantEnemyTypes = Skill.BonusTarget.Beast | Skill.BonusTarget.Human
        };
        dungeonLocations.Add(warrens);
        var weald = new DungeonLocation
        {
            Name = "Weald",
            Image = "/images/locations/weald.webp",
            Strengths = "High Blight Resist, High Enemy Dodge & PROT",
            Weaknesses = "Vulnerable to Bleed, Weak to +Human Damage",
            DominantEnemyTypes = Skill.BonusTarget.Eldritch | Skill.BonusTarget.Beast | Skill.BonusTarget.Human
        };
        dungeonLocations.Add(weald);
        var cove = new DungeonLocation
        {
            Name = "Cove",
            Image = "/images/locations/cove.webp",
            Strengths = "High Bleed Resist, Heavy Enemy Damage & PROT",
            Weaknesses = "Vulnerable to Blight, Weak to +Eldritch Damage",
            DominantEnemyTypes = Skill.BonusTarget.Eldritch
        };
        dungeonLocations.Add(cove);

        return dungeonLocations;
    }
}