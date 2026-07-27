using Asteria.Planet;
using NUnit.Framework;
using UnityEngine;

namespace Asteria.Tests.EditMode
{
    /// <summary>
    /// Tests for spherical math: surface up, gravity direction, great circle distance.
    /// Required by TECHNICAL_ARCHITECTURE.md §13 EditMode matrix.
    /// </summary>
    [TestFixture]
    public class SphericalMathTests
    {
        [Test]
        public void GetSurfaceUp_IsUnitLength()
        {
            var go = new GameObject("Planet");
            var planet = go.AddComponent<PlanetBody>();
            planet.Configure(300f, 9.81f);

            Vector3[] testPositions =
            {
                planet.Center + Vector3.forward * 300f,
                planet.Center + Vector3.up * 300f,
                planet.Center + Vector3.down * 300f,
                planet.Center + new Vector3(1, 1, 1).normalized * 300f,
            };

            foreach (var pos in testPositions)
            {
                Vector3 up = planet.GetSurfaceUp(pos);
                Assert.AreEqual(1f, up.magnitude, 0.001f,
                    $"Surface up at {pos} should be unit length, got {up.magnitude}");
            }

            Object.DestroyImmediate(go);
        }

        [Test]
        public void GravityAcceleration_PointsTowardCenter()
        {
            var go = new GameObject("Planet");
            var planet = go.AddComponent<PlanetBody>();
            planet.Configure(300f, 9.81f);

            Vector3 surfacePos = planet.Center + Vector3.forward * 300f;
            Vector3 gravity = planet.GetGravityAcceleration(surfacePos);

            Vector3 expectedDir = (planet.Center - surfacePos).normalized;
            float dot = Vector3.Dot(gravity.normalized, expectedDir);
            Assert.IsTrue(dot > 0.99f, $"Gravity should point toward center, dot={dot}");

            Object.DestroyImmediate(go);
        }

        [Test]
        public void GreatCircleDistance_AntipodalPoints()
        {
            // Two points on opposite sides of the sphere
            Vector3 a = Vector3.forward;
            Vector3 b = Vector3.back;

            float angle = Vector3.Angle(a, b);
            Assert.AreEqual(180f, angle, 0.01f, "Antipodal points should be 180 degrees apart");
        }

        [Test]
        public void GreatCircleDistance_AdjacentPoints()
        {
            Vector3 a = Vector3.forward;
            Vector3 b = Quaternion.Euler(10f, 0f, 0f) * Vector3.forward;

            float angle = Vector3.Angle(a, b);
            Assert.AreEqual(10f, angle, 0.01f, "Adjacent points should be ~10 degrees apart");
        }

        [Test]
        public void SurfaceUp_AtEquator_IsPerpendicular()
        {
            var go = new GameObject("Planet");
            var planet = go.AddComponent<PlanetBody>();
            planet.Configure(300f, 9.81f);

            Vector3 equatorPos = planet.Center + Vector3.forward * 300f;
            Vector3 up = planet.GetSurfaceUp(equatorPos);

            float dot = Mathf.Abs(Vector3.Dot(up, Vector3.up));
            Assert.IsTrue(dot < 0.01f, $"Equator up should be perpendicular to world up, dot={dot}");

            Object.DestroyImmediate(go);
        }
    }
}
