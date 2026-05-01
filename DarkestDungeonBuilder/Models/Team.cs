namespace DarkestDungeonBuilder.Models;


public enum AssignResult
{
    Success,
    RequiresReplacement // return, when slot full
}
public class Team
{
    public Dictionary<int, Hero?> Slots { get; set; } = new()
    {
        { 1, null }, 
        { 2, null }, 
        { 3, null }, 
        { 4, null }
    };
    
    public AssignResult TryAssignHero(Hero newHero, int targetSlotKey)
    {
        // slot is empty
        if (Slots[targetSlotKey] == null)
        {
            Slots[targetSlotKey] = newHero.Clone();
            return AssignResult.Success;
        }

        // try left slot
        int leftSlot = targetSlotKey - 1;
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
        Slots[targetSlotKey] = newHero.Clone();
    }
}