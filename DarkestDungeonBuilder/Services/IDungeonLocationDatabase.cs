using DarkestDungeonBuilder.Models;

namespace DarkestDungeonBuilder.Services;

public interface IDungeonLocationDatabase
{
    List<DungeonLocation> GetInitialDungeonLocations();
}