using DarkestDungeonBuilder.Models;

namespace DarkestDungeonBuilder.Services;

public interface ILocationStrategy
{
    AdvisorAnalysis AnalyzeTeamForLocation(Team team);
}
