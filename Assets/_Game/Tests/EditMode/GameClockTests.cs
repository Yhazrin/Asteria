using Asteria.Core;
using NUnit.Framework;
using UnityEngine;

namespace Asteria.Tests.EditMode
{
    [TestFixture]
    public class GameClockTests
    {
        GameObject _go;
        GameClock _clock;

        [SetUp]
        public void SetUp()
        {
            _go = new GameObject("TestClock");
            _clock = _go.AddComponent<GameClock>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(_go);
        }

        [Test]
        public void InitialState_Day1_Midnight()
        {
            Assert.AreEqual(1, _clock.WorldDay);
            Assert.AreEqual(0f, _clock.TimeOfDay, 0.001f);
        }

        [Test]
        public void Tick_AdvancesTimeOfDay()
        {
            _clock.Tick(1f);
            Assert.IsTrue(_clock.TimeOfDay > 0f, "TimeOfDay should advance after tick");
            Assert.AreEqual(1, _clock.WorldDay, "Day should not change after small tick");
        }

        [Test]
        public void Tick_IncrementsDay()
        {
            // Default secondsPerDay = 720
            _clock.Tick(720f);
            Assert.AreEqual(2, _clock.WorldDay, "Day should increment after full day");
            Assert.AreEqual(0f, _clock.TimeOfDay, 0.01f, "TimeOfDay should wrap to 0");
        }

        [Test]
        public void SetDay_OverridesCurrentDay()
        {
            _clock.SetDay(5);
            Assert.AreEqual(5, _clock.WorldDay);
        }

        [Test]
        public void SetDay_ClampsMinimum()
        {
            _clock.SetDay(-1);
            Assert.AreEqual(1, _clock.WorldDay);
        }

        [Test]
        public void ElapsedSeconds_TracksAccumulatedTime()
        {
            _clock.Tick(10f);
            _clock.Tick(20f);
            Assert.AreEqual(30f, _clock.ElapsedSeconds, 0.01f);
        }
    }
}
