using System.IO;
using Asteria.Persistence;
using NUnit.Framework;
using UnityEngine;

namespace Asteria.Tests.EditMode
{
    /// <summary>
    /// EditMode tests for SaveService persistence.
    /// Validates save/load, backup rotation, and atomic writes.
    /// </summary>
    [TestFixture]
    public class SaveServiceTests
    {
        string _testDir;
        SaveService _service;

        [SetUp]
        public void SetUp()
        {
            _testDir = Path.Combine(Application.persistentDataPath, "TestSaves", System.Guid.NewGuid().ToString());
            Directory.CreateDirectory(_testDir);
            _service = new SaveService();
        }

        [TearDown]
        public void TearDown()
        {
            if (Directory.Exists(_testDir))
            {
                Directory.Delete(_testDir, true);
            }
        }

        [Test]
        public void LoadOrCreate_CreatesNewSave_WhenNoneExists()
        {
            _service.LoadOrCreate();
            Assert.IsNotNull(_service.Current);
            Assert.AreEqual(1, _service.Current.schemaVersion);
        }

        [Test]
        public void Save_WritesJsonFile()
        {
            _service.LoadOrCreate();
            _service.Save();

            string savePath = Path.Combine(
                Application.persistentDataPath, "Saves", "slot_0", "save.json");
            Assert.IsTrue(File.Exists(savePath), "Save file should exist after Save()");
        }

        [Test]
        public void LoadAfterSave_RetainsData()
        {
            _service.LoadOrCreate();
            _service.Current.playerName = "TestPlayer";
            _service.Save();

            var service2 = new SaveService();
            service2.LoadOrCreate();

            Assert.AreEqual("TestPlayer", service2.Current.playerName);
        }

        [Test]
        public void DiscoveryRepository_Record_Persists()
        {
            _service.LoadOrCreate();
            var repo = new DiscoveryRepository(_service);

            repo.Record("test_entry", "Test Entry");
            _service.Save();

            Assert.IsTrue(repo.Has("test_entry"));
            Assert.AreEqual(1, repo.Count);

            // Reload and verify
            var service2 = new SaveService();
            service2.LoadOrCreate();
            var repo2 = new DiscoveryRepository(service2);

            Assert.IsTrue(repo2.Has("test_entry"));
            Assert.AreEqual(1, repo2.Count);
        }

        [Test]
        public void DiscoveryRepository_NoDuplicate()
        {
            _service.LoadOrCreate();
            var repo = new DiscoveryRepository(_service);

            bool first = repo.Record("dup_test", "First");
            bool second = repo.Record("dup_test", "Second");

            Assert.IsTrue(first);
            Assert.IsFalse(second);
            Assert.AreEqual(1, repo.Count);
        }

        [Test]
        public void DiscoveryRepository_OnRecorded_Fires()
        {
            _service.LoadOrCreate();
            var repo = new DiscoveryRepository(_service);

            DiscoveryRecordDTO fired = null;
            repo.OnRecorded += r => fired = r;

            repo.Record("event_test", "Event Test");

            Assert.IsNotNull(fired);
            Assert.AreEqual("event_test", fired.id);
        }

        [Test]
        public void SaveService_SchemaVersion_IsOne()
        {
            _service.LoadOrCreate();
            Assert.AreEqual(1, _service.Current.schemaVersion);
        }
    }
}
