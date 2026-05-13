using DarkestDungeonBuilder.Models;
namespace DarkestDungeonBuilder.Services;

public class HeroDatabase : IHeroDatabase
{
    public List<Hero> GetInitialRoster()
    {
        var roster = new List<Hero>();

        // --- 1. CRUSADER ---
        var crusader = new Hero
        {
            Name = "Crusader",
            Portrait = "/images/heroes/crusader/crusader_icon.png",
            Sprite = "/images/heroes/crusader/crusader_sprite.webp",
            Role = "Frontline",
            Health = 33,
            Skills =
            [
                // COMBAT SKILLS
                new Skill
                {
                    Name = "Smite", SkillImage = "/images/heroes/crusader/spells/smite.webp",
                    CastablePositions = [1, 2], Targets = [1, 2],
                    IsFriendly = false, Effects = "Damage to front lines",
                    EffectsBitfield = Skill.SkillEffect.Damage,
                    BonusAgainst = Skill.BonusTarget.Unholy, // <-- Přidaný bonus
                    Category = Skill.SkillCategory.Combat
                },
                new Skill
                {
                    Name = "Zealous Accusation", SkillImage = "/images/heroes/crusader/spells/zealous_accusation.webp",
                    CastablePositions = [1, 2], Targets = [1, 2],
                    IsFriendly = false, Effects = "AoE Damage",
                    EffectsBitfield = Skill.SkillEffect.Damage,
                    Category = Skill.SkillCategory.Combat
                },
                new Skill
                {
                    Name = "Stunning Blow", SkillImage = "/images/heroes/crusader/spells/stunning_blow.webp",
                    CastablePositions = [1, 2], Targets = [1, 2],
                    IsFriendly = false, Effects = "Damage + Stun",
                    EffectsBitfield = Skill.SkillEffect.Damage | Skill.SkillEffect.Stun,
                    Category = Skill.SkillCategory.Combat
                },
                new Skill
                {
                    Name = "Bulwark of Faith", SkillImage = "/images/heroes/crusader/spells/bulwark_of_faith.webp",
                    CastablePositions = [1, 2], Targets = [1], // Self
                    IsFriendly = true, Effects = "Prot Buff + Mark",
                    EffectsBitfield = Skill.SkillEffect.Buff | Skill.SkillEffect.Mark | Skill.SkillEffect.Torch,
                    Category = Skill.SkillCategory.Combat
                },
                new Skill
                {
                    Name = "Battle Heal", SkillImage = "/images/heroes/crusader/spells/battle_heal.webp",
                    CastablePositions = [1, 2, 3, 4], Targets = [1, 2, 3, 4],
                    IsFriendly = true, Effects = "Small Heal",
                    EffectsBitfield = Skill.SkillEffect.Heal,
                    Category = Skill.SkillCategory.Combat
                },
                new Skill
                {
                    Name = "Holy Lance", SkillImage = "/images/heroes/crusader/spells/holy_lance.webp",
                    CastablePositions = [3, 4], Targets = [2, 3, 4],
                    IsFriendly = false, Effects = "Damage + Move Forward 1",
                    EffectsBitfield = Skill.SkillEffect.Damage | Skill.SkillEffect.Move,
                    BonusAgainst = Skill.BonusTarget.Unholy, // <-- Přidaný bonus
                    Category = Skill.SkillCategory.Combat
                },
                new Skill
                {
                    Name = "Inspiring Cry", SkillImage = "/images/heroes/crusader/spells/inspiring_cry.webp",
                    CastablePositions = [1, 2, 3, 4], Targets = [1, 2, 3, 4],
                    IsFriendly = true, Effects = "Heal + Stress Heal + Torch",
                    EffectsBitfield = Skill.SkillEffect.Heal | Skill.SkillEffect.StressHeal | Skill.SkillEffect.Torch,
                    Category = Skill.SkillCategory.Combat
                },

                // CAMPING SKILLS
                new Skill
                {
                    Name = "Zealous Vigil", SkillImage = "/images/heroes/crusader/spells/zealous_vigil.webp",
                    CastablePositions = [1, 2, 3, 4], Targets = [1], // Self
                    IsFriendly = true, Effects = "Stress Heal, Prevents Nighttime Ambush",
                    EffectsBitfield = Skill.SkillEffect.StressHeal | Skill.SkillEffect.Buff,
                    Category = Skill.SkillCategory.Camping
                },
                new Skill
                {
                    Name = "Zealous Speech", SkillImage = "/images/heroes/crusader/spells/zealous_speech.webp",
                    CastablePositions = [1, 2, 3, 4], Targets = [1, 2, 3, 4], // Party
                    IsFriendly = true, Effects = "Party Stress Heal + Stress Resist",
                    EffectsBitfield = Skill.SkillEffect.StressHeal | Skill.SkillEffect.Buff,
                    Category = Skill.SkillCategory.Camping
                },
                new Skill
                {
                    Name = "Stand Tall", SkillImage = "/images/heroes/crusader/spells/stand_tall.webp",
                    CastablePositions = [1, 2, 3, 4], Targets = [1, 2, 3, 4], // One Ally
                    IsFriendly = true, Effects = "Stress Heal + Remove Mortality Debuffs",
                    EffectsBitfield = Skill.SkillEffect.StressHeal | Skill.SkillEffect.Buff | Skill.SkillEffect.Cure,
                    Category = Skill.SkillCategory.Camping
                },
                new Skill
                {
                    Name = "Unshakeable Leader", SkillImage = "/images/heroes/crusader/spells/unshakeable_leader.webp",
                    CastablePositions = [1, 2, 3, 4], Targets = [1], // Self
                    IsFriendly = true, Effects = "Stress Resist",
                    EffectsBitfield = Skill.SkillEffect.Buff,
                    Category = Skill.SkillCategory.Camping
                }
            ]
        };
        roster.Add(crusader);

        // --- 2. PLAGUE DOCTOR ---
        var plagueDoctor = new Hero
        {
            Name = "Plague Doctor",
            Portrait = "/images/heroes/plague_doctor/pd_hero.png",
            Sprite = "/images/heroes/plague_doctor/pd_sprite.webp",
            Role = "Support",
            Health = 22,
            Skills =
            [
                // COMBAT SKILLS
                new Skill
                {
                    Name = "Noxious Blast", SkillImage = "/images/heroes/plague_doctor/spells/noxious_blast.webp",
                    CastablePositions = [2, 3, 4], Targets = [1, 2],
                    IsFriendly = false, Effects = "Damage + Blight",
                    EffectsBitfield = Skill.SkillEffect.Damage | Skill.SkillEffect.Blight,
                    Category = Skill.SkillCategory.Combat
                },
                new Skill
                {
                    Name = "Plague Grenade", SkillImage = "/images/heroes/plague_doctor/spells/plague_grenade.webp",
                    CastablePositions = [3, 4], Targets = [3, 4],
                    IsFriendly = false, Effects = "AoE Blight",
                    EffectsBitfield = Skill.SkillEffect.Blight,
                    Category = Skill.SkillCategory.Combat
                },
                new Skill
                {
                    Name = "Blinding Gas", SkillImage = "/images/heroes/plague_doctor/spells/blinding_gas.webp",
                    CastablePositions = [3, 4], Targets = [3, 4],
                    IsFriendly = false, Effects = "AoE Stun",
                    EffectsBitfield = Skill.SkillEffect.Stun,
                    Category = Skill.SkillCategory.Combat
                },
                new Skill
                {
                    Name = "Incision", SkillImage = "/images/heroes/plague_doctor/spells/incision.webp",
                    CastablePositions = [1, 2, 3], Targets = [1, 2],
                    IsFriendly = false, Effects = "Damage + Bleed",
                    EffectsBitfield = Skill.SkillEffect.Damage | Skill.SkillEffect.Bleed,
                    Category = Skill.SkillCategory.Combat
                },
                new Skill
                {
                    Name = "Battlefield Medicine", SkillImage = "/images/heroes/plague_doctor/spells/battlefield_medicine.webp",
                    CastablePositions = [3, 4], Targets = [1, 2, 3, 4],
                    IsFriendly = true, Effects = "Heal + Cure Bleed/Blight",
                    EffectsBitfield = Skill.SkillEffect.Heal | Skill.SkillEffect.Cure,
                    Category = Skill.SkillCategory.Combat
                },
                new Skill
                {
                    Name = "Emboldening Vapours", SkillImage = "/images/heroes/plague_doctor/spells/emboldening_vapours.webp",
                    CastablePositions = [1, 2, 3, 4], Targets = [1, 2, 3, 4],
                    IsFriendly = true, Effects = "Damage Buff + SPD Buff",
                    EffectsBitfield = Skill.SkillEffect.Buff,
                    Category = Skill.SkillCategory.Combat
                },
                new Skill
                {
                    Name = "Disorienting Blast", SkillImage = "/images/heroes/plague_doctor/spells/disorienting_blast.webp",
                    CastablePositions = [2, 3, 4], Targets = [2, 3, 4],
                    IsFriendly = false, Effects = "Stun + Shuffle + Clear Corpses",
                    EffectsBitfield = Skill.SkillEffect.Stun | Skill.SkillEffect.Move | Skill.SkillEffect.ClearCorpse,
                    Category = Skill.SkillCategory.Combat
                },

                // CAMPING SKILLS
                new Skill
                {
                    Name = "Experimental Vapours", SkillImage = "/images/heroes/plague_doctor/spells/experimental_vapours.webp",
                    CastablePositions = [1, 2, 3, 4], Targets = [1, 2, 3, 4],
                    IsFriendly = true, Effects = "Huge Heal on One Ally",
                    EffectsBitfield = Skill.SkillEffect.Heal | Skill.SkillEffect.Buff,
                    Category = Skill.SkillCategory.Camping
                },
                new Skill
                {
                    Name = "Leeches", SkillImage = "/images/heroes/plague_doctor/spells/leeches.webp",
                    CastablePositions = [1, 2, 3, 4], Targets = [1, 2, 3, 4],
                    IsFriendly = true, Effects = "Heal + Cure Disease/Blight",
                    EffectsBitfield = Skill.SkillEffect.Heal | Skill.SkillEffect.Disease | Skill.SkillEffect.Cure,
                    Category = Skill.SkillCategory.Camping
                },
                new Skill
                {
                    Name = "Self-Medicate", SkillImage = "/images/heroes/plague_doctor/spells/self-medicate.webp",
                    CastablePositions = [1, 2, 3, 4], Targets = [1], // Self
                    IsFriendly = true, Effects = "Heal + Disease Resist",
                    EffectsBitfield = Skill.SkillEffect.Heal | Skill.SkillEffect.Buff,
                    Category = Skill.SkillCategory.Camping
                },
                new Skill
                {
                    Name = "The Cure", SkillImage = "/images/heroes/plague_doctor/spells/the_cure.webp",
                    CastablePositions = [1, 2, 3, 4], Targets = [1], // Self
                    IsFriendly = true, Effects = "Cure Disease on Self",
                    EffectsBitfield = Skill.SkillEffect.Disease | Skill.SkillEffect.Cure,
                    Category = Skill.SkillCategory.Camping
                }
            ]
        };
        roster.Add(plagueDoctor);
        
        // --- 3. HIGHWAYMAN ---
        var highwayman = new Hero
        {
            Name = "Highwayman",
            Portrait = "/images/heroes/highwayman/hwm-icon.webp",
            Sprite = "/images/heroes/highwayman/hwm-sprite.webp",
            Role = "Damage",
            Health = 23,
            Skills =
            [
                // COMBAT SKILLS
                new Skill
                {
                    Name = "Wicked Slice", SkillImage = "/images/heroes/highwayman/spells/wicked_slice.webp",
                    CastablePositions = [1, 2, 3], Targets = [1, 2],
                    IsFriendly = false, Effects = "High Melee Damage",
                    EffectsBitfield = Skill.SkillEffect.Damage,
                    Category = Skill.SkillCategory.Combat
                },
                new Skill
                {
                    Name = "Pistol Shot", SkillImage = "/images/heroes/highwayman/spells/pistol_shot.webp",
                    CastablePositions = [2, 3, 4], Targets = [2, 3, 4],
                    IsFriendly = false, Effects = "Ranged Damage",
                    EffectsBitfield = Skill.SkillEffect.Damage,
                    BonusAgainst = Skill.BonusTarget.Marked,
                    Category = Skill.SkillCategory.Combat
                },
                new Skill
                {
                    Name = "Point Blank Shot", SkillImage = "/images/heroes/highwayman/spells/point_blank_shot.webp",
                    CastablePositions = [1], Targets = [1],
                    IsFriendly = false, Effects = "Massive Damage + Push Self Back 1",
                    EffectsBitfield = Skill.SkillEffect.Damage | Skill.SkillEffect.Move,
                    Category = Skill.SkillCategory.Combat
                },
                new Skill
                {
                    Name = "Duelist's Advance", SkillImage = "/images/heroes/highwayman/spells/duelists_advance.webp",
                    CastablePositions = [2, 3, 4], Targets = [1, 2, 3],
                    IsFriendly = false, Effects = "Damage + Move Forward 1 + Riposte",
                    EffectsBitfield = Skill.SkillEffect.Damage | Skill.SkillEffect.Move | Skill.SkillEffect.Riposte,
                    Category = Skill.SkillCategory.Combat
                },
                new Skill
                {
                    Name = "Tracking Shot", SkillImage = "/images/heroes/highwayman/spells/tracking_shot.webp",
                    CastablePositions = [2, 3, 4], Targets = [1, 2, 3, 4],
                    IsFriendly = false, Effects = "Buff Damage/Crit + De-Stealth",
                    EffectsBitfield = Skill.SkillEffect.Buff | Skill.SkillEffect.DeStealth,
                    Category = Skill.SkillCategory.Combat
                },
                new Skill
                {
                    Name = "Grapeshot Blast", SkillImage = "/images/heroes/highwayman/spells/grapeshot_blast.webp",
                    CastablePositions = [2, 3], Targets = [1, 2, 3],
                    IsFriendly = false, Effects = "AoE Damage",
                    EffectsBitfield = Skill.SkillEffect.Damage,
                    Category = Skill.SkillCategory.Combat
                },
                new Skill
                {
                    Name = "Open Vein", SkillImage = "/images/heroes/highwayman/spells/open_vein.webp",
                    CastablePositions = [1, 2, 3], Targets = [1, 2],
                    IsFriendly = false, Effects = "Damage + Bleed + Debuff Speed",
                    EffectsBitfield = Skill.SkillEffect.Damage | Skill.SkillEffect.Bleed | Skill.SkillEffect.Debuff,
                    Category = Skill.SkillCategory.Combat
                },

                // CAMPING SKILLS
                new Skill
                {
                    Name = "Bandit's Sense", SkillImage = "/images/heroes/highwayman/spells/bandits_sense.webp",
                    CastablePositions = [1, 2, 3, 4], Targets = [1, 2, 3, 4],
                    IsFriendly = true, Effects = "Prevents Nighttime Ambush",
                    EffectsBitfield = Skill.SkillEffect.Buff,
                    Category = Skill.SkillCategory.Camping
                },
                new Skill
                {
                    Name = "Clean Guns", SkillImage = "/images/heroes/highwayman/spells/clean_guns.webp",
                    CastablePositions = [1, 2, 3, 4], Targets = [1], // Self
                    IsFriendly = true, Effects = "Buff Ranged Damage and ACC",
                    EffectsBitfield = Skill.SkillEffect.Buff,
                    Category = Skill.SkillCategory.Camping
                },
                new Skill
                {
                Name = "Gallows Humor", SkillImage = "/images/heroes/highwayman/spells/gallows_humor.webp",
                CastablePositions = [1, 2, 3, 4], Targets = [1, 2, 3, 4], // Self + Party
                IsFriendly = true, Effects = "Self: -25 Stress. Party: -20 Stress (75%) or +10 Stress (25%)",
                EffectsBitfield = Skill.SkillEffect.StressHeal | Skill.SkillEffect.StressDamage,
                Category = Skill.SkillCategory.Camping
                },
                new Skill
                {
                    Name = "Unparalleled Finesse", SkillImage = "/images/heroes/highwayman/spells/unparalleled_finesse.webp",
                    CastablePositions = [1, 2, 3, 4], Targets = [1], // Self
                    IsFriendly = true, Effects = "+10 DODGE, +2 SPD, +20% Melee DMG, +10 Melee ACC (4 Battles)",
                    EffectsBitfield = Skill.SkillEffect.Buff,
                    Category = Skill.SkillCategory.Camping
                }
            ]
        };
        roster.Add(highwayman);

        // --- 4. VESTAL ---
        var vestal = new Hero
        {
            Name = "Vestal",
            Portrait = "/images/heroes/vestal/vestal-icon.webp",
            Sprite = "/images/heroes/vestal/vestal-sprite.webp",
            Role = "Healer",
            Health = 24,
            Skills =
            [
                // COMBAT SKILLS
                new Skill
                {
                    Name = "Mace Bash", SkillImage = "/images/heroes/vestal/spells/mace_bash.webp",
                    CastablePositions = [1, 2], Targets = [1, 2],
                    IsFriendly = false, Effects = "Melee Damage",
                    EffectsBitfield = Skill.SkillEffect.Damage,
                    Category = Skill.SkillCategory.Combat
                },
                new Skill
                {
                    Name = "Judgement", SkillImage = "/images/heroes/vestal/spells/judgement.webp",
                    CastablePositions = [3, 4], Targets = [1, 2, 3, 4],
                    IsFriendly = false, Effects = "Damage + Self Heal",
                    EffectsBitfield = Skill.SkillEffect.Damage | Skill.SkillEffect.Heal,
                    Category = Skill.SkillCategory.Combat
                },
                new Skill
                {
                    Name = "Dazzling Light", SkillImage = "/images/heroes/vestal/spells/dazzling_light.webp",
                    CastablePositions = [2, 3, 4], Targets = [1, 2, 3],
                    IsFriendly = false, Effects = "Stun + Torch",
                    EffectsBitfield = Skill.SkillEffect.Stun | Skill.SkillEffect.Torch | Skill.SkillEffect.Damage,
                    Category = Skill.SkillCategory.Combat
                },
                new Skill
                {
                    Name = "Divine Grace", SkillImage = "/images/heroes/vestal/spells/divine_grace.webp",
                    CastablePositions = [3, 4], Targets = [1, 2, 3, 4],
                    IsFriendly = true, Effects = "Strong Single Target Heal",
                    EffectsBitfield = Skill.SkillEffect.Heal,
                    Category = Skill.SkillCategory.Combat
                },
                new Skill
                {
                    Name = "Divine Comfort", SkillImage = "/images/heroes/vestal/spells/divine_comfort.webp",
                    CastablePositions = [2, 3, 4], Targets = [1, 2, 3, 4], // AoE Heal
                    IsFriendly = true, Effects = "Party AoE Heal",
                    EffectsBitfield = Skill.SkillEffect.Heal,
                    Category = Skill.SkillCategory.Combat
                },
                new Skill
                {
                    Name = "Illumination", SkillImage = "/images/heroes/vestal/spells/illumination.webp",
                    CastablePositions = [2, 3, 4], Targets = [1, 2, 3, 4],
                    IsFriendly = false, Effects = "De-Stealth + Debuff Dodge + Torch",
                    EffectsBitfield = Skill.SkillEffect.DeStealth | Skill.SkillEffect.Debuff | Skill.SkillEffect.Torch,
                    Category = Skill.SkillCategory.Combat
                },
                new Skill
                {
                    Name = "Hand of Light", SkillImage = "/images/heroes/vestal/spells/hand_of_light.webp",
                    CastablePositions = [1, 2], Targets = [1, 2, 3],
                    IsFriendly = false, Effects = "Damage + Buff Self Damage/ACC",
                    EffectsBitfield = Skill.SkillEffect.Damage | Skill.SkillEffect.Buff,
                    BonusAgainst = Skill.BonusTarget.Unholy,
                    Category = Skill.SkillCategory.Combat
                },

                // CAMPING SKILLS
                new Skill
                {
                    Name = "Sanctuary", SkillImage = "/images/heroes/vestal/spells/sanctuary.webp",
                    CastablePositions = [1, 2, 3, 4], Targets = [1, 2, 3, 4],
                    IsFriendly = true, Effects = "Prevents Nighttime Ambush + Heal",
                    EffectsBitfield = Skill.SkillEffect.Buff | Skill.SkillEffect.Heal,
                    Category = Skill.SkillCategory.Camping
                },
                new Skill
                {
                    Name = "Bless", SkillImage = "/images/heroes/vestal/spells/bless.webp",
                    CastablePositions = [1, 2, 3, 4], Targets = [1, 2, 3, 4],
                    IsFriendly = true, Effects = "Buff ACC and Dodge",
                    EffectsBitfield = Skill.SkillEffect.Buff,
                    Category = Skill.SkillCategory.Camping
                },
                new Skill
                {
                    Name = "Chant", SkillImage = "/images/heroes/vestal/spells/chant.webp",
                    CastablePositions = [1, 2, 3, 4], Targets = [1, 2, 3, 4], // One Companion
                    IsFriendly = true, 
                    Effects = "Target: -15 Stress (-5 if not religious), -20% Stress Resist (-10% if not religious)",
                    EffectsBitfield = Skill.SkillEffect.StressHeal | Skill.SkillEffect.Buff,
                    Category = Skill.SkillCategory.Camping
                },
                new Skill
                {
                    Name = "Pray", SkillImage = "/images/heroes/vestal/spells/pray.webp",
                    CastablePositions = [1, 2, 3, 4], Targets = [1, 2, 3, 4], // All Companions
                    IsFriendly = true, 
                    Effects = "Party: -15 Stress (-5 if not religious), +15% PROT (+5% if not religious)",
                    EffectsBitfield = Skill.SkillEffect.StressHeal | Skill.SkillEffect.Buff,
                    Category = Skill.SkillCategory.Camping
                }
            ]
        };
        roster.Add(vestal);

        return roster;
    }
    
}