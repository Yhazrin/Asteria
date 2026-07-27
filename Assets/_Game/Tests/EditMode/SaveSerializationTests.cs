using System.Collections.Generic;
using Asteria.Persistence;
using NUnit.Framework;
using UnityEngine;

namespace Asteria.Tests.EditMode
{
    /// <summary>
    /// Tests for save serialization and deserialization.
    /// Required by TEST_SPEC.md EditMode matrix.
    /// </summary>
    [TestFixture]
    public class SaveSerializationTests
    {
        [Test]
        public void SaveRoot_SerializesToJson()
        {
            var save = CreateTestSave();
            string json = JsonUtility.ToJson(save, true);

            Assert.IsFalse(string.IsNullOrEmpty(json));
            Assert.IsTrue(json.Contains("test_profile"));
        }

        [Test]
        public void SaveRoot_DeserializesFromJson()
        {
            var save = CreateTestSave();
            string json = JsonUtility.ToJson(save, true);
            var loaded = JsonUtility.FromJson<SaveRoot>(json);

            Assert.IsNotNull(loaded);
            Assert.AreEqual("test_profile", loaded.profileId);
            Assert.AreEqual(1, loaded.schemaVersion);
        }

        [Test]
        public void SaveRoot_PreservesDiscoveries()
        {
            var save = CreateTestSave();
            save.discoveries.Add(new DiscoveryRecordDTO
            {
                id = "test_discovery",
                displayName = "Test Discovery"
            });

            string json = JsonUtility.ToJson(save, true);
            var loaded = JsonUtility.FromJson<SaveRoot>(json);

            Assert.AreEqual(1, loaded.discoveries.Count);
            Assert.AreEqual("test_discovery", loaded.discoveries[0].id);
        }

        [Test]
        public void SaveRoot_PreservesResidents()
        {
            var save = CreateTestSave();
            save.residents.Add(new ResidentStateDTO
            {
                residentId = "test_resident",
                affinity = 0.7f
            });

            string json = JsonUtility.ToJson(save, true);
            var loaded = JsonUtility.FromJson<SaveRoot>(json);

            Assert.AreEqual(1, loaded.residents.Count);
            Assert.AreEqual(0.7f, loaded.residents[0].affinity, 0.001f);
        }

        [Test]
        public void SaveRoot_PreservesExpeditionHistory()
        {
            var save = CreateTestSave();
            save.expeditionHistory.Add(new ExpeditionResultDTO
            {
                expeditionId = "exp_01",
                outcomeType = "success"
            });

            string json = JsonUtility.ToJson(save, true);
            var loaded = JsonUtility.FromJson<SaveRoot>(json);

            Assert.AreEqual(1, loaded.expeditionHistory.Count);
            Assert.AreEqual("success", loaded.expeditionHistory[0].outcomeType);
        }

        [Test]
        public void SaveRoot_PreservesWishes()
        {
            var save = CreateTestSave();
            save.activeWishes.Add(new WishStateDTO
            {
                wishId = "wish_01",
                status = "active"
            });

            string json = JsonUtility.ToJson(save, true);
            var loaded = JsonUtility.FromJson<SaveRoot>(json);

            Assert.AreEqual(1, loaded.activeWishes.Count);
            Assert.AreEqual("active", loaded.activeWishes[0].status);
        }

        SaveRoot CreateTestSave()
        {
            return new SaveRoot
            {
                schemaVersion = 1,
                profileId = "test_profile",
                playerName = "TestPlayer"
            };
        }
    }
}
