using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using DarkestDungeonBuilder.Models;

namespace DarkestDungeonBuilder.Services;

public class DungeonLocationDatabase : IDungeonLocationDatabase
{
    private readonly HttpClient _http;

    public DungeonLocationDatabase(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<DungeonLocation>> GetLocationsAsync()
    {
        var options = new JsonSerializerOptions
        {
            Converters = { new JsonStringEnumConverter() }
        };

        var locations = await _http.GetFromJsonAsync<List<DungeonLocation>>("data/locations.json", options);
        return locations ?? new List<DungeonLocation>();
    }
}