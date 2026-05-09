using DarkestDungeonBuilder.Models;

namespace DarkestDungeonBuilder.Services;

public interface IHeroDatabase
{
    List<Hero> GetInitialRoster();
}