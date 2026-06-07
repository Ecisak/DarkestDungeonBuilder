using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using DarkestDungeonBuilder.Models;

namespace DarkestDungeonBuilder.Services;

public class HeroDatabase(HttpClient http) : IHeroDatabase
{
    public async Task<List<Hero>> GetHeroesAsync()
    {
        var options = new JsonSerializerOptions
        {
            Converters = { new JsonStringEnumConverter() }
        };

        var heroes = await http.GetFromJsonAsync<List<Hero>>("data/heroes.json", options);
        return heroes ?? new List<Hero>();
    }
}