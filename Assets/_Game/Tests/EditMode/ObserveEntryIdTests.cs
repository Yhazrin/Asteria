using Asteria.Data;
using NUnit.Framework;
using UnityEngine;

namespace Asteria.Tests.EditMode
{
    /// <summary>
    /// EditMode tests for ObserveEntry stable ID validation.
    /// </summary>
    [TestFixture]
    public class ObserveEntryIdTests
    {
        [Test]
        public void IsIdValid_SimpleSnakeCase_ReturnsTrue()
        {
            var entry = ScriptableObject.CreateInstance<ObserveEntry>();
            entry.id = "wind_bell_stone";
            Assert.IsTrue(entry.IsIdValid());
            Object.DestroyImmediate(entry);
        }

        [Test]
        public void IsIdValid_DotSeparated_ReturnsTrue()
        {
            var entry = ScriptableObject.CreateInstance<ObserveEntry>();
            entry.id = "observe.wind_bell_stone";
            Assert.IsTrue(entry.IsIdValid());
            Object.DestroyImmediate(entry);
        }

        [Test]
        public void IsIdValid_MultiDot_ReturnsTrue()
        {
            var entry = ScriptableObject.CreateInstance<ObserveEntry>();
            entry.id = "biome.wind_grassland.poi_01";
            Assert.IsTrue(entry.IsIdValid());
            Object.DestroyImmediate(entry);
        }

        [Test]
        public void IsIdValid_Empty_ReturnsFalse()
        {
            var entry = ScriptableObject.CreateInstance<ObserveEntry>();
            entry.id = "";
            Assert.IsFalse(entry.IsIdValid());
            Object.DestroyImmediate(entry);
        }

        [Test]
        public void IsIdValid_Null_ReturnsFalse()
        {
            var entry = ScriptableObject.CreateInstance<ObserveEntry>();
            entry.id = null;
            Assert.IsFalse(entry.IsIdValid());
            Object.DestroyImmediate(entry);
        }

        [Test]
        public void IsIdValid_Whitespace_ReturnsFalse()
        {
            var entry = ScriptableObject.CreateInstance<ObserveEntry>();
            entry.id = "   ";
            Assert.IsFalse(entry.IsIdValid());
            Object.DestroyImmediate(entry);
        }

        [Test]
        public void IsIdValid_StartsWithNumber_ReturnsFalse()
        {
            var entry = ScriptableObject.CreateInstance<ObserveEntry>();
            entry.id = "1wind_bell";
            Assert.IsFalse(entry.IsIdValid());
            Object.DestroyImmediate(entry);
        }

        [Test]
        public void IsIdValid_Uppercase_ReturnsFalse()
        {
            var entry = ScriptableObject.CreateInstance<ObserveEntry>();
            entry.id = "WindBellStone";
            Assert.IsFalse(entry.IsIdValid());
            Object.DestroyImmediate(entry);
        }

        [Test]
        public void IsIdValid_ContainsSpaces_ReturnsFalse()
        {
            var entry = ScriptableObject.CreateInstance<ObserveEntry>();
            entry.id = "wind bell stone";
            Assert.IsFalse(entry.IsIdValid());
            Object.DestroyImmediate(entry);
        }

        [Test]
        public void IsIdValid_ContainsHyphen_ReturnsFalse()
        {
            var entry = ScriptableObject.CreateInstance<ObserveEntry>();
            entry.id = "wind-bell-stone";
            Assert.IsFalse(entry.IsIdValid());
            Object.DestroyImmediate(entry);
        }

        [Test]
        public void IsIdValid_DotAtStart_ReturnsFalse()
        {
            var entry = ScriptableObject.CreateInstance<ObserveEntry>();
            entry.id = ".wind_bell";
            Assert.IsFalse(entry.IsIdValid());
            Object.DestroyImmediate(entry);
        }

        [Test]
        public void IsIdValid_DotAtEnd_ReturnsFalse()
        {
            var entry = ScriptableObject.CreateInstance<ObserveEntry>();
            entry.id = "wind_bell.";
            Assert.IsFalse(entry.IsIdValid());
            Object.DestroyImmediate(entry);
        }

        [Test]
        public void IsIdValid_DoubleDot_ReturnsFalse()
        {
            var entry = ScriptableObject.CreateInstance<ObserveEntry>();
            entry.id = "wind..bell";
            Assert.IsFalse(entry.IsIdValid());
            Object.DestroyImmediate(entry);
        }
    }
}
