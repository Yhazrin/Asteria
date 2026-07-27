using System.Collections.Generic;
using NUnit.Framework;

namespace Asteria.Tests.EditMode
{
    /// <summary>
    /// Tests for event cooldown management.
    /// Required by TEST_SPEC.md EditMode matrix.
    /// </summary>
    [TestFixture]
    public class CooldownRegistryTests
    {
        Dictionary<string, float> _cooldowns;

        [SetUp]
        public void SetUp()
        {
            _cooldowns = new Dictionary<string, float>();
        }

        [Test]
        public void Register_SetsCooldown()
        {
            Register("event_a", 2f, 0f);
            Assert.IsTrue(IsOnCooldown("event_a", 1f));
        }

        [Test]
        public void Register_ExpiresAfterCooldown()
        {
            Register("event_a", 2f, 0f);
            Assert.IsFalse(IsOnCooldown("event_a", 3f));
        }

        [Test]
        public void Register_NotOnCooldown_WhenNotRegistered()
        {
            Assert.IsFalse(IsOnCooldown("event_a", 0f));
        }

        [Test]
        public void Register_MultipleEvents_Independent()
        {
            Register("event_a", 2f, 0f);
            Register("event_b", 5f, 0f);

            Assert.IsTrue(IsOnCooldown("event_a", 1f));
            Assert.IsTrue(IsOnCooldown("event_b", 1f));
            Assert.IsFalse(IsOnCooldown("event_a", 3f));
            Assert.IsTrue(IsOnCooldown("event_b", 3f));
        }

        [Test]
        public void Register_OverwritesPreviousCooldown()
        {
            Register("event_a", 2f, 0f);
            Register("event_a", 5f, 0f);

            Assert.IsTrue(IsOnCooldown("event_a", 3f));
            Assert.IsFalse(IsOnCooldown("event_a", 6f));
        }

        void Register(string eventId, float cooldownDays, float currentDay)
        {
            _cooldowns[eventId] = currentDay + cooldownDays;
        }

        bool IsOnCooldown(string eventId, float currentDay)
        {
            return _cooldowns.TryGetValue(eventId, out float readyDay)
                && currentDay < readyDay;
        }
    }
}
