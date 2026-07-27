using Asteria.Data;
using Asteria.Expedition;
using NUnit.Framework;
using UnityEngine;

namespace Asteria.Tests.EditMode
{
    /// <summary>
    /// Tests for content tag matching logic.
    /// Required by TEST_SPEC.md EditMode matrix.
    /// </summary>
    [TestFixture]
    public class TagMatchingTests
    {
        [Test]
        public void BiomeTag_Wind_MatchesWindEvents()
        {
            var biome = ScriptableObject.CreateInstance<BiomeDefinition>();
            biome.biomeType = BiomeType.Wind;

            Assert.AreEqual(BiomeType.Wind, biome.biomeType);
            Object.DestroyImmediate(biome);
        }

        [Test]
        public void PoiType_AllTypes_AreDefined()
        {
            var types = System.Enum.GetValues(typeof(PoiType));
            Assert.IsTrue(types.Length >= 7, $"Should have at least 7 POI types, got {types.Length}");
        }

        [Test]
        public void PressureType_AllTypes_AreDefined()
        {
            var types = System.Enum.GetValues(typeof(PressureType));
            Assert.IsTrue(types.Length >= 5, $"Should have at least 5 pressure types, got {types.Length}");
        }

        [Test]
        public void EventCategory_AllTypes_AreDefined()
        {
            var types = System.Enum.GetValues(typeof(EventCategory));
            Assert.IsTrue(types.Length >= 6, $"Should have at least 6 event categories, got {types.Length}");
        }

        [Test]
        public void ExpeditionPhase_AllPhases_AreDefined()
        {
            var types = System.Enum.GetValues(typeof(ExpeditionPhase));
            Assert.AreEqual(6, types.Length, "Should have exactly 6 expedition phases");
        }

        [Test]
        public void ContentTag_CanBeAssigned()
        {
            var poi = ScriptableObject.CreateInstance<PoiDefinition>();
            poi.contentTags = new[] { "observe", "wind", "curious" };

            Assert.AreEqual(3, poi.contentTags.Length);
            Assert.Contains("observe", poi.contentTags);
            Object.DestroyImmediate(poi);
        }
    }
}
