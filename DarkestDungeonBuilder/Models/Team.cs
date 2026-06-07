namespace DarkestDungeonBuilder.Models;


public enum AssignResult
{
    Success,
    RequiresReplacement, // return, when slot full
    InvalidSlot
}
public class Team
{
    public const int CurrentSaveVersion = 1;

    public int SaveVersion { get; set; } = CurrentSaveVersion;

    public Dictionary<int, Hero?> Slots { get; set; } = new()
    {
        { 1, null },
        { 2, null },
        { 3, null },
        { 4, null }
    };

    public string? CurrentLocationName { get; set; }

    public static bool IsValidSlotKey(int slotKey) => slotKey is >= 1 and <= 4;

    public AssignResult TryAssignHero(Hero newHero, int targetSlotKey)
    {
        if (!IsValidSlotKey(targetSlotKey))
        {
            return AssignResult.InvalidSlot;
        }

        // slot is empty
        if (Slots[targetSlotKey] == null)
        {
            Slots[targetSlotKey] = newHero.Clone();
            return AssignResult.Success;
        }

        // try left slot
        var leftSlot = targetSlotKey - 1;
        if (leftSlot >= 1 && Slots[leftSlot] == null)
        {
            Slots[leftSlot] = Slots[targetSlotKey];
            Slots[targetSlotKey] = newHero.Clone();
            return AssignResult.Success;
        }

        // try right slot
        int rightSlot = targetSlotKey + 1;
        if (rightSlot <= 4 && Slots[rightSlot] == null)
        {
            Slots[rightSlot] = Slots[targetSlotKey];
            Slots[targetSlotKey] = newHero.Clone();
            return AssignResult.Success;
        }

        // space none, need to pop modal
        return AssignResult.RequiresReplacement;
    }

    // helper method, when user wants to replace the hero
    public void ForceReplaceHero(Hero newHero, int targetSlotKey)
    {
        if (!IsValidSlotKey(targetSlotKey))
        {
            return;
        }

        Slots[targetSlotKey] = newHero.Clone();
    }

    public void RemoveHero(int slotKey)
    {
        if (!IsValidSlotKey(slotKey))
        {
            return;
        }

        Slots[slotKey] = null;
    }

    public void ClearAll()
    {
        foreach (var key in Slots.Keys.ToList())
        {
            Slots[key] = null;
        }
    }
}
