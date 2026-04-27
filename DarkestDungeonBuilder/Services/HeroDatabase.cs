using DarkestDungeonBuilder.Models;
namespace DarkestDungeonBuilder.Services;

public class HeroDatabase
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
                    Name = "Noxious Blast", SkillImage = "noxious_blast.png",
                    CastablePositions = [2, 3, 4], Targets = [1, 2],
                    IsFriendly = false, Effects = "Damage + Blight",
                    EffectsBitfield = Skill.SkillEffect.Damage | Skill.SkillEffect.Blight,
                    Category = Skill.SkillCategory.Combat
                },
                new Skill
                {
                    Name = "Plague Grenade", SkillImage = "plague_grenade.png",
                    CastablePositions = [3, 4], Targets = [3, 4],
                    IsFriendly = false, Effects = "AoE Blight",
                    EffectsBitfield = Skill.SkillEffect.Blight,
                    Category = Skill.SkillCategory.Combat
                },
                new Skill
                {
                    Name = "Blinding Gas", SkillImage = "blinding_gas.png",
                    CastablePositions = [3, 4], Targets = [3, 4],
                    IsFriendly = false, Effects = "AoE Stun",
                    EffectsBitfield = Skill.SkillEffect.Stun,
                    Category = Skill.SkillCategory.Combat
                },
                new Skill
                {
                    Name = "Incision", SkillImage = "incision.png",
                    CastablePositions = [1, 2, 3], Targets = [1, 2],
                    IsFriendly = false, Effects = "Damage + Bleed",
                    EffectsBitfield = Skill.SkillEffect.Damage | Skill.SkillEffect.Bleed,
                    Category = Skill.SkillCategory.Combat
                },
                new Skill
                {
                    Name = "Battlefield Medicine", SkillImage = "battlefield_medicine.png",
                    CastablePositions = [3, 4], Targets = [1, 2, 3, 4],
                    IsFriendly = true, Effects = "Heal + Cure Bleed/Blight",
                    EffectsBitfield = Skill.SkillEffect.Heal | Skill.SkillEffect.Cure,
                    Category = Skill.SkillCategory.Combat
                },
                new Skill
                {
                    Name = "Emboldening Vapours", SkillImage = "emboldening_vapours.png",
                    CastablePositions = [1, 2, 3, 4], Targets = [1, 2, 3, 4],
                    IsFriendly = true, Effects = "Damage Buff + SPD Buff",
                    EffectsBitfield = Skill.SkillEffect.Buff,
                    Category = Skill.SkillCategory.Combat
                },
                new Skill
                {
                    Name = "Disorienting Blast", SkillImage = "disorienting_blast.png",
                    CastablePositions = [2, 3, 4], Targets = [2, 3, 4],
                    IsFriendly = false, Effects = "Stun + Shuffle + Clear Corpses",
                    EffectsBitfield = Skill.SkillEffect.Stun | Skill.SkillEffect.Move | Skill.SkillEffect.ClearCorpse,
                    Category = Skill.SkillCategory.Combat
                },

                // CAMPING SKILLS
                new Skill
                {
                    Name = "Experimental Vapours", SkillImage = "experimental_vapours.png",
                    CastablePositions = [1, 2, 3, 4], Targets = [1, 2, 3, 4],
                    IsFriendly = true, Effects = "Huge Heal on One Ally",
                    EffectsBitfield = Skill.SkillEffect.Heal | Skill.SkillEffect.Buff,
                    Category = Skill.SkillCategory.Camping
                },
                new Skill
                {
                    Name = "Leeches", SkillImage = "leeches.png",
                    CastablePositions = [1, 2, 3, 4], Targets = [1, 2, 3, 4],
                    IsFriendly = true, Effects = "Heal + Cure Disease/Blight",
                    EffectsBitfield = Skill.SkillEffect.Heal | Skill.SkillEffect.Disease | Skill.SkillEffect.Cure,
                    Category = Skill.SkillCategory.Camping
                },
                new Skill
                {
                    Name = "Preventative Medicine", SkillImage = "preventative_medicine.png",
                    CastablePositions = [1, 2, 3, 4], Targets = [1], // Self
                    IsFriendly = true, Effects = "Heal + Disease Resist",
                    EffectsBitfield = Skill.SkillEffect.Heal | Skill.SkillEffect.Buff,
                    Category = Skill.SkillCategory.Camping
                },
                new Skill
                {
                    Name = "The Cure", SkillImage = "the_cure.png",
                    CastablePositions = [1, 2, 3, 4], Targets = [1], // Self
                    IsFriendly = true, Effects = "Cure Disease on Self",
                    EffectsBitfield = Skill.SkillEffect.Disease | Skill.SkillEffect.Cure,
                    Category = Skill.SkillCategory.Camping
                }
            ]
        };
        roster.Add(plagueDoctor);

        return roster;
    }
}