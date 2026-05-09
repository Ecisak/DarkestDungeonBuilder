namespace DarkestDungeonBuilder.Models;

public interface ILocationStrategy
{
    List<string> AnalyzeTeamForLocation(Team team);
}