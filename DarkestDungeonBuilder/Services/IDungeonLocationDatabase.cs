using DarkestDungeonBuilder.Models;

namespace DarkestDungeonBuilder.Services;

public interface IDungeonLocationDatabase
{
    Task<List<DungeonLocation>> GetLocationsAsync();
}