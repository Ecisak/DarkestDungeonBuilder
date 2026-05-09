using DarkestDungeonBuilder.Models;

namespace DarkestDungeonBuilder.Services;

public interface ITrinketDatabase
{
    List<Trinket> GetMockTrinkets();
}