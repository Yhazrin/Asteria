using System.Collections;
using Asteria.Core;
using Asteria.Interaction;
using Asteria.Planet;
using Asteria.Residents;
using Asteria.UI;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Asteria.Tests.PlayMode
{
    /// <summary>
    /// Comprehensive integration tests validating all systems work together.
    /// Tests the complete game flow: Bootstrap → Home → Expedition → Return.
    /// </summary>
    [TestFixture]
    public class IntegrationTests
    {
        [UnityTest]
        public IEnumerator FullFlow_BootstrapCreatesAllSystems()
        {
            // Create bootstrap
            var bootstrapGo = new GameObject("TestBootstrap");
            var bootstrap = bootstrapGo.AddComponent<GameBootstrap>();

            yield return null;
            yield return null;

            // Verify all services exist
            Assert.IsNotNull(GameBootstrap.Instance, "GameBootstrap should exist");
            Assert.IsNotNull(GameBootstrap.Instance.GameClock, "GameClock should exist");
            Assert.IsNotNull(GameBootstrap.Instance.WorldState, "WorldStateService should exist");
            Assert.IsNotNull(GameBootstrap.Instance.Relationships, "RelationshipService should exist");
            Assert.IsNotNull(GameBootstrap.Instance.SaveService, "SaveService should exist");
            Assert.IsNotNull(GameBootstrap.Instance.DiscoveryRepository, "DiscoveryRepository should exist");

            Object.DestroyImmediate(bootstrapGo);
        }

        [UnityTest]
        public IEnumerator FullFlow_HomePlanetCreatesAllFeatures()
        {
            // Create bootstrap first
            var bootstrapGo = new GameObject("TestBootstrap");
            bootstrapGo.AddComponent<GameBootstrap>();

            yield return null;

            // Create home planet
            var homeGo = new GameObject("TestHome");
            homeGo.AddComponent<HomePlanetBootstrap>();

            yield return null;
            yield return null;

            // Verify planet exists
            var planet = Object.FindFirstObjectByType<PlanetBody>();
            Assert.IsNotNull(planet, "Planet should exist");

            // Verify key locations
            Assert.IsNotNull(GameObject.Find("Observatory"), "Observatory should exist");
            Assert.IsNotNull(GameObject.Find("DepartureBeacon"), "DepartureBeacon should exist");
            Assert.IsNotNull(GameObject.Find("ResidentialArea"), "ResidentialArea should exist");
            Assert.IsNotNull(GameObject.Find("Plaza"), "Plaza should exist");

            // Verify environment systems
            Assert.IsNotNull(Object.FindFirstObjectByType<DayNightCycle>(), "DayNightCycle should exist");
            Assert.IsNotNull(Object.FindFirstObjectByType<WindEffect>(), "WindEffect should exist");

            // Verify UI systems
            Assert.IsNotNull(Object.FindFirstObjectByType<GameUIBridge>(), "GameUIBridge should exist");
            Assert.IsNotNull(Object.FindFirstObjectByType<Compass>(), "Compass should exist");
            Assert.IsNotNull(Object.FindFirstObjectByType<SphericalMiniMap>(), "MiniMap should exist");
            Assert.IsNotNull(Object.FindFirstObjectByType<PhotoMode>(), "PhotoMode should exist");

            // Verify residents
            var manager = Object.FindFirstObjectByType<ResidentManager>();
            Assert.IsNotNull(manager, "ResidentManager should exist");
            Assert.IsTrue(manager.Agents.Count >= 2, "Should have at least 2 residents");

            // Verify resident features
            foreach (var agent in manager.Agents)
            {
                Assert.IsNotNull(agent.GetComponentInChildren<ResidentMoodBubble>(),
                    $"Resident {agent.Definition.DisplayName} should have mood bubble");
                Assert.IsNotNull(agent.GetComponentInChildren<ResidentDialogueBubble>(),
                    $"Resident {agent.Definition.DisplayName} should have dialogue bubble");
            }

            // Cleanup
            Object.DestroyImmediate(bootstrapGo);
            Object.DestroyImmediate(homeGo);
            Object.DestroyImmediate(planet?.gameObject);
        }

        [UnityTest]
        public IEnumerator FullFlow_ExpeditionCreatesAllFeatures()
        {
            // Create planet
            var planetGo = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            var planet = planetGo.AddComponent<PlanetBody>();
            planet.Configure(100f, 9.81f);

            // Create player
            var playerGo = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            playerGo.transform.position = planet.GetPointOnSurface(Vector3.forward, 1f);
            playerGo.AddComponent<Rigidbody>().useGravity = false;
            playerGo.AddComponent<SphericalGravityBody>().Planet = planet;
            playerGo.AddComponent<SphericalMotor>();
            playerGo.AddComponent<LegacyInputAdapter>();
            playerGo.AddComponent<InteractionDetector>();

            // Create camera
            var camGo = new GameObject("Camera");
            camGo.tag = "MainCamera";
            camGo.AddComponent<Camera>();
            camGo.AddComponent<AudioListener>();
            var orbit = camGo.AddComponent<SphericalThirdPersonCamera>();
            orbit.Target = playerGo.transform;
            orbit.Planet = planet;

            yield return null;

            // Setup expedition features
            var windGo = new GameObject("Wind");
            windGo.AddComponent<WindEffect>();

            var pressureGo = new GameObject("Pressure");
            playerGo.AddComponent<PlayerPressureState>();

            var directorGo = new GameObject("Director");
            directorGo.AddComponent<EventDirectorMinimal>();

            var toolGo = new GameObject("Tools");
            toolGo.AddComponent<ToolPlacementSystem>();

            yield return null;

            // Verify expedition systems
            Assert.IsNotNull(Object.FindFirstObjectByType<WindEffect>(), "Wind should exist");
            Assert.IsNotNull(Object.FindFirstObjectByType<PlayerPressureState>(), "Pressure should exist");
            Assert.IsNotNull(Object.FindFirstObjectByType<EventDirectorMinimal>(), "Director should exist");
            Assert.IsNotNull(Object.FindFirstObjectByType<ToolPlacementSystem>(), "ToolPlacement should exist");

            // Cleanup
            Object.DestroyImmediate(planetGo);
            Object.DestroyImmediate(playerGo);
            Object.DestroyImmediate(camGo);
            Object.DestroyImmediate(windGo);
            Object.DestroyImmediate(pressureGo);
            Object.DestroyImmediate(directorGo);
            Object.DestroyImmediate(toolGo);
        }

        [UnityTest]
        public IEnumerator FullFlow_DiscoveryPersistsAcrossFrames()
        {
            var journalGo = new GameObject("Journal");
            var journal = journalGo.AddComponent<DiscoveryJournal>();

            yield return null;

            var entry = ScriptableObject.CreateInstance<Data.ObserveEntry>();
            entry.id = "integration_test";
            entry.displayName = "Integration Test";

            // Unlock
            bool unlocked = journal.TryUnlock(entry);
            Assert.IsTrue(unlocked, "Should unlock");

            yield return null;

            // Verify persists
            Assert.IsTrue(journal.Has(entry), "Should persist across frames");
            Assert.AreEqual(1, journal.Count, "Count should be 1");

            // Try duplicate
            bool duplicate = journal.TryUnlock(entry);
            Assert.IsFalse(duplicate, "Should not unlock duplicate");

            Object.DestroyImmediate(journalGo);
            Object.DestroyImmediate(entry);
        }
    }
}
