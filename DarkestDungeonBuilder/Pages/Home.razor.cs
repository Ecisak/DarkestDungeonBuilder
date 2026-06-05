using System.Text.Json;
using System.Text.Json.Serialization;
using DarkestDungeonBuilder.Components;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using DarkestDungeonBuilder.Models;
using DarkestDungeonBuilder.Services;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;

namespace DarkestDungeonBuilder.Pages;

public partial class Home : ComponentBase
{
    [Inject] private IHeroDatabase HeroDb { get; set; } = null!;
    [Inject] private IDialogService DialogService { get; set; } = null!;
    [Inject] private IDungeonLocationDatabase LocationDb { get; set; } = null!;
    [Inject] private ITrinketDatabase TrinketDb { get; set; } = null!;
    [Inject] private Blazored.LocalStorage.ILocalStorageService LocalStorage { get; set; } = null!;
    [Inject] private TeamAdvisorService Advisor { get; set; } = null!;
    [Inject] protected IJSRuntime JsRuntime { get; set; } = null!;
    [Inject] protected ISnackbar Snackbar { get; set; } = null!;
    
    private List<Hero> _roster = null!;
    private List<DungeonLocation> _locations = null!;
    private List<Trinket> _trinkets = null!;
    private Hero? _heroToAssign;
    private DungeonLocation? _selectedLocation;
    private int? _draggedSlotKey;
    private Team _currentTeam = new();
    private List<string> _currentWarnings = [];
    
    protected override async Task OnInitializedAsync()
    {
        _roster = await HeroDb.GetHeroesAsync();
        _locations = await LocationDb.GetLocationsAsync();
        _trinkets = await TrinketDb.GetTrinketsAsync();
        GetHeroesForLocation();
        
        
        try
        {
            _currentTeam = await LocalStorage.GetItemAsync<Team>("currentTeam") ?? new Team();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Cannot load team from LocalStorage: {ex.Message}");
            _currentTeam = new Team();
        }

        if (!string.IsNullOrEmpty(_currentTeam.CurrentLocationName))
        {
            _selectedLocation = _locations.FirstOrDefault(l => l.Name == _currentTeam.CurrentLocationName);
        }
        else
        {
            var savedLocationName = await LocalStorage.GetItemAsync<string>("savedLocation");
            if (!string.IsNullOrEmpty(savedLocationName))
            {
                _selectedLocation = _locations.FirstOrDefault(l => l.Name == savedLocationName);
                _currentTeam.CurrentLocationName = savedLocationName;
            }
        }

        UpdateAdvisor();
    }
    private async Task AddHero(Hero clickedHero, int slotKey)
    {
        await AssignHeroToSlotAsync(clickedHero, slotKey);


        StateHasChanged();
    }

    private async Task SetLocation(DungeonLocation? loc)
    {
        if (loc == null) return;
        _selectedLocation = loc;
        _currentTeam.CurrentLocationName = loc.Name;
        await LocalStorage.SetItemAsync("savedLocation", _selectedLocation.Name);
        await LocalStorage.SetItemAsync("currentTeam", _currentTeam);
        UpdateAdvisor();
    }
    
    private async Task ResetBuilderAsync()
    {
        var result = await DialogService.ShowMessageBoxAsync(
            "Irreversible action", 
            "Do you really want to reset? This action is not reversible..", 
            yesText: "YES", 
            cancelText: "NO"
        );

        if (result != true)
        {
            return; 
        }

        _currentTeam.ClearAll();
    
        _selectedLocation = null; 
    
        await SaveTeam();
        await LocalStorage.RemoveItemAsync("savedLocation");
        
        UpdateAdvisor();
    }
    
    private async Task ExportTeamAsync()
    {
        // setup formating
        var options = new JsonSerializerOptions { WriteIndented = true };
    
        // serialize team into string
        string jsonString = JsonSerializer.Serialize(_currentTeam, options);
    
        // generate name of the file
        string fileName = $"dd_team_{DateTime.Now:yyyyMMdd_HHmmss}.json";
    
        // downloads file
        await JsRuntime.InvokeVoidAsync("downloadJsonFile", fileName, jsonString);
    }
    
    private async Task ImportTeamAsync(InputFileChangeEventArgs e)
    {
        var file = e.File;

        try
        {
            // open stream (limit size for security)
            await using var stream = file.OpenReadStream(maxAllowedSize: 512000);
        
            // set options with convertor for enums and bitfields
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonStringEnumConverter() } 
            };

            // deserialize JSON back to Team object
            var importedTeam = await JsonSerializer.DeserializeAsync<Team>(stream, options);

            if (importedTeam != null)
            {
                // overwrite current team
                _currentTeam = importedTeam;
                
                _selectedLocation = _locations.FirstOrDefault(l => l.Name == _currentTeam.CurrentLocationName);
                
                await SaveTeam();
                
                if (_selectedLocation != null)
                {
                    await LocalStorage.SetItemAsync("savedLocation", _selectedLocation.Name);
                }
            
                UpdateAdvisor();
                Snackbar.Add("Import successful!", Severity.Success);
                StateHasChanged();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Import Error: {ex.Message}\n{ex.StackTrace}");
            await DialogService.ShowMessageBoxAsync(
                "Import Error", 
                $"Failed to load team. Check if the JSON file is valid."
            );
        }
    }

    private async Task ChangeLocation()
    {
        _selectedLocation = null;
        await SaveTeam();
        await LocalStorage.RemoveItemAsync("savedLocation");
        _currentTeam.CurrentLocationName = null;
        StateHasChanged();
    }
    

    private async Task AssignHeroToSlotAsync(Hero? heroToAssign, int targetSlotKey)
    {
        if (heroToAssign == null) return;
        
        var result = _currentTeam.TryAssignHero(heroToAssign, targetSlotKey);

        // if full, pop modal
        if (result == AssignResult.RequiresReplacement)
        {
            var dialogResult = await DialogService.ShowMessageBoxAsync(
                "Slot is full.",
                "The slot and all adjacent slots are full. Do you want to replace the hero with a new one?",
                yesText: "Replace", cancelText: "Cancel");
            
            if (dialogResult == true)
            {
                _currentTeam.ForceReplaceHero(heroToAssign, targetSlotKey);
            }
            else
            {
                return; 
            }
        }
        UpdateAdvisor();
        await SaveTeam();
    }
    
    private async Task HandleDropToRoster()
    {
        if (_draggedSlotKey.HasValue)
        {
            _currentTeam.RemoveHero(_draggedSlotKey.Value);
        
            _draggedSlotKey = null;
        
            await SaveTeam();
            UpdateAdvisor();
        }
    }
    
    private async Task SaveTeam()
    {
        await LocalStorage.SetItemAsync("currentTeam", _currentTeam);
    }
    
    private void GetHeroesForLocation()
    {
        foreach (var location in _locations)
        {
            location.RecommendedHeroes = DungeonLocation.GetRecommendedHeroes(_roster, location);
        }
    }
    private void HandleSlotDragStartFromPanel(Hero hero)
    {
        _heroToAssign = hero;
        _draggedSlotKey = null;
    }

    private async Task HandleAddHeroFromPanel((Hero hero, int slotKey) args)
    {
        await AddHero(args.hero, args.slotKey);
    }

    private async Task HandleDrop(int targetSlotKey)
    {
        // Přetahování nového hrdiny z Rosteru
        if (_heroToAssign != null)
        {
            await AssignHeroToSlotAsync(_heroToAssign, targetSlotKey);
            _heroToAssign = null;
        }
        // Prohazování dvou hrdinů už existujících v týmu
        else if (_draggedSlotKey.HasValue && _draggedSlotKey != targetSlotKey)
        {
            var sourceHero = _currentTeam.Slots[_draggedSlotKey.Value];
            var targetHero = _currentTeam.Slots[targetSlotKey];
        
            _currentTeam.Slots[targetSlotKey] = sourceHero;
            _currentTeam.Slots[_draggedSlotKey.Value] = targetHero;
        
            _draggedSlotKey = null;
            await SaveTeam();
        }

        UpdateAdvisor();
        StateHasChanged();
    }

    private async Task OpenDialogAsync(Hero hero)
    {
        var parameters = new DialogParameters<HeroDetailDialog> { { x => x.Hero, hero } };

        // Tady už je opravená ta mezera z Fáze 1
        var dialog = await DialogService.ShowAsync<HeroDetailDialog>($"Hero setup of {hero.Name}", parameters);

        var result = await dialog.Result;

        if (result is { Canceled: false })
        { 
            await SaveTeam();
            UpdateAdvisor();
            StateHasChanged();
        }
    }

    private void HandleSlotDragStart(int slotKey)
    {
        _draggedSlotKey = slotKey;
        _heroToAssign = null;
    }
    private async Task RemoveHero(int slotKey)
    {
        var result = await DialogService.ShowMessageBoxAsync(
            "Hero removal",
            "Do you want to remove the hero from the roster? All of the settings will be lost.",
            yesText: "Delete", cancelText: "Cancel");

        if (result == true)
        {
            _currentTeam.RemoveHero(slotKey);
        }
        await SaveTeam();
        UpdateAdvisor();
    }
    
    private void UpdateAdvisor()
    {
        
        if (_selectedLocation == null)
        {
            _currentWarnings.Clear();
            return;
        }

        ILocationStrategy? strategy = _selectedLocation.Name switch
        {
            "Ruins" => new RuinsStrategy(),
            "Weald" => new WealdStrategy(),
            "Warrens" => new WarrensStrategy(),
            "Cove" => new CoveStrategy(),
            _ => null
        };
        if (strategy != null) _currentWarnings = Advisor.EvaluateTeam(_currentTeam, strategy);


        StateHasChanged();
    }
}
