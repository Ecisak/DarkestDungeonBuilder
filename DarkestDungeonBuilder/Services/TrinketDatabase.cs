using DarkestDungeonBuilder.Models;

namespace DarkestDungeonBuilder.Services;

public class TrinketDatabase
{
    public List<Trinket> GetMockTrinkets()
    {
        return
        [
            new Trinket
            {
                Name = "Blasphemous Vial",
                Rarity = "Very Rare",
                Effects = "+15% Blight Chance, +10% Stun Chance, +15% Acc",
                TrinketImage = "/images/trinkets/blasphemous_vial.webp",
                ClassTrinket = "Plague Doctor"
            },

            new Trinket
            {
                Name = "Diseased Heb",
                Rarity = "Common",
                Effects = "+40% Disease Resist",
                TrinketImage = "/images/trinkets/diseased_herb.webp",
                ClassTrinket = "Plague Doctor"
            },

            // --- Crusader ---

            new Trinket
            {
                Name = "Holy Orders",
                Rarity = "Very Rare",
                Effects = "+15% Virtue Chance, -20% Stress, +10% Death Blow Resist",
                TrinketImage = "/images/trinkets/holy_orders.webp",
                ClassTrinket = "Crusader"
            },

            new Trinket
            {
                Name = "Paralyzer's Crest",
                Rarity = "Uncommon",
                Effects = "+20% Stun Skill Chance, -1 Dodge",
                TrinketImage = "/images/trinkets/paralyzers_crest.webp",
                ClassTrinket = "Crusader"
            },

            // --- VZÁCNÉ (Ancestral / Rare) ---

            new Trinket
            {
                Name = "Ancestor's Map",
                Rarity = "Ancestral",
                Effects = "+25% Trap Disarm, +25% Scouting Chance",
                TrinketImage = "/images/trinkets/ancestors_map.webp"
            },

            new Trinket
            {
                Name = "Sun Ring",
                Rarity = "Rare",
                Effects = "+10 Acc, +5% Crit, -8 Dodge",
                TrinketImage = "/images/trinkets/focus_ring.webp"
            },

            // --- BEZ CLASSY (Common / Universal) ---

            new Trinket
            {
                Name = "Caution Cloak",
                Rarity = "Common",
                Effects = "+2 Speed",
                TrinketImage = "/images/trinkets/caution_cloak.webp"
            },

            new Trinket
            {
                Name = "Reckless Charm",
                Rarity = "Common",
                Effects = "+15% Healing Skills",
                TrinketImage = "/images/trinkets/reckless_charm.webp"
            },

            new Trinket
            {
                Name = "Warrior's Cap",
                Rarity = "Common",
                Effects = "+5 Acc (Melee skills only)",
                TrinketImage = "/images/trinkets/warriors_cap.webp"
            }
        ];
    }
}