using DarkestDungeonBuilder.Models;
namespace DarkestDungeonBuilder.Services;

public class HeroDatabase
{
    public List<Hero> GetInitialRoster()
    {
        var roster = new List<Hero>();
        
        var crusader = new Hero
        {
            Name = "Crusader",
            Portrait = "crusader.png",
            Role = "Warrior",
            Health = 100,
            Skills =
            [
                new Skill
                {
                    Name = "Smite",
                    CastablePositions =
                    [
                        1,
                        2
                    ],
                    IsFriendly = false,
                    SkillImage = null,
                    Targets = null,
                    Effects = null
                },
                new Skill
                {
                    Name = "Smite2",
                    CastablePositions = new List<int>
                    {
                        1,
                        2
                    },
                    IsFriendly = false,
                    SkillImage = null,
                    Targets = null,
                    Effects = null
                }
            ]
        };
        roster.Add(crusader);
        return roster;
    }
}
