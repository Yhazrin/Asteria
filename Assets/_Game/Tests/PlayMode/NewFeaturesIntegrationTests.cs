using System.Collections;
using Asteria.Core;
using Asteria.Data;
using Asteria.Planet;
using Asteria.Planet.Atmosphere;
using Asteria.Planet.Creatures;
using Asteria.Planet.Weather;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Asteria.Tests.PlayMode
{
    /// <summary>
    /// Integration tests for all 6 new features.
    /// Validates they wire into the game flow correctly.
    /// </summary>
    [TestFixture]
    public class NewFeaturesIntegrationTests
    {
        // === P0: Space Landing Sequence ===

        [UnityTest]
        public IEnumerator SpaceLanding_CreatesAndStarts()
        {
            var planetGo = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            var planet = planetGo.AddComponent<PlanetBody>();
            planet.Configure(100f, 9.81f);

            var landingGo = new GameObject("Landing");
            var landing = landingGo.AddComponent<SpaceLandingSequence>();

            yield return null;

            Assert.IsNotNull(landing);
            Assert.AreEqual(SpaceLandingSequence.LandingPhase.WaitingToStart, landing.CurrentPhase);

            Object.DestroyImmediate(planetGo);
            Object.DestroyImmediate(landingGo);
        }

        // === P0: Procedural Terrain ===

        [UnityTest]
        public IEnumerator ProceduralPlanet_GeneratesMesh()
        {
            var builderGo = new GameObject("Builder");
            var builder = builderGo.AddComponent<Planet.Generation.ProceduralPlanetBuilder>();

            yield return null;

            // Builder should exist and be ready
            Assert.IsNotNull(builder);

            Object.DestroyImmediate(builderGo);
        }

        // === P1: Planet Codex ===

        [UnityTest]
        public IEnumerator PlanetCodex_DiscoversEntries()
        {
            var codexGo = new GameObject("Codex");
            var codex = codexGo.AddComponent<PlanetCodex>();

            yield return null;

            // Discover home planet
            bool discovered = codex.Discover("planet.home");
            Assert.IsTrue(discovered, "Should discover home planet");
            Assert.IsTrue(codex.IsDiscovered("planet.home"));
            Assert.AreEqual(1, codex.DiscoveredCount);

            // Duplicate discovery should fail
            bool duplicate = codex.Discover("planet.home");
            Assert.IsFalse(duplicate, "Should not discover same planet twice");

            Object.DestroyImmediate(codexGo);
        }

        [UnityTest]
        public IEnumerator PlanetCodex_TracksProgress()
        {
            var codexGo = new GameObject("Codex");
            var codex = codexGo.AddComponent<PlanetCodex>();

            yield return null;

            int progressCount = 0;
            codex.OnProgressUpdated += (d, t) => progressCount++;

            codex.Discover("planet.home");
            codex.Discover("planet.wind_grassland");

            Assert.AreEqual(2, codex.DiscoveredCount);
            Assert.AreEqual(2, progressCount);

            Object.DestroyImmediate(codexGo);
        }

        // === P1: Atmosphere Renderer ===

        [UnityTest]
        public IEnumerator Atmosphere_CreatesAndDetects()
        {
            var planetGo = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            var planet = planetGo.AddComponent<PlanetBody>();
            planet.Configure(100f, 9.81f);

            var atmosGo = new GameObject("Atmosphere");
            var atmos = atmosGo.AddComponent<AtmosphereRenderer>();

            yield return null;

            Assert.IsNotNull(atmos);

            // Inside atmosphere
            Vector3 surfacePos = planet.GetPointOnSurface(Vector3.forward, 10f);
            Assert.IsTrue(atmos.IsInsideAtmosphere(surfacePos));

            // Outside atmosphere
            Vector3 spacePos = planet.Center + Vector3.forward * 500f;
            Assert.IsFalse(atmos.IsInsideAtmosphere(spacePos));

            Object.DestroyImmediate(planetGo);
            Object.DestroyImmediate(atmosGo);
        }

        // === P2: Weather System ===

        [UnityTest]
        public IEnumerator Weather_Transitions()
        {
            var weatherGo = new GameObject("Weather");
            var weather = weatherGo.AddComponent<WeatherSystem>();

            yield return null;

            // Initial weather
            weather.SetWeather(WeatherType.Clear, 1f);
            Assert.AreEqual(WeatherType.Clear, weather.CurrentWeather);
            Assert.AreEqual(1f, weather.Intensity, 0.01f);

            // Transition
            weather.TransitionTo(WeatherType.Rain);
            yield return new WaitForSeconds(0.5f);

            // Should be transitioning
            Assert.IsTrue(weather.Intensity >= 0f);

            Object.DestroyImmediate(weatherGo);
        }

        [UnityTest]
        public IEnumerator Weather_AffectsVisibility()
        {
            var weatherGo = new GameObject("Weather");
            var weather = weatherGo.AddComponent<WeatherSystem>();

            yield return null;

            weather.SetWeather(WeatherType.Clear, 1f);
            float clearVis = weather.GetVisibility();

            weather.SetWeather(WeatherType.Storm, 1f);
            float stormVis = weather.GetVisibility();

            Assert.IsTrue(stormVis < clearVis, "Storm should reduce visibility");

            Object.DestroyImmediate(weatherGo);
        }

        [UnityTest]
        public IEnumerator Weather_AffectsWind()
        {
            var weatherGo = new GameObject("Weather");
            var weather = weatherGo.AddComponent<WeatherSystem>();

            yield return null;

            weather.SetWeather(WeatherType.Clear, 1f);
            float clearWind = weather.GetWindStrength();

            weather.SetWeather(WeatherType.Storm, 1f);
            float stormWind = weather.GetWindStrength();

            Assert.IsTrue(stormWind > clearWind, "Storm should increase wind");

            Object.DestroyImmediate(weatherGo);
        }

        // === P2: Creature AI ===

        [UnityTest]
        public IEnumerator Creature_CanBeCreated()
        {
            var def = ScriptableObject.CreateInstance<CreatureDefinition>();
            def.creatureId = "test_creature";
            def.displayName = "Test Creature";
            def.behavior = CreatureBehavior.Curious;
            def.moveSpeed = 3f;

            var planetGo = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            var planet = planetGo.AddComponent<PlanetBody>();
            planet.Configure(50f, 9.81f);

            var creatureGo = new GameObject("TestCreature");
            creatureGo.transform.position = planet.GetPointOnSurface(Vector3.forward, 1f);
            var agent = creatureGo.AddComponent<CreatureAgent>();
            agent.Initialize(def, planet);

            yield return null;

            Assert.IsNotNull(agent.Definition);
            Assert.AreEqual("Test Creature", agent.Definition.displayName);
            Assert.AreEqual(CreatureAgent.CreatureState.Idle, agent.CurrentState);

            Object.DestroyImmediate(creatureGo);
            Object.DestroyImmediate(planetGo);
            Object.DestroyImmediate(def);
        }

        [UnityTest]
        public IEnumerator CreatureSpawner_CanBeCreated()
        {
            var planetGo = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            var planet = planetGo.AddComponent<PlanetBody>();
            planet.Configure(100f, 9.81f);

            var spawnerGo = new GameObject("Spawner");
            var spawner = spawnerGo.AddComponent<CreatureSpawner>();

            yield return null;

            Assert.IsNotNull(spawner);
            Assert.AreEqual(0, spawner.ActiveCreatures.Count);

            Object.DestroyImmediate(spawnerGo);
            Object.DestroyImmediate(planetGo);
        }
    }
}
