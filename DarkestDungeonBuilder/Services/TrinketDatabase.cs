using System.Net.Http.Json;
using DarkestDungeonBuilder.Models;

namespace DarkestDungeonBuilder.Services;

public class TrinketDatabase(HttpClient http) : ITrinketDatabase
{
    public async Task<List<Trinket>> GetTrinketsAsync()
    {
        var trinkets = await http.GetFromJsonAsync<List<Trinket>>("data/trinkets.json");
        return trinkets ?? [];
    }
}