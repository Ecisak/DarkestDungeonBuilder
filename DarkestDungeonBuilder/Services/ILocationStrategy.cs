using DarkestDungeonBuilder.Models;

namespace DarkestDungeonBuilder.Services;

public interface ILocationStrategy
{
    List<string> AnalyzeTeamForLocation(Team team);
}