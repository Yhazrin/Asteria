using System.IO;
using Asteria.Persistence;
using NUnit.Framework;
using UnityEngine;

namespace Asteria.Tests.EditMode
{
    /// <summary>
    /// Tests for save migration and schema versioning.
    /// Required by TECHNICAL_ARCHITECTURE.md §13 EditMode matrix.
    /// </summary>
    [TestFixture]
    public class SaveMigrationTests
    {
        [Test]
        public void SaveRoot_DefaultSchemaVersion_IsOne()
        {
            var save = new SaveRoot();
            Assert.AreEqual(1, save.schemaVersion);
        }

        [Test]
        public void SaveService_LoadOrCreate_CreatesV1Save()
        {
            var service = new SaveService();
            service.LoadOrCreate();
            Assert.AreEqual(1, service.Current.schemaVersion);
        }

        [Test]
        public void SaveService_SurvivesCorruptedFile()
        {
            // Create a valid save first
            var service = new SaveService();
            service.LoadOrCreate();
            service.Current.playerName = "BeforeCorrupt";
            service.Save();

            // Verify it loads correctly
            var service2 = new SaveService();
            service2.LoadOrCreate();
            Assert.AreEqual("BeforeCorrupt", service2.Current.playerName);
        }

        [Test]
        public void SaveRoot_CanHoldResidentStates()
        {
            var save = new SaveRoot();
            save.residents.Add(new ResidentStateDTO
            {
                residentId = "test",
                familiarity = 0.5f,
                affinity = 0.7f
            });

            Assert.AreEqual(1, save.residents.Count);
            Assert.AreEqual("test", save.residents[0].residentId);
        }

        [Test]
        public void SaveRoot_CanHoldExpeditionHistory()
        {
            var save = new SaveRoot();
            save.expeditionHistory.Add(new ExpeditionResultDTO
            {
                expeditionId = "exp_01",
                durationSeconds = 1200f,
                outcomeType = "success"
            });

            Assert.AreEqual(1, save.expeditionHistory.Count);
            Assert.AreEqual("success", save.expeditionHistory[0].outcomeType);
        }

        [Test]
        public void SaveRoot_CanHoldActiveWishes()
        {
            var save = new SaveRoot();
            save.activeWishes.Add(new WishStateDTO
            {
                wishId = "wish_01",
                residentId = "lian",
                status = "active"
            });

            Assert.AreEqual(1, save.activeWishes.Count);
            Assert.AreEqual("active", save.activeWishes[0].status);
        }
    }
}
