using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Content budget tracking for the game.
    /// Ensures content stays within planned limits.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Content Budget Database")]
    public sealed class ContentBudgetDatabase : ScriptableObject
    {
        [Header("Content Budgets")]
        public ContentBudget[] budgets = new ContentBudget[]
        {
            new ContentBudget
            {
                category = "Planet Archetypes",
                currentCount = 3,
                targetCount = 6,
                notes = "Wind Grassland complete, Mist Forest and Night Valley planned"
            },
            new ContentBudget
            {
                category = "Biomes",
                currentCount = 6,
                targetCount = 6,
                notes = "All 6 biomes defined"
            },
            new ContentBudget
            {
                category = "POIs per Planet",
                currentCount = 8,
                targetCount = 10,
                notes = "Wind Grassland has 8 POIs"
            },
            new ContentBudget
            {
                category = "World Events per Expedition",
                currentCount = 8,
                targetCount = 8,
                notes = "Wind Grassland has 8 events"
            },
            new ContentBudget
            {
                category = "Social Events",
                currentCount = 12,
                targetCount = 12,
                notes = "12 social events defined"
            },
            new ContentBudget
            {
                category = "Residents",
                currentCount = 6,
                targetCount = 12,
                notes = "6 residents defined, 6 more planned"
            },
            new ContentBudget
            {
                category = "Facilities",
                currentCount = 8,
                targetCount = 8,
                notes = "8 facilities defined"
            },
            new ContentBudget
            {
                category = "Tools",
                currentCount = 6,
                targetCount = 6,
                notes = "6 tools defined"
            },
            new ContentBudget
            {
                category = "Wishes",
                currentCount = 6,
                targetCount = 6,
                notes = "6 wishes defined"
            },
            new ContentBudget
            {
                category = "Memory Cards",
                currentCount = 5,
                targetCount = 10,
                notes = "5 memory cards, 5 more planned"
            },
            new ContentBudget
            {
                category = "Achievements",
                currentCount = 15,
                targetCount = 15,
                notes = "15 achievements defined"
            },
            new ContentBudget
            {
                category = "Creature Types",
                currentCount = 6,
                targetCount = 6,
                notes = "6 creature behavior types defined"
            },
        };
    }

    [System.Serializable]
    public class ContentBudget
    {
        public string category;
        public int currentCount;
        public int targetCount;
        public string notes;

        public float Progress => targetCount > 0 ? (float)currentCount / targetCount : 0f;
        public bool IsComplete => currentCount >= targetCount;
    }
}
