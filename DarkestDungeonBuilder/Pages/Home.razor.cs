using DarkestDungeonBuilder.Components;
using Microsoft.AspNetCore.Components;

using Microsoft.AspNetCore.Components;
using MudBlazor;
using DarkestDungeonBuilder.Models;
using DarkestDungeonBuilder.Services;

namespace DarkestDungeonBuilder.Pages;

public partial class Home : ComponentBase
{
    [Inject] private HeroDatabase HeroDb { get; set; } = null!;
    [Inject] private IDialogService DialogService { get; set; } = null!;
    [Inject] private DungeonLocationDatabase LocationDb { get; set; } = null!;
    [Inject] private TrinketDatabase TrinketDb { get; set; } = null!;
    [Inject] private Blazored.LocalStorage.ILocalStorageService LocalStorage { get; set; } = null!;
    
    private List<Hero> _roster = null!;
    private List<DungeonLocation> _locations = null!;
    private List<Trinket> _trinkets = null!;
    private Hero? _heroToAssign;
    private DungeonLocation? _selectedLocation;
    private int? _draggedSlotKey;
    private Team _currentTeam = new Team();
    
    protected override async Task OnInitializedAsync()
    {
        _roster = HeroDb.GetInitialRoster();
        _locations = LocationDb.GetInitialDungeonLocations();
        _trinkets = TrinketDb.GetMockTrinkets();

        var savedLocationName = await LocalStorage.GetItemAsync<string>("savedLocation");
        GetHeroesForLocation();

        if (!string.IsNullOrEmpty(savedLocationName))
        {
            _selectedLocation = _locations.FirstOrDefault(l => l.Name == savedLocationName);
        }

        var savedRoster = await LocalStorage.GetItemAsync<Team>("savedRoster");
        if (savedRoster != null)
        {
            _currentTeam = savedRoster;
        }
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
        await LocalStorage.SetItemAsync("savedLocation", _selectedLocation.Name);
    }

    private async Task RemoveHero(int slotKey)
    {
        var result = await DialogService.ShowMessageBoxAsync(
            "Hero removal",
            "Do you want to remove the hero from the roster? All of the settings will be lost.",
            yesText: "Delete", cancelText: "Cancel");

        if (result == true)
        {
            _currentTeam.Slots[slotKey] = null;
        }

        await LocalStorage.SetItemAsync("savedRoster", _currentTeam);
    }

    private void ChangeLocation()
    {
        _selectedLocation = null;
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
        
        await LocalStorage.SetItemAsync("savedRoster", _currentTeam);
    }

    private void HandleSlotDragStart(int slotKey)
    {
        _draggedSlotKey = slotKey;
        _heroToAssign = null;
    }

    private async Task HandleDrop(int targetSlotKey)
    {
        //dragging new hero
        if (_heroToAssign != null)
        {
            await AssignHeroToSlotAsync(_heroToAssign, targetSlotKey);
            _heroToAssign = null;
        }
        
        // swapping heroes
        else if (_draggedSlotKey.HasValue && _draggedSlotKey != targetSlotKey)
        {
            var sourceHero = _currentTeam.Slots[_draggedSlotKey.Value];
            var targetHero = _currentTeam.Slots[targetSlotKey];
            
            _currentTeam.Slots[targetSlotKey] = sourceHero;
            _currentTeam.Slots[_draggedSlotKey.Value] = targetHero;
            
            _draggedSlotKey = null;
            
            await LocalStorage.SetItemAsync("savedRoster", _currentTeam);
        }


        StateHasChanged();
    }

    private async Task OpenDialogAsync(Hero hero)
    {
        var parameters = new DialogParameters<HeroDetailDialog> { { x => x.Hero, hero } };

        var dialog = await DialogService.ShowAsync<HeroDetailDialog>("Hero setup of" + hero.Name, parameters);

        var result = await dialog.Result;

        if (result is { Canceled: false })
        {
            await LocalStorage.SetItemAsync("savedRoster", _currentTeam);
            StateHasChanged();
        }
    }
    private async Task SaveTeam()
    {
        await LocalStorage.SetItemAsync("savedRoster", _currentTeam);
    }
    
    private void GetHeroesForLocation()
    {
        foreach (var location in _locations)
        {
            location.RecommendedHeroes = location.GetRecommendedHeroes(_roster, location);
        }
    }
}
