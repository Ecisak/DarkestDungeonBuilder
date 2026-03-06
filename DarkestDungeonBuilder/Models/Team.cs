namespace DarkestDungeonBuilder.Models;

public class Team
{
    public Dictionary<int, Hero?> Slots { get; set; } = new()
    {
        { 1, null }, 
        { 2, null }, 
        { 3, null }, 
        { 4, null }
    };
    
    // TODO: methods addhero etc.
}