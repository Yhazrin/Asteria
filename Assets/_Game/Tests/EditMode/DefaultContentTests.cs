using Asteria.Data;
using Asteria.Expedition;
using Asteria.Interaction;
using Asteria.Residents;
using NUnit.Framework;
using UnityEngine;

namespace Asteria.Tests.EditMode
{
    /// <summary>
    /// Tests for the default content factory.
    /// Validates that all Alpha content is properly created.
    /// </summary>
    [TestFixture]
    public class DefaultContentTests
    {
        [Test]
        public void WindGrassland_HasCorrectId()
        {
            var archetype = DefaultContentFactory.CreateWindGrassland();
            Assert.AreEqual("wind_grassland", archetype.archetypeId);
            Assert.AreEqual("风之草原", archetype.displayName);
            Object.DestroyImmediate(archetype);
        }

        [Test]
        public void WindGrassland_Has8PoiSlots()
        {
            var archetype = DefaultContentFactory.CreateWindGrassland();
            Assert.AreEqual(8, archetype.poiSlots.Length);
            Object.DestroyImmediate(archetype);
        }

        [Test]
        public void WindGrasslandBiome_IsWindType()
        {
            var biome = DefaultContentFactory.CreateWindGrasslandBiome();
            Assert.AreEqual(BiomeType.Wind, biome.biomeType);
            Object.DestroyImmediate(biome);
        }

        [Test]
        public void WindGrasslandEvents_Has8Events()
        {
            var events = DefaultContentFactory.CreateWindGrasslandEvents();
            Assert.AreEqual(8, events.Length);

            // Verify phases are covered
            var phases = new System.Collections.Generic.HashSet<ExpeditionPhase>();
            foreach (var evt in events)
            {
                phases.Add(evt.phase);
            }

            Assert.IsTrue(phases.Contains(ExpeditionPhase.Arrival), "Should have Arrival event");
            Assert.IsTrue(phases.Contains(ExpeditionPhase.Pressure), "Should have Pressure event");
            Assert.IsTrue(phases.Contains(ExpeditionPhase.Resolution), "Should have Resolution event");

            foreach (var evt in events) Object.DestroyImmediate(evt);
        }

        [Test]
        public void DefaultTools_Has3Tools()
        {
            var tools = DefaultContentFactory.CreateDefaultTools();
            Assert.AreEqual(3, tools.Length);

            var ids = new System.Collections.Generic.HashSet<string>();
            foreach (var tool in tools)
            {
                ids.Add(tool.toolId);
            }

            Assert.IsTrue(ids.Contains("resonance_mirror"));
            Assert.IsTrue(ids.Contains("warm_light"));
            Assert.IsTrue(ids.Contains("beacon"));

            foreach (var tool in tools) Object.DestroyImmediate(tool);
        }

        [Test]
        public void DefaultSocialEvents_Has6Events()
        {
            var events = DefaultContentFactory.CreateDefaultSocialEvents();
            Assert.AreEqual(6, events.Length);
            foreach (var evt in events) Object.DestroyImmediate(evt);
        }

        [Test]
        public void DefaultResidents_Has6Residents()
        {
            var residents = DefaultResidents.CreateDefaultResidentDefinitions();
            Assert.AreEqual(6, residents.Length);

            var ids = new System.Collections.Generic.HashSet<string>();
            foreach (var res in residents)
            {
                ids.Add(res.ResidentId);
            }

            Assert.IsTrue(ids.Contains("lian"));
            Assert.IsTrue(ids.Contains("kai"));
            Assert.IsTrue(ids.Contains("qing"));
            Assert.IsTrue(ids.Contains("shuang"));
            Assert.IsTrue(ids.Contains("yan"));
            Assert.IsTrue(ids.Contains("yun"));

            foreach (var res in residents) Object.DestroyImmediate(res);
        }

        [Test]
        public void DefaultWishes_Has6Wishes()
        {
            var wishes = DefaultWishes.CreateDefaultWishes();
            Assert.AreEqual(6, wishes.Length);
            foreach (var wish in wishes) Object.DestroyImmediate(wish);
        }

        [Test]
        public void DefaultMemoryCards_Has4Cards()
        {
            var cards = DefaultMemoryCards.CreateDefaultCards();
            Assert.AreEqual(4, cards.Length);
            foreach (var card in cards) Object.DestroyImmediate(card);
        }
    }
}
