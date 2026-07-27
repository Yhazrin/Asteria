using Asteria.Data;
using NUnit.Framework;
using UnityEngine;

namespace Asteria.Tests.EditMode
{
    /// <summary>
    /// Tests for resource limits and trace limits.
    /// Required by TEST_SPEC.md EditMode matrix.
    /// </summary>
    [TestFixture]
    public class ResourceLimitTests
    {
        [Test]
        public void TraceLimitsConfig_DefaultValues_AreReasonable()
        {
            var config = ScriptableObject.CreateInstance<TraceLimitsConfig>();

            Assert.IsTrue(config.maxCampLights > 0, "maxCampLights should be > 0");
            Assert.IsTrue(config.maxWaymarks > 0, "maxWaymarks should be > 0");
            Assert.IsTrue(config.maxPhotos > 0, "maxPhotos should be > 0");
            Assert.IsTrue(config.defaultDecaySeconds > 0, "defaultDecaySeconds should be > 0");

            Object.DestroyImmediate(config);
        }

        [Test]
        public void TraceLimitsConfig_CampLights_LessThanWaymarks()
        {
            var config = ScriptableObject.CreateInstance<TraceLimitsConfig>();
            Assert.IsTrue(config.maxCampLights <= config.maxWaymarks,
                "Camp lights should be limited more than waymarks");

            Object.DestroyImmediate(config);
        }

        [Test]
        public void PlayerMotorConfig_DefaultValues_AreReasonable()
        {
            var config = ScriptableObject.CreateInstance<PlayerMotorConfig>();

            Assert.IsTrue(config.walkSpeed > 0, "walkSpeed should be > 0");
            Assert.IsTrue(config.runSpeed > config.walkSpeed, "runSpeed should be > walkSpeed");
            Assert.IsTrue(config.acceleration > 0, "acceleration should be > 0");
            Assert.IsTrue(config.jumpSpeed > 0, "jumpSpeed should be > 0");

            Object.DestroyImmediate(config);
        }

        [Test]
        public void AlphaContentRegistry_TargetCounts_AreReasonable()
        {
            var registry = ScriptableObject.CreateInstance<AlphaContentRegistry>();

            Assert.IsTrue(registry.targetResidentCount >= 6, "Should target at least 6 residents");
            Assert.IsTrue(registry.targetFacilityCount >= 3, "Should target at least 3 facilities");
            Assert.IsTrue(registry.targetSocialEventCount >= 12, "Should target at least 12 events");
            Assert.IsTrue(registry.targetPoiCount >= 8, "Should target at least 8 POIs");

            Object.DestroyImmediate(registry);
        }
    }
}
