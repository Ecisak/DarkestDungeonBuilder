using DarkestDungeonBuilder.Models;

namespace DarkestDungeonBuilder.Services;

public interface IHeroDatabase
{
    Task<List<Hero>> GetHeroesAsync(); 
}