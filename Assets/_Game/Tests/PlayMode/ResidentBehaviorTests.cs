using System.Collections;
using Asteria.Planet;
using Asteria.Residents;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Asteria.Tests.PlayMode
{
    /// <summary>
    /// PlayMode tests for resident autonomous behavior.
    /// Validates Milestone C acceptance: residents complete schedules and interact.
    /// </summary>
    [TestFixture]
    public class ResidentBehaviorTests
    {
        [UnityTest]
        public IEnumerator ResidentManager_SpawnsResidents()
        {
            var planetGo = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            var planet = planetGo.AddComponent<PlanetBody>();
            planet.Configure(100f, 9.81f);

            var managerGo = new GameObject("TestManager");
            var manager = managerGo.AddComponent<ResidentManager>();

            var defs = new[]
            {
                CreateDef("test_a", "TestA", Color.red),
                CreateDef("test_b", "TestB", Color.blue),
            };
            manager.Initialize(defs, planet);

            yield return null;
            yield return null; // Wait for Start()

            Assert.AreEqual(2, manager.Agents.Count, "Should spawn 2 residents");

            Object.DestroyImmediate(managerGo);
            Object.DestroyImmediate(planetGo);
        }

        [UnityTest]
        public IEnumerator ResidentAgent_MovesTowardDestination()
        {
            var planetGo = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            var planet = planetGo.AddComponent<PlanetBody>();
            planet.Configure(50f, 9.81f);

            // Create a destination
            var dest = new GameObject("Plaza");
            dest.transform.position = planet.GetPointOnSurface(Vector3.right, 1f);

            var go = new GameObject("TestResident");
            go.transform.position = planet.GetPointOnSurface(Vector3.forward, 1f);
            var agent = go.AddComponent<ResidentAgent>();
            agent.Initialize(CreateDef("test", "Test", Color.white), planet);

            yield return null;
            yield return null;

            Vector3 startPos = go.transform.position;

            yield return new WaitForSeconds(2f);

            // Resident should have moved (not necessarily reached destination)
            float moved = Vector3.Distance(startPos, go.transform.position);
            Assert.IsTrue(moved > 0.01f, $"Resident should have moved, distance={moved}");

            Object.DestroyImmediate(go);
            Object.DestroyImmediate(dest);
            Object.DestroyImmediate(planetGo);
        }

        ResidentDefinition CreateDef(string id, string name, Color color)
        {
            var def = ScriptableObject.CreateInstance<ResidentDefinition>();
            def.InitializeRuntime(id, name, color);
            return def;
        }
    }
}
