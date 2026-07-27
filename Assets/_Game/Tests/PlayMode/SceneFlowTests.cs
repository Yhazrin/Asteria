using System.Collections;
using Asteria.Core;
using Asteria.Interaction;
using Asteria.Planet;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Asteria.Tests.PlayMode
{
    /// <summary>
    /// PlayMode tests for the home-expedition-home scene flow.
    /// Validates the core loop from ROADMAP_V2.md Milestone B.
    /// </summary>
    [TestFixture]
    public class SceneFlowTests
    {
        [UnityTest]
        public IEnumerator HomePlanetBootstrap_CreatesPlanet()
        {
            var go = new GameObject("TestBootstrap");
            var bootstrap = go.AddComponent<HomePlanetBootstrap>();

            yield return null;
            yield return null; // Wait for Start()

            var planet = Object.FindFirstObjectByType<PlanetBody>();
            Assert.IsNotNull(planet, "HomePlanetBootstrap should create a planet");

            Object.DestroyImmediate(go);
            Object.DestroyImmediate(planet?.gameObject);
        }

        [UnityTest]
        public IEnumerator HomePlanetBootstrap_CreatesObservatory()
        {
            var go = new GameObject("TestBootstrap");
            var bootstrap = go.AddComponent<HomePlanetBootstrap>();

            yield return null;
            yield return null;

            var obs = GameObject.Find("Observatory");
            Assert.IsNotNull(obs, "HomePlanetBootstrap should create Observatory");

            Object.DestroyImmediate(go);
            var planet = Object.FindFirstObjectByType<PlanetBody>();
            Object.DestroyImmediate(planet?.gameObject);
        }

        [UnityTest]
        public IEnumerator DiscoveryJournal_PersistsAcrossFrames()
        {
            var go = new GameObject("TestJournal");
            var journal = go.AddComponent<DiscoveryJournal>();

            yield return null;

            var entry = ScriptableObject.CreateInstance<Data.ObserveEntry>();
            entry.id = "persist_test";
            entry.displayName = "Persist Test";

            journal.TryUnlock(entry);

            yield return null;

            Assert.IsTrue(journal.Has(entry), "Discovery should persist across frames");
            Assert.AreEqual(1, journal.Count);

            Object.DestroyImmediate(go);
            Object.DestroyImmediate(entry);
        }

        [UnityTest]
        public IEnumerator ObserveInteractable_BlocksAfterUnlock()
        {
            var go = new GameObject("TestJournal");
            var journal = go.AddComponent<DiscoveryJournal>();

            var poi = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            poi.transform.position = Vector3.forward * 10f;

            var entry = ScriptableObject.CreateInstance<Data.ObserveEntry>();
            entry.id = "block_test";
            entry.displayName = "Block Test";

            var observe = poi.AddComponent<ObserveInteractable>();
            observe.Entry = entry;

            yield return null;

            Assert.IsTrue(observe.CanInteract, "Should be interactable before unlock");

            observe.Interact(new InteractionContext(null));
            yield return null;

            Assert.IsFalse(observe.CanInteract, "Should NOT be interactable after one-shot unlock");

            Object.DestroyImmediate(go);
            Object.DestroyImmediate(poi);
            Object.DestroyImmediate(entry);
        }
    }
}
