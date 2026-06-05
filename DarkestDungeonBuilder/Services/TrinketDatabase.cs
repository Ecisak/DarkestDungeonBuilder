using System.Net.Http.Json;
using DarkestDungeonBuilder.Models;

namespace DarkestDungeonBuilder.Services;

public class TrinketDatabase : ITrinketDatabase
{
    private readonly HttpClient _http;

    public TrinketDatabase(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<Trinket>> GetTrinketsAsync()
    {
        var trinkets = await _http.GetFromJsonAsync<List<Trinket>>("data/trinkets.json");
        return trinkets ?? new List<Trinket>();
    }
}