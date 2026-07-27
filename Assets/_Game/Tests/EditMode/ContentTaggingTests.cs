using Asteria.Data;
using Asteria.Expedition;
using Asteria.Interaction;
using NUnit.Framework;
using UnityEngine;

namespace Asteria.Tests.EditMode
{
    /// <summary>
    /// Tests for content definitions and tagging.
    /// Required by TECHNICAL_ARCHITECTURE.md §13 EditMode matrix.
    /// </summary>
    [TestFixture]
    public class ContentTaggingTests
    {
        [Test]
        public void BiomeDefinition_CanBeCreated()
        {
            var biome = ScriptableObject.CreateInstance<BiomeDefinition>();
            biome.biomeId = "wind_grassland";
            biome.displayName = "风之草原";
            biome.biomeType = BiomeType.Wind;
            biome.moodTags = new[] { "Cozy", "Curious" };
            biome.pressureTypes = new[] { "Wind" };

            Assert.AreEqual("wind_grassland", biome.biomeId);
            Assert.AreEqual(BiomeType.Wind, biome.biomeType);
            Assert.AreEqual(2, biome.moodTags.Length);
            Object.DestroyImmediate(biome);
        }

        [Test]
        public void PoiDefinition_Types_AreComplete()
        {
            // Verify all POI types from WORLD_CONTENT_MATRIX.md are represented
            var types = System.Enum.GetValues(typeof(PoiType));
            Assert.IsTrue(types.Length >= 7, $"Should have at least 7 POI types, got {types.Length}");
        }

        [Test]
        public void SocialEventDefinition_CanBeCreated()
        {
            var evt = ScriptableObject.CreateInstance<SocialEventDefinition>();
            evt.eventId = "daily_cooking_fail";
            evt.title = "做饭失败";
            evt.category = EventCategory.Daily;
            evt.minParticipants = 2;
            evt.cooldownDays = 0.5f;

            Assert.AreEqual("daily_cooking_fail", evt.eventId);
            Assert.AreEqual(EventCategory.Daily, evt.category);
            Object.DestroyImmediate(evt);
        }

        [Test]
        public void WorldEventDefinition_CanBeCreated()
        {
            var evt = ScriptableObject.CreateInstance<WorldEventDefinition>();
            evt.eventId = "global_wind";
            evt.title = "全球强风";
            evt.phase = ExpeditionPhase.Pressure;
            evt.biomeTags = new[] { "Wind" };
            evt.durationMinSeconds = 120f;
            evt.durationMaxSeconds = 240f;

            Assert.AreEqual(ExpeditionPhase.Pressure, evt.phase);
            Object.DestroyImmediate(evt);
        }

        [Test]
        public void ToolDefinition_CanBeCreated()
        {
            var tool = ScriptableObject.CreateInstance<ToolDefinition>();
            tool.toolId = "warm_light";
            tool.displayName = "暖光灯";
            tool.maxEnergy = 100f;
            tool.interactionTags = new[] { "cold", "dark" };

            Assert.AreEqual("warm_light", tool.toolId);
            Assert.AreEqual(2, tool.interactionTags.Length);
            Object.DestroyImmediate(tool);
        }

        [Test]
        public void PlanetArchetype_CanBeCreated()
        {
            var archetype = ScriptableObject.CreateInstance<PlanetArchetypeDefinition>();
            archetype.archetypeId = "wind_grassland";
            archetype.displayName = "风之草原";
            archetype.planetRadius = 300f;

            Assert.AreEqual(300f, archetype.planetRadius);
            Object.DestroyImmediate(archetype);
        }
    }
}
