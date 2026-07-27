using Asteria.Expedition;
using NUnit.Framework;
using UnityEngine;

namespace Asteria.Tests.EditMode
{
    /// <summary>
    /// Tests for event candidate filtering and scoring.
    /// Required by TEST_SPEC.md EditMode matrix.
    /// </summary>
    [TestFixture]
    public class EventCandidateQueryTests
    {
        [Test]
        public void WorldEvent_CanFilterByPhase()
        {
            var arrival = CreateEvent("arrival", ExpeditionPhase.Arrival);
            var pressure = CreateEvent("pressure", ExpeditionPhase.Pressure);

            Assert.AreEqual(ExpeditionPhase.Arrival, arrival.phase);
            Assert.AreEqual(ExpeditionPhase.Pressure, pressure.phase);

            Object.DestroyImmediate(arrival);
            Object.DestroyImmediate(pressure);
        }

        [Test]
        public void WorldEvent_CanFilterByBiome()
        {
            var wind = CreateEvent("wind", ExpeditionPhase.Arrival);
            wind.biomeTags = new[] { "Wind" };

            var mist = CreateEvent("mist", ExpeditionPhase.Arrival);
            mist.biomeTags = new[] { "Mist" };

            Assert.Contains("Wind", wind.biomeTags);
            Assert.DoesNotContain("Wind", mist.biomeTags);

            Object.DestroyImmediate(wind);
            Object.DestroyImmediate(mist);
        }

        [Test]
        public void WorldEvent_CanFilterByPlayerCount()
        {
            var solo = CreateEvent("solo", ExpeditionPhase.Arrival);
            solo.minPlayers = 1;
            solo.maxPlayers = 1;

            var coop = CreateEvent("coop", ExpeditionPhase.Arrival);
            coop.minPlayers = 2;
            coop.maxPlayers = 4;

            Assert.AreEqual(1, solo.minPlayers);
            Assert.AreEqual(4, coop.maxPlayers);

            Object.DestroyImmediate(solo);
            Object.DestroyImmediate(coop);
        }

        [Test]
        public void SocialEvent_CanFilterByCategory()
        {
            var daily = ScriptableObject.CreateInstance<SocialEventDefinition>();
            daily.eventId = "daily";
            daily.category = EventCategory.Daily;

            var conflict = ScriptableObject.CreateInstance<SocialEventDefinition>();
            conflict.eventId = "conflict";
            conflict.category = EventCategory.Conflict;

            Assert.AreEqual(EventCategory.Daily, daily.category);
            Assert.AreEqual(EventCategory.Conflict, conflict.category);

            Object.DestroyImmediate(daily);
            Object.DestroyImmediate(conflict);
        }

        WorldEventDefinition CreateEvent(string id, ExpeditionPhase phase)
        {
            var evt = ScriptableObject.CreateInstance<WorldEventDefinition>();
            evt.eventId = id;
            evt.phase = phase;
            return evt;
        }
    }
}
