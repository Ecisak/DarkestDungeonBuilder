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
    private AdvisorAnalysis _advisorAnalysis = new();

    protected override async Task OnInitializedAsync()
    {
        await LoadGameDataAsync();
        var restoredSavedState = await RestoreBuilderStateAsync();

        UpdateAdvisor();

        if (restoredSavedState)
        {
            Snackbar.Add("Restored previous team state.", Severity.Info);
        }
    }

    private async Task LoadGameDataAsync()
    {
        _roster = await HeroDb.GetHeroesAsync();
        _locations = await LocationDb.GetLocationsAsync();
        _trinkets = await TrinketDb.GetTrinketsAsync();
        GetHeroesForLocation();
    }

    private async Task<bool> RestoreBuilderStateAsync()
    {
        var restoredSavedState = await RestoreTeamStateAsync();
        restoredSavedState |= await RestoreSelectedLocationAsync();
        return restoredSavedState;
    }

    private async Task<bool> RestoreTeamStateAsync()
    {
        try
        {
            var storedTeam = await LocalStorage.GetItemAsync<Team>("currentTeam");
            if (storedTeam != null && TryBuildImportedTeam(storedTeam, out var normalizedStoredTeam, out _, out _))
            {
                _currentTeam = normalizedStoredTeam;
                return _currentTeam.Slots.Values.Any(h => h != null) || !string.IsNullOrWhiteSpace(_currentTeam.CurrentLocationName);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Cannot load team from LocalStorage: {ex.Message}");
        }

        _currentTeam = new Team();
        return false;
    }

    private async Task<bool> RestoreSelectedLocationAsync()
    {
        if (!string.IsNullOrEmpty(_currentTeam.CurrentLocationName))
        {
            _selectedLocation = _locations.FirstOrDefault(l => l.Name == _currentTeam.CurrentLocationName);
            return _selectedLocation != null;
        }

        var savedLocationName = await LocalStorage.GetItemAsync<string>("savedLocation");
        if (string.IsNullOrEmpty(savedLocationName))
        {
            return false;
        }

        _selectedLocation = _locations.FirstOrDefault(l => l.Name == savedLocationName);
        if (_selectedLocation == null)
        {
            return false;
        }

        _currentTeam.CurrentLocationName = savedLocationName;
        return true;
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
        await SaveTeam();
        UpdateAdvisor();
        Snackbar.Add($"Location set to {_selectedLocation.Name}.", Severity.Info);
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
        _currentTeam.CurrentLocationName = null;
        _selectedLocation = null;

        await SaveTeam();
        await LocalStorage.RemoveItemAsync("savedLocation");

        UpdateAdvisor();
        Snackbar.Add("Builder reset successfully.", Severity.Success);
    }

    private async Task ExportTeamAsync()
    {
        _currentTeam.SaveVersion = Team.CurrentSaveVersion;

        // setup formating
        var options = new JsonSerializerOptions { WriteIndented = true };

        // serialize team into string
        string jsonString = JsonSerializer.Serialize(_currentTeam, options);

        // generate name of the file
        string fileName = $"dd_team_{DateTime.Now:yyyyMMdd_HHmmss}.json";

        // downloads file
        await JsRuntime.InvokeVoidAsync("downloadJsonFile", fileName, jsonString);
        Snackbar.Add($"Team exported to {fileName}.", Severity.Success);
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

            if (!TryBuildImportedTeam(importedTeam, out var normalizedTeam, out var validationMessage, out var validationWarnings))
            {
                await DialogService.ShowMessageBoxAsync("Import Error", validationMessage);
                return;
            }

            _currentTeam = normalizedTeam;
            _selectedLocation = _locations.FirstOrDefault(l => l.Name == _currentTeam.CurrentLocationName);

            await SaveTeam();

            if (_selectedLocation != null)
            {
                await LocalStorage.SetItemAsync("savedLocation", _selectedLocation.Name);
            }
            else
            {
                await LocalStorage.RemoveItemAsync("savedLocation");
            }

            if (validationWarnings.Count > 0)
            {
                await DialogService.ShowMessageBoxAsync("Import Warnings", string.Join("\n", validationWarnings));
                Snackbar.Add($"Import completed with {validationWarnings.Count} warning(s).", Severity.Warning);
            }
            else
            {
                Snackbar.Add("Import successful!", Severity.Success);
            }

            UpdateAdvisor();
            StateHasChanged();
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
        _currentTeam.CurrentLocationName = null;
        await SaveTeam();
        await LocalStorage.RemoveItemAsync("savedLocation");
        UpdateAdvisor();
        Snackbar.Add("Location cleared. Choose a new destination.", Severity.Info);
        StateHasChanged();
    }


    private async Task AssignHeroToSlotAsync(Hero? heroToAssign, int targetSlotKey)
    {
        if (heroToAssign == null) return;

        var result = _currentTeam.TryAssignHero(heroToAssign, targetSlotKey);

        if (result == AssignResult.InvalidSlot)
        {
            Snackbar.Add("Invalid team slot selected.", Severity.Error);
            return;
        }

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
        _currentTeam.SaveVersion = Team.CurrentSaveVersion;
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
        if (!Team.IsValidSlotKey(targetSlotKey))
        {
            Snackbar.Add("Invalid team slot selected.", Severity.Error);
            return;
        }

        // dargging new hero to the roster
        if (_heroToAssign != null)
        {
            await AssignHeroToSlotAsync(_heroToAssign, targetSlotKey);
            _heroToAssign = null;
        }
        // swapping 2 already existing heroes
        else if (_draggedSlotKey.HasValue && _draggedSlotKey != targetSlotKey && Team.IsValidSlotKey(_draggedSlotKey.Value))
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
            await SaveTeam();
            UpdateAdvisor();
            Snackbar.Add($"Removed hero from rank {slotKey}.", Severity.Info);
        }
    }

    private bool TryBuildImportedTeam(Team? importedTeam, out Team normalizedTeam, out string validationMessage, out List<string> validationWarnings)
    {
        normalizedTeam = new Team();
        validationMessage = string.Empty;
        validationWarnings = [];

        if (!ValidateImportedTeam(importedTeam, out validationMessage, validationWarnings))
        {
            return false;
        }

        if (!BuildImportedSlots(importedTeam!, out var normalizedSlots, out validationMessage, validationWarnings))
        {
            return false;
        }

        if (!ResolveImportedLocation(importedTeam!.CurrentLocationName, out var normalizedLocationName, out validationMessage))
        {
            return false;
        }

        normalizedTeam = new Team
        {
            SaveVersion = Team.CurrentSaveVersion,
            Slots = normalizedSlots,
            CurrentLocationName = normalizedLocationName
        };

        return true;
    }

    private bool ValidateImportedTeam(Team? importedTeam, out string validationMessage, List<string> validationWarnings)
    {
        validationMessage = string.Empty;

        if (importedTeam == null)
        {
            validationMessage = "The selected file does not contain a valid team.";
            return false;
        }

        if (importedTeam.SaveVersion > Team.CurrentSaveVersion)
        {
            validationMessage = $"This save file uses version {importedTeam.SaveVersion}, but the app supports up to version {Team.CurrentSaveVersion}.";
            return false;
        }

        if (importedTeam.SaveVersion <= 0)
        {
            validationWarnings.Add("Legacy save format detected. The team was upgraded to the current save version.");
        }

        if (importedTeam.Slots == null)
        {
            validationMessage = "The imported file is missing team slot data.";
            return false;
        }

        var invalidSlotKeys = importedTeam.Slots.Keys.Where(key => !Team.IsValidSlotKey(key)).OrderBy(key => key).ToList();
        if (invalidSlotKeys.Count > 0)
        {
            validationWarnings.Add($"Ignored invalid slot keys in save file: {string.Join(", ", invalidSlotKeys)}.");
        }

        return true;
    }

    private bool BuildImportedSlots(Team importedTeam, out Dictionary<int, Hero?> normalizedSlots, out string validationMessage, List<string> validationWarnings)
    {
        normalizedSlots = CreateEmptySlots();
        validationMessage = string.Empty;

        foreach (var slotKey in normalizedSlots.Keys.ToList())
        {
            if (!importedTeam.Slots!.TryGetValue(slotKey, out var importedHero) || importedHero == null)
            {
                continue;
            }

            if (!BuildImportedHero(importedHero, slotKey, out var normalizedHero, out validationMessage, validationWarnings))
            {
                return false;
            }

            normalizedSlots[slotKey] = normalizedHero;
        }

        return true;
    }

    private static Dictionary<int, Hero?> CreateEmptySlots() => new()
    {
        [1] = null,
        [2] = null,
        [3] = null,
        [4] = null
    };

    private bool ResolveImportedLocation(string? importedLocationName, out string? normalizedLocationName, out string validationMessage)
    {
        validationMessage = string.Empty;
        normalizedLocationName = null;

        if (string.IsNullOrWhiteSpace(importedLocationName))
        {
            return true;
        }

        var matchedLocation = _locations.FirstOrDefault(l =>
            string.Equals(l.Name, importedLocationName, StringComparison.OrdinalIgnoreCase));

        if (matchedLocation == null)
        {
            validationMessage = $"Unknown location '{importedLocationName}' in imported file.";
            return false;
        }

        normalizedLocationName = matchedLocation.Name;
        return true;
    }

    private bool BuildImportedHero(Hero importedHero, int slotKey, out Hero normalizedHero, out string validationMessage, List<string> validationWarnings)
    {
        validationMessage = string.Empty;
        normalizedHero = null!;

        if (!TryFindRosterHero(importedHero.Name, out var rosterHero))
        {
            validationMessage = $"Unknown hero '{importedHero.Name}' found in slot {slotKey}.";
            return false;
        }

        normalizedHero = rosterHero.Clone();
        ApplyImportedSelectedSkills(importedHero, normalizedHero, slotKey, validationWarnings);
        ApplyImportedTrinkets(importedHero, normalizedHero, rosterHero, slotKey, validationWarnings);
        return true;
    }

    private bool TryFindRosterHero(string heroName, out Hero hero)
    {
        hero = _roster.FirstOrDefault(h => string.Equals(h.Name, heroName, StringComparison.OrdinalIgnoreCase))!;
        return hero != null;
    }

    private void ApplyImportedSelectedSkills(Hero importedHero, Hero normalizedHero, int slotKey, List<string> validationWarnings)
    {
        var importedSelectedSkills = importedHero.SelectedSkills?
            .Where(s => s != null)
            .ToList() ?? [];

        var selectedSkillNames = importedSelectedSkills
            .Select(s => s.Name)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        foreach (var category in Enum.GetValues<Skill.SkillCategory>())
        {
            var categoryNames = selectedSkillNames
                .Where(name => importedSelectedSkills.Any(s => string.Equals(s.Name, name, StringComparison.OrdinalIgnoreCase) && s.Category == category))
                .ToList();

            if (categoryNames.Count > 4)
            {
                validationWarnings.Add($"{normalizedHero.Name} in slot {slotKey} had more than 4 {category.ToString().ToLowerInvariant()} skills. Extra skills were ignored.");
            }

            foreach (var skillName in categoryNames.Take(4))
            {
                var canonicalSkill = normalizedHero.Skills.FirstOrDefault(s =>
                    s.Category == category && string.Equals(s.Name, skillName, StringComparison.OrdinalIgnoreCase));

                if (canonicalSkill == null)
                {
                    validationWarnings.Add($"Ignored unknown skill '{skillName}' on {normalizedHero.Name} in slot {slotKey}.");
                    continue;
                }

                normalizedHero.SelectedSkills.Add(canonicalSkill);
            }
        }
    }

    private void ApplyImportedTrinkets(Hero importedHero, Hero normalizedHero, Hero rosterHero, int slotKey, List<string> validationWarnings)
    {
        var normalizedTrinkets = new Trinket?[2];
        var equippedTrinketNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var importedTrinkets = importedHero.EquippedTrinkets ?? [];

        if (importedTrinkets.Length > 2)
        {
            validationWarnings.Add($"{rosterHero.Name} in slot {slotKey} had more than 2 trinket slots in the imported file. Extra slots were ignored.");
        }

        for (int i = 0; i < Math.Min(2, importedTrinkets.Length); i++)
        {
            var canonicalTrinket = ResolveImportedTrinket(importedTrinkets[i], rosterHero, equippedTrinketNames, slotKey, validationWarnings);
            if (canonicalTrinket != null)
            {
                normalizedTrinkets[i] = canonicalTrinket;
            }
        }

        normalizedHero.EquippedTrinkets = normalizedTrinkets;
    }

    private Trinket? ResolveImportedTrinket(Trinket? importedTrinket, Hero rosterHero, HashSet<string> equippedTrinketNames, int slotKey, List<string> validationWarnings)
    {
        if (importedTrinket == null)
        {
            return null;
        }

        var canonicalTrinket = _trinkets.FirstOrDefault(t => string.Equals(t.Name, importedTrinket.Name, StringComparison.OrdinalIgnoreCase));
        if (canonicalTrinket == null)
        {
            validationWarnings.Add($"Ignored unknown trinket '{importedTrinket.Name}' on {rosterHero.Name} in slot {slotKey}.");
            return null;
        }

        if (!equippedTrinketNames.Add(canonicalTrinket.Name))
        {
            validationWarnings.Add($"Ignored duplicate trinket '{canonicalTrinket.Name}' on {rosterHero.Name} in slot {slotKey}.");
            return null;
        }

        if (!string.IsNullOrWhiteSpace(canonicalTrinket.ClassTrinket) &&
            !string.Equals(canonicalTrinket.ClassTrinket, rosterHero.Name, StringComparison.OrdinalIgnoreCase))
        {
            validationWarnings.Add($"Ignored class trinket '{canonicalTrinket.Name}' on {rosterHero.Name} because it belongs to {canonicalTrinket.ClassTrinket}.");
            return null;
        }

        return canonicalTrinket;
    }

    private void UpdateAdvisor()
    {

        if (_selectedLocation == null)
        {
            _advisorAnalysis = new AdvisorAnalysis();
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
        _advisorAnalysis = strategy != null
            ? Advisor.EvaluateTeam(_currentTeam, strategy)
            : new AdvisorAnalysis();

        StateHasChanged();
    }
}
