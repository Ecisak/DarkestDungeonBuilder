using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using DarkestDungeonBuilder.Models;

namespace DarkestDungeonBuilder.Services;

public class HeroDatabase : IHeroDatabase
{
    private readonly HttpClient _http;

    public HeroDatabase(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<Hero>> GetHeroesAsync()
    {
        var options = new JsonSerializerOptions
        {
            Converters = { new JsonStringEnumConverter() }
        };

        var heroes = await _http.GetFromJsonAsync<List<Hero>>("data/heroes.json", options);
        return heroes ?? new List<Hero>();
    }
}