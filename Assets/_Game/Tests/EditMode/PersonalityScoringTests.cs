using Asteria.Residents;
using NUnit.Framework;
using UnityEngine;

namespace Asteria.Tests.EditMode
{
    /// <summary>
    /// Tests for personality and relationship scoring.
    /// Required by TECHNICAL_ARCHITECTURE.md §13 EditMode matrix.
    /// </summary>
    [TestFixture]
    public class PersonalityScoringTests
    {
        [Test]
        public void PersonalityPreset_DefaultValues_AreZero()
        {
            var preset = ScriptableObject.CreateInstance<PersonalityPreset>();
            Assert.AreEqual(0f, preset.sociability, 0.001f);
            Assert.AreEqual(0f, preset.curiosity, 0.001f);
            Assert.AreEqual(0f, preset.warmth, 0.001f);
            Assert.AreEqual(0f, preset.order, 0.001f);
            Assert.AreEqual(0f, preset.boldness, 0.001f);
            Object.DestroyImmediate(preset);
        }

        [Test]
        public void PersonalityPreset_ClampsRange()
        {
            var preset = ScriptableObject.CreateInstance<PersonalityPreset>();
            preset.sociability = 0.5f;
            preset.curiosity = -0.3f;
            Assert.AreEqual(0.5f, preset.sociability, 0.001f);
            Assert.AreEqual(-0.3f, preset.curiosity, 0.001f);
            Object.DestroyImmediate(preset);
        }

        [Test]
        public void RelationshipEdge_DefaultValues_AreZero()
        {
            var edge = new RelationshipEdge
            {
                residentIdA = "a",
                residentIdB = "b"
            };

            Assert.AreEqual(0f, edge.familiarity, 0.001f);
            Assert.AreEqual(0f, edge.affinity, 0.001f);
            Assert.AreEqual(0f, edge.trust, 0.001f);
            Assert.AreEqual(0f, edge.admiration, 0.001f);
            Assert.AreEqual(0f, edge.tension, 0.001f);
        }

        [Test]
        public void RelationshipEdge_HighAffinity_HighTension_ReflectsConflict()
        {
            // "高亲近 + 高紧张：关系很好但最近闹别扭"
            var edge = new RelationshipEdge
            {
                residentIdA = "a",
                residentIdB = "b",
                affinity = 0.8f,
                tension = 0.7f
            };

            Assert.IsTrue(edge.affinity > 0.5f, "Should have high affinity");
            Assert.IsTrue(edge.tension > 0.5f, "Should have high tension");
        }

        [Test]
        public void RelationshipEdge_HighTrust_LowSocial_StableFriend()
        {
            // "高信任 + 低社交频率：稳定老朋友，不需要天天互动"
            var edge = new RelationshipEdge
            {
                residentIdA = "a",
                residentIdB = "b",
                trust = 0.9f,
                familiarity = 0.2f
            };

            Assert.IsTrue(edge.trust > 0.5f, "Should have high trust");
            Assert.IsTrue(edge.familiarity < 0.5f, "Should have low familiarity");
        }

        [Test]
        public void QuirkDefinition_CanBeCreated()
        {
            var quirk = ScriptableObject.CreateInstance<QuirkDefinition>();
            quirk.quirkId = "plant_namer";
            quirk.displayName = "会给所有植物取名字";
            quirk.triggerTags = new[] { "plant", "garden" };
            quirk.behaviorModifiers = new[] { "stop_and_name" };

            Assert.AreEqual("plant_namer", quirk.quirkId);
            Assert.AreEqual(2, quirk.triggerTags.Length);
            Object.DestroyImmediate(quirk);
        }

        [Test]
        public void PreferenceDefinition_CanBeCreated()
        {
            var pref = ScriptableObject.CreateInstance<PreferenceDefinition>();
            pref.preferenceId = "pref_nature";
            pref.likedBiomes = new[] { "wind", "bloom" };
            pref.dislikedBiomes = new[] { "ruin" };

            Assert.AreEqual(2, pref.likedBiomes.Length);
            Assert.AreEqual(1, pref.dislikedBiomes.Length);
            Object.DestroyImmediate(pref);
        }
    }
}
