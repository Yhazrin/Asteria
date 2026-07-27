using Asteria.Planet;
using NUnit.Framework;
using UnityEngine;

namespace Asteria.Tests.EditMode
{
    /// <summary>
    /// EditMode tests for PlanetBody spherical math.
    /// Validates GetSurfaceUp, GetGravityAcceleration, GetPointOnSurface.
    /// </summary>
    [TestFixture]
    public class PlanetBodyTests
    {
        GameObject _planetGo;
        PlanetBody _planet;

        [SetUp]
        public void SetUp()
        {
            _planetGo = new GameObject("TestPlanet");
            _planet = _planetGo.AddComponent<PlanetBody>();
            _planet.Configure(300f, 9.81f);
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(_planetGo);
        }

        [Test]
        public void GetSurfaceUp_AtSurface_ReturnsNormalizedOutward()
        {
            // Point on the +Z surface
            Vector3 worldPos = _planet.Center + Vector3.forward * 300f;
            Vector3 up = _planet.GetSurfaceUp(worldPos);

            Assert.AreEqual(1f, up.magnitude, 0.001f, "Surface up should be normalized");
            Assert.IsTrue(Vector3.Dot(up, Vector3.forward) > 0.99f, "Surface up at +Z should point toward +Z");
        }

        [Test]
        public void GetSurfaceUp_AtCenter_ReturnsFallback()
        {
            // At the exact center, sqrMagnitude < 0.0001, should return transform.up
            Vector3 up = _planet.GetSurfaceUp(_planet.Center);
            Assert.AreEqual(_planetGo.transform.up, up);
        }

        [Test]
        public void GetSurfaceUp_AtNorthPole_ReturnsUp()
        {
            Vector3 worldPos = _planet.Center + Vector3.up * 300f;
            Vector3 up = _planet.GetSurfaceUp(worldPos);

            Assert.IsTrue(Vector3.Dot(up, Vector3.up) > 0.99f, "Surface up at north pole should be world up");
        }

        [Test]
        public void GetSurfaceUp_AtSouthPole_ReturnsDown()
        {
            Vector3 worldPos = _planet.Center + Vector3.down * 300f;
            Vector3 up = _planet.GetSurfaceUp(worldPos);

            Assert.IsTrue(Vector3.Dot(up, Vector3.down) > 0.99f, "Surface up at south pole should be world down");
        }

        [Test]
        public void GetGravityAcceleration_PointsTowardCenter()
        {
            Vector3 worldPos = _planet.Center + Vector3.forward * 300f;
            Vector3 gravity = _planet.GetGravityAcceleration(worldPos);

            // Gravity should point toward center (negative Z direction)
            Assert.IsTrue(Vector3.Dot(gravity, Vector3.back) > 0.99f, "Gravity should point toward center");
            Assert.AreEqual(9.81f, gravity.magnitude, 0.01f, "Gravity magnitude should equal gravityStrength");
        }

        [Test]
        public void GetPointOnSurface_ReturnsCorrectDistance()
        {
            Vector3 dir = new Vector3(1, 1, 1).normalized;
            Vector3 point = _planet.GetPointOnSurface(dir, 0f);

            float distance = (point - _planet.Center).magnitude;
            Assert.AreEqual(300f, distance, 0.01f, "Point should be at planet radius");
        }

        [Test]
        public void GetPointOnSurface_WithOffset_AddsHeight()
        {
            Vector3 dir = Vector3.forward;
            Vector3 point = _planet.GetPointOnSurface(dir, 5f);

            float distance = (point - _planet.Center).magnitude;
            Assert.AreEqual(305f, distance, 0.01f, "Point with offset should be at radius + offset");
        }

        [Test]
        public void Configure_ClampsMinimumRadius()
        {
            _planet.Configure(-10f, 9.81f);
            Assert.AreEqual(1f, _planet.Radius, "Radius should be clamped to minimum 1");
        }

        [Test]
        public void Configure_ClampsMinimumGravity()
        {
            _planet.Configure(300f, -5f);
            Assert.AreEqual(0.01f, _planet.GravityStrength, 0.001f, "Gravity should be clamped to minimum 0.01");
        }

        [Test]
        public void AlignTransformToSurface_OrientsUpward()
        {
            GameObject target = new GameObject("Target");
            target.transform.position = _planet.Center + Vector3.forward * 301f;

            _planet.AlignTransformToSurface(target.transform, Vector3.right);

            Vector3 expectedUp = _planet.GetSurfaceUp(target.transform.position);
            float dot = Vector3.Dot(target.transform.up, expectedUp);
            Assert.IsTrue(dot > 0.99f, $"Target up should align with surface up, dot={dot}");

            Object.DestroyImmediate(target);
        }
    }
}
