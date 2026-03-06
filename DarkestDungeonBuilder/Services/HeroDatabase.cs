using DarkestDungeonBuilder.Models;
namespace DarkestDungeonBuilder.Services;

public class HeroDatabase
{
    public List<Hero> GetInitialRoster()
    {
        var roster = new List<Hero>();
        
        Hero crusader = new Hero();
        crusader.Name = "Crusader";
        crusader.Portrait = "crusader.png";
        crusader.Role = "Warrior";
        crusader.Health = 100;
        crusader.Skills =
        [
            new Skill { Name = "Smite", CastablePositions = new List<int> { 1, 2 }, IsFriendly = false },
            new Skill { Name = "Smite2", CastablePositions = new List<int> { 1, 2 }, IsFriendly = false }
        ];
        roster.Add(crusader);
        return roster;
    }
}
