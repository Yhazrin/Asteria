using Asteria.Residents;
using NUnit.Framework;
using UnityEngine;

namespace Asteria.Tests.EditMode
{
    [TestFixture]
    public class ResidentTests
    {
        [Test]
        public void ResidentDefinition_DefaultValues_AreValid()
        {
            var def = ScriptableObject.CreateInstance<ResidentDefinition>();

            Assert.IsNotNull(def.ResidentId);
            Assert.IsNotNull(def.DisplayName);
            Assert.AreEqual(0f, def.Sociability, 0.01f);
            Object.DestroyImmediate(def);
        }

        [Test]
        public void ResidentState_DefaultMemory_IsEmpty()
        {
            var state = new ResidentState
            {
                residentId = "test"
            };

            Assert.IsNotNull(state.memories);
            Assert.AreEqual(0, state.memories.Count);
        }

        [Test]
        public void MemoryRecord_CanBeCreated()
        {
            var memory = new MemoryRecord
            {
                eventId = "test_event",
                timestamp = System.DateTime.UtcNow.ToString("o"),
                participants = new[] { "a", "b" },
                location = "plaza",
                emotionalTone = "happy",
                tags = new[] { "friendly" },
                importance = 0.5f,
                isPermanent = false
            };

            Assert.AreEqual("test_event", memory.eventId);
            Assert.AreEqual(2, memory.participants.Length);
            Assert.IsFalse(memory.isPermanent);
        }
    }
}
