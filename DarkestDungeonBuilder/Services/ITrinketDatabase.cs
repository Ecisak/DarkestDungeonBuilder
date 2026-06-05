using DarkestDungeonBuilder.Models;

namespace DarkestDungeonBuilder.Services;

public interface ITrinketDatabase
{
    Task<List<Trinket>> GetTrinketsAsync();
}