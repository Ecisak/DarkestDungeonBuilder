using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using DarkestDungeonBuilder;
using DarkestDungeonBuilder.Services;
using MudBlazor.Services;
using Blazored.LocalStorage;
using DarkestDungeonBuilder.Models;


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddMudServices();

builder.Services.AddScoped<IHeroDatabase, HeroDatabase>();
builder.Services.AddScoped<IDungeonLocationDatabase, DungeonLocationDatabase>();
builder.Services.AddScoped<ITrinketDatabase, TrinketDatabase>();
builder.Services.AddScoped<TeamAdvisorService>();

builder.Services.AddBlazoredLocalStorage();

await builder.Build().RunAsync();
