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
            Portrait = "/images/mock/rider.png",
            Role = "Frontline",
            Health = 33,
            Skills =
            [
                // COMBAT SKILLS
                new Skill
                {
                    Name = "Smite", SkillImage = "smite.png",
                    CastablePositions = [1, 2], Targets = [1, 2],
                    IsFriendly = false, Effects = "Damage to front lines",
                    Category = Skill.SkillCategory.Combat
                },
                new Skill
                {
                    Name = "Zealous Accusation", SkillImage = "zealous_accusation.png",
                    CastablePositions = [1, 2], Targets = [1, 2],
                    IsFriendly = false, Effects = "AoE Damage",
                    Category = Skill.SkillCategory.Combat
                },
                new Skill
                {
                    Name = "Stunning Blow", SkillImage = "stunning_blow.png",
                    CastablePositions = [1, 2], Targets = [1, 2],
                    IsFriendly = false, Effects = "Stun",
                    Category = Skill.SkillCategory.Combat
                },
                new Skill
                {
                    Name = "Bulwark of Faith", SkillImage = "bulwark_of_faith.png",
                    CastablePositions = [1, 2], Targets = [1], // Self
                    IsFriendly = true, Effects = "Prot Buff + Mark",
                    Category = Skill.SkillCategory.Combat
                },
                new Skill
                {
                    Name = "Battle Heal", SkillImage = "battle_heal.png",
                    CastablePositions = [1, 2, 3, 4], Targets = [1, 2, 3, 4],
                    IsFriendly = true, Effects = "Small Heal",
                    Category = Skill.SkillCategory.Combat
                },
                new Skill
                {
                    Name = "Holy Lance", SkillImage = "holy_lance.png",
                    CastablePositions = [3, 4], Targets = [2, 3, 4],
                    IsFriendly = false, Effects = "Damage + Move Forward 1",
                    Category = Skill.SkillCategory.Combat
                },
                new Skill
                {
                    Name = "Inspiring Cry", SkillImage = "inspiring_cry.png",
                    CastablePositions = [1, 2, 3, 4], Targets = [1, 2, 3, 4],
                    IsFriendly = true, Effects = "Stress Heal + Torch",
                    Category = Skill.SkillCategory.Combat
                },

                // CAMPING SKILLS
                new Skill
                {
                    Name = "Zealous Vigil", SkillImage = "zealous_vigil.png",
                    CastablePositions = [1, 2, 3, 4], Targets = [1], // Self
                    IsFriendly = true, Effects = "Stress Heal, Prevents Nighttime Ambush",
                    Category = Skill.SkillCategory.Camping
                },
                new Skill
                {
                    Name = "Zealous Speech", SkillImage = "zealous_speech.png",
                    CastablePositions = [1, 2, 3, 4], Targets = [1, 2, 3, 4], // Party
                    IsFriendly = true, Effects = "Party Stress Heal + Stress Resist",
                    Category = Skill.SkillCategory.Camping
                },
                new Skill
                {
                    Name = "Stand Tall", SkillImage = "stand_tall.png",
                    CastablePositions = [1, 2, 3, 4], Targets = [1, 2, 3, 4], // One Ally
                    IsFriendly = true, Effects = "Stress Heal + Remove Mortality Debuffs",
                    Category = Skill.SkillCategory.Camping
                },
                new Skill
                {
                    Name = "Unshakeable Leader", SkillImage = "unshakeable_leader.png",
                    CastablePositions = [1, 2, 3, 4], Targets = [1], // Self
                    IsFriendly = true, Effects = "Stress Resist",
                    Category = Skill.SkillCategory.Camping
                }
            ]
        };
        roster.Add(crusader);

        // --- 2. PLAGUE DOCTOR ---
        var plagueDoctor = new Hero
        {
            Name = "Plague Doctor",
            Portrait = "/images/mock/pd_hero.png",
            Role = "Support",
            Health = 22,
            Skills =
            [
                // COMBAT SKILLS
                new Skill
                {
                    Name = "Noxious Blast", SkillImage = "noxious_blast.png",
                    CastablePositions = [2, 3, 4], Targets = [1, 2],
                    IsFriendly = false, Effects = "Blight",
                    Category = Skill.SkillCategory.Combat
                },
                new Skill
                {
                    Name = "Plague Grenade", SkillImage = "plague_grenade.png",
                    CastablePositions = [3, 4], Targets = [3, 4],
                    IsFriendly = false, Effects = "AoE Blight",
                    Category = Skill.SkillCategory.Combat
                },
                new Skill
                {
                    Name = "Blinding Gas", SkillImage = "blinding_gas.png",
                    CastablePositions = [3, 4], Targets = [3, 4],
                    IsFriendly = false, Effects = "AoE Stun",
                    Category = Skill.SkillCategory.Combat
                },
                new Skill
                {
                    Name = "Incision", SkillImage = "incision.png",
                    CastablePositions = [1, 2, 3], Targets = [1, 2],
                    IsFriendly = false, Effects = "Bleed",
                    Category = Skill.SkillCategory.Combat
                },
                new Skill
                {
                    Name = "Battlefield Medicine", SkillImage = "battlefield_medicine.png",
                    CastablePositions = [3, 4], Targets = [1, 2, 3, 4],
                    IsFriendly = true, Effects = "Heal + Cure Bleed/Blight",
                    Category = Skill.SkillCategory.Combat
                },
                new Skill
                {
                    Name = "Emboldening Vapours", SkillImage = "emboldening_vapours.png",
                    CastablePositions = [1, 2, 3, 4], Targets = [1, 2, 3, 4],
                    IsFriendly = true, Effects = "Damage Buff + SPD Buff",
                    Category = Skill.SkillCategory.Combat
                },
                new Skill
                {
                    Name = "Disorienting Blast", SkillImage = "disorienting_blast.png",
                    CastablePositions = [2, 3, 4], Targets = [2, 3, 4],
                    IsFriendly = false, Effects = "Stun + Shuffle + Clear Corpses",
                    Category = Skill.SkillCategory.Combat
                },

                // CAMPING SKILLS
                new Skill
                {
                    Name = "Experimental Vapours", SkillImage = "experimental_vapours.png",
                    CastablePositions = [1, 2, 3, 4], Targets = [1, 2, 3, 4],
                    IsFriendly = true, Effects = "Huge Heal on One Ally",
                    Category = Skill.SkillCategory.Camping
                },
                new Skill
                {
                    Name = "Leeches", SkillImage = "leeches.png",
                    CastablePositions = [1, 2, 3, 4], Targets = [1, 2, 3, 4],
                    IsFriendly = true, Effects = "Heal + Cure Disease",
                    Category = Skill.SkillCategory.Camping
                },
                new Skill
                {
                    Name = "Preventative Medicine", SkillImage = "preventative_medicine.png",
                    CastablePositions = [1, 2, 3, 4], Targets = [1], // Self
                    IsFriendly = true, Effects = "Heal + Resistances",
                    Category = Skill.SkillCategory.Camping
                },
                new Skill
                {
                    Name = "The Cure", SkillImage = "the_cure.png",
                    CastablePositions = [1, 2, 3, 4], Targets = [1], // Self
                    IsFriendly = true, Effects = "Cure Disease on Self",
                    Category = Skill.SkillCategory.Camping
                }
            ]
        };
        roster.Add(plagueDoctor);

        return roster;
    }
}

