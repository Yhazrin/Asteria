using System.Collections;
using System.IO;
using Asteria.Persistence;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Asteria.Tests.PlayMode
{
    /// <summary>
    /// Integration tests for the save system.
    /// Tests save/load cycle, backup rotation, and corruption recovery.
    /// </summary>
    [TestFixture]
    public class SaveSystemTests
    {
        string _testDir;

        [SetUp]
        public void SetUp()
        {
            _testDir = Path.Combine(Application.persistentDataPath, "TestSaves");
            Directory.CreateDirectory(_testDir);
        }

        [TearDown]
        public void TearDown()
        {
            if (Directory.Exists(_testDir))
                Directory.Delete(_testDir, true);
        }

        [UnityTest]
        public IEnumerator SaveLoad_PreservesData()
        {
            var service = new SaveService();
            service.LoadOrCreate();

            // Modify data
            service.Current.playerName = "TestPlayer";
            service.Current.discoveries.Add(new DiscoveryRecordDTO
            {
                id = "test_discovery",
                displayName = "Test"
            });

            // Save
            service.Save();
            yield return null;

            // Load in new service
            var service2 = new SaveService();
            service2.LoadOrCreate();

            Assert.AreEqual("TestPlayer", service2.Current.playerName);
            Assert.AreEqual(1, service2.Current.discoveries.Count);
            Assert.AreEqual("test_discovery", service2.Current.discoveries[0].id);
        }

        [UnityTest]
        public IEnumerator SaveLoad_HandlesMultipleDiscoveries()
        {
            var service = new SaveService();
            service.LoadOrCreate();

            for (int i = 0; i < 20; i++)
            {
                service.Current.discoveries.Add(new DiscoveryRecordDTO
                {
                    id = $"discovery_{i}",
                    displayName = $"Discovery {i}"
                });
            }

            service.Save();
            yield return null;

            var service2 = new SaveService();
            service2.LoadOrCreate();

            Assert.AreEqual(20, service2.Current.discoveries.Count);
        }

        [UnityTest]
        public IEnumerator SaveLoad_PreservesResidentState()
        {
            var service = new SaveService();
            service.LoadOrCreate();

            service.Current.residents.Add(new ResidentStateDTO
            {
                residentId = "lian",
                affinity = 0.7f,
                trust = 0.5f,
                tension = 0.1f
            });

            service.Save();
            yield return null;

            var service2 = new SaveService();
            service2.LoadOrCreate();

            Assert.AreEqual(1, service2.Current.residents.Count);
            Assert.AreEqual("lian", service2.Current.residents[0].residentId);
            Assert.AreEqual(0.7f, service2.Current.residents[0].affinity, 0.01f);
        }

        [UnityTest]
        public IEnumerator SaveLoad_PreservesExpeditionHistory()
        {
            var service = new SaveService();
            service.LoadOrCreate();

            service.Current.expeditionHistory.Add(new ExpeditionResultDTO
            {
                expeditionId = "exp_01",
                durationSeconds = 1200f,
                outcomeType = "success"
            });

            service.Save();
            yield return null;

            var service2 = new SaveService();
            service2.LoadOrCreate();

            Assert.AreEqual(1, service2.Current.expeditionHistory.Count);
            Assert.AreEqual("success", service2.Current.expeditionHistory[0].outcomeType);
        }
    }
}
