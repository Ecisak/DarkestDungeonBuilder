namespace DarkestDungeonBuilder.Models;

public class Skill
{
    public required string Name { get; init; }
    public required string SkillImage { get; init; }
    public required List<int> CastablePositions { get; init; }
    public required List<int> Targets { get; init; }
    public bool IsFriendly { get; init; }
    public required string Effects { get; init; }

    public SkillCategory Category { get; init; }
    
    [Flags]
    public enum SkillEffect
    {
        None = 0,
    
        // --- basics ---
        Damage = 1 << 0,       // 1
        Heal = 1 << 1,         // 2
        StressHeal = 1 << 2,   // 4
        StressDamage = 1 << 3, // 8 
    
        // --- Stats and modificators ---
        Buff = 1 << 4,         // 16
        Debuff = 1 << 5,       // 32
    
        // --- Status and DoT ---
        Stun = 1 << 6,         // 64
        Blight = 1 << 7,       // 128
        Bleed = 1 << 8,        // 256
        Horror = 1 << 9,       // 512 
        Disease = 1 << 10,     // 1024
    
        // --- tactics ---
        Move = 1 << 11,    
        Mark = 1 << 12,    
        Guard = 1 << 13,      
        Riposte = 1 << 14,    
        Stealth = 1 << 15,   
        DeStealth = 1 << 16,  
        ArmorPiercing = 1 << 17,
        
        // --- Utility ---
        Cure = 1 << 18,        
        Torch = 1 << 19,
        ClearCorpse = 1 << 20
    }
    
    [Flags]
    public enum BonusTarget
    {
        None = 0,

        // --- Types of monsters ---
        Unholy = 1 << 0,       
        Eldritch = 1 << 1,
        Beast = 1 << 2,        
        Human = 1 << 3,        
        Bloodsucker = 1 << 4,  
        Husk = 1 << 5,         

        // --- Status of enemies ---
        Marked = 1 << 6,       
        Stunned = 1 << 7,      
        Blighted = 1 << 8,     
        Bleeding = 1 << 9      
    }
    public enum SkillCategory
    {
        Combat,
        Camping
    }
    public SkillEffect EffectsBitfield { get; init; }
    
    public BonusTarget BonusAgainst { get; init; } = BonusTarget.None;

    public Skill Clone()
    {
        return new Skill
        {
            Name = this.Name,
            SkillImage = this.SkillImage,
            CastablePositions = this.CastablePositions,
            Targets = this.Targets,
            IsFriendly = this.IsFriendly,
            Effects = this.Effects, 
            Category = this.Category,
            BonusAgainst = this.BonusAgainst,
            EffectsBitfield = this.EffectsBitfield
        };
    }
}