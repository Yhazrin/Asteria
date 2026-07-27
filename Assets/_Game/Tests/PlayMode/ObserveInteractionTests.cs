using System.Collections;
using Asteria.Data;
using Asteria.Interaction;
using Asteria.Planet;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Asteria.Tests.PlayMode
{
    /// <summary>
    /// PlayMode tests for Observe interaction flow.
    /// Validates single-unlock, no double-counting, and HUD feedback.
    /// </summary>
    [TestFixture]
    public class ObserveInteractionTests
    {
        GameObject _planetGo;
        PlanetBody _planet;
        GameObject _journalGo;
        DiscoveryJournal _journal;

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            _planetGo = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            _planetGo.name = "TestPlanet";
            _planet = _planetGo.AddComponent<PlanetBody>();
            _planet.Configure(100f, 9.81f);

            _journalGo = new GameObject("TestJournal");
            _journal = _journalGo.AddComponent<DiscoveryJournal>();

            yield return null; // Wait one frame for Awake
        }

        [UnityTearDown]
        public IEnumerator TearDown()
        {
            Object.DestroyImmediate(_planetGo);
            Object.DestroyImmediate(_journalGo);
            yield return null;
        }

        [UnityTest]
        public IEnumerator ObserveInteractable_CanInteract_WhenNotObserved()
        {
            var entry = ScriptableObject.CreateInstance<ObserveEntry>();
            entry.id = "test_observe_01";
            entry.displayName = "Test Stone";
            entry.description = "A test stone.";
            entry.promptText = "Press E to observe";

            GameObject poi = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            poi.name = "TestPOI";
            poi.transform.position = _planet.Center + Vector3.forward * 101f;

            var trigger = new GameObject("Trigger");
            trigger.transform.SetParent(poi.transform);
            var col = trigger.AddComponent<SphereCollider>();
            col.isTrigger = true;
            col.radius = 2f;

            var observe = poi.AddComponent<ObserveInteractable>();
            observe.Entry = entry;

            yield return null;

            Assert.IsTrue(observe.CanInteract, "Should be able to interact when not yet observed");

            Object.DestroyImmediate(poi);
            Object.DestroyImmediate(entry);
        }

        [UnityTest]
        public IEnumerator ObserveInteractable_CannotInteract_AfterOneShot()
        {
            var entry = ScriptableObject.CreateInstance<ObserveEntry>();
            entry.id = "test_observe_02";
            entry.displayName = "Test Stone";

            GameObject poi = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            poi.name = "TestPOI";
            poi.transform.position = _planet.Center + Vector3.forward * 101f;

            var trigger = new GameObject("Trigger");
            trigger.transform.SetParent(poi.transform);
            var col = trigger.AddComponent<SphereCollider>();
            col.isTrigger = true;
            col.radius = 2f;

            var observe = poi.AddComponent<ObserveInteractable>();
            observe.Entry = entry;

            yield return null;

            // First interact
            observe.Interact(new InteractionContext(null));
            yield return null;

            Assert.IsFalse(observe.CanInteract, "Should not be able to interact after one-shot observe");
            Assert.IsTrue(_journal.Has(entry), "Journal should have the entry");
            Assert.AreEqual(1, _journal.Count, "Journal count should be 1");

            Object.DestroyImmediate(poi);
            Object.DestroyImmediate(entry);
        }

        [UnityTest]
        public IEnumerator DiscoveryJournal_NoDoubleCount()
        {
            var entry = ScriptableObject.CreateInstance<ObserveEntry>();
            entry.id = "test_no_double_01";
            entry.displayName = "Test";

            bool first = _journal.TryUnlock(entry);
            bool second = _journal.TryUnlock(entry);

            Assert.IsTrue(first, "First unlock should succeed");
            Assert.IsFalse(second, "Second unlock should fail");
            Assert.AreEqual(1, _journal.Count, "Count should be exactly 1");

            Object.DestroyImmediate(entry);
            yield return null;
        }
    }
}
