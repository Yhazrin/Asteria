using System.Collections;
using Asteria.Planet;
using Asteria.Player;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Asteria.Tests.PlayMode
{
    /// <summary>
    /// PlayMode tests for spherical movement.
    /// Required by TEST_SPEC.md PlayMode matrix.
    /// </summary>
    [TestFixture]
    public class SphericalMovementTests
    {
        [UnityTest]
        public IEnumerator Player_StaysOnSurface()
        {
            var planetGo = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            var planet = planetGo.AddComponent<PlanetBody>();
            planet.Configure(50f, 9.81f);

            var playerGo = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            playerGo.transform.position = planet.GetPointOnSurface(Vector3.forward, 1f);

            var rb = playerGo.AddComponent<Rigidbody>();
            rb.useGravity = false;
            rb.constraints = RigidbodyConstraints.FreezeRotation;

            var gravity = playerGo.AddComponent<SphericalGravityBody>();
            gravity.Planet = planet;

            yield return new WaitForSeconds(1f);

            float distFromCenter = (playerGo.transform.position - planet.Center).magnitude;
            Assert.IsTrue(distFromCenter > planet.Radius * 0.9f,
                $"Player should stay on surface, dist={distFromCenter}, radius={planet.Radius}");

            Object.DestroyImmediate(playerGo);
            Object.DestroyImmediate(planetGo);
        }

        [UnityTest]
        public IEnumerator Gravity_PointsTowardCenter()
        {
            var planetGo = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            var planet = planetGo.AddComponent<PlanetBody>();
            planet.Configure(50f, 9.81f);

            var playerGo = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            playerGo.transform.position = planet.GetPointOnSurface(Vector3.forward, 2f);

            var rb = playerGo.AddComponent<Rigidbody>();
            rb.useGravity = false;
            rb.constraints = RigidbodyConstraints.FreezeRotation;

            var gravity = playerGo.AddComponent<SphericalGravityBody>();
            gravity.Planet = planet;

            yield return new WaitForSeconds(0.5f);

            Vector3 expectedDown = (planet.Center - playerGo.transform.position).normalized;
            Vector3 actualUp = gravity.SurfaceUp;
            float dot = Vector3.Dot(actualUp, -expectedDown);

            Assert.IsTrue(dot > 0.9f, $"Gravity should point toward center, dot={dot}");

            Object.DestroyImmediate(playerGo);
            Object.DestroyImmediate(planetGo);
        }

        [UnityTest]
        public IEnumerator SurfaceUp_IsNormalized()
        {
            var planetGo = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            var planet = planetGo.AddComponent<PlanetBody>();
            planet.Configure(100f, 9.81f);

            Vector3[] positions =
            {
                planet.GetPointOnSurface(Vector3.forward, 1f),
                planet.GetPointOnSurface(Vector3.up, 1f),
                planet.GetPointOnSurface(Vector3.down, 1f),
            };

            foreach (var pos in positions)
            {
                Vector3 up = planet.GetSurfaceUp(pos);
                Assert.AreEqual(1f, up.magnitude, 0.01f, $"Surface up should be normalized at {pos}");
            }

            Object.DestroyImmediate(planetGo);
            yield return null;
        }
    }
}
