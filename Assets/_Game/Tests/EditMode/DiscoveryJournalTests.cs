using Asteria.Data;
using Asteria.Interaction;
using NUnit.Framework;
using UnityEngine;

namespace Asteria.Tests.EditMode
{
    /// <summary>
    /// EditMode tests for DiscoveryJournal single-unlock behavior.
    /// Validates that entries are not double-counted.
    /// </summary>
    [TestFixture]
    public class DiscoveryJournalTests
    {
        GameObject _journalGo;
        DiscoveryJournal _journal;

        [SetUp]
        public void SetUp()
        {
            _journalGo = new GameObject("TestJournal");
            _journal = _journalGo.AddComponent<DiscoveryJournal>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(_journalGo);
        }

        [Test]
        public void TryUnlock_ValidEntry_ReturnsTrue()
        {
            var entry = ScriptableObject.CreateInstance<ObserveEntry>();
            entry.id = "test_entry_01";
            entry.displayName = "Test Entry";

            bool result = _journal.TryUnlock(entry);
            Assert.IsTrue(result, "First unlock should return true");

            Object.DestroyImmediate(entry);
        }

        [Test]
        public void TryUnlock_SameEntryTwice_ReturnsFalseSecondTime()
        {
            var entry = ScriptableObject.CreateInstance<ObserveEntry>();
            entry.id = "test_entry_02";
            entry.displayName = "Test Entry";

            _journal.TryUnlock(entry);
            bool result = _journal.TryUnlock(entry);

            Assert.IsFalse(result, "Second unlock of same entry should return false");
            Assert.AreEqual(1, _journal.Count, "Count should be 1 after double unlock attempt");

            Object.DestroyImmediate(entry);
        }

        [Test]
        public void TryUnlock_NullEntry_ReturnsFalse()
        {
            bool result = _journal.TryUnlock(null);
            Assert.IsFalse(result, "Null entry should return false");
        }

        [Test]
        public void TryUnlock_EmptyId_ReturnsFalse()
        {
            var entry = ScriptableObject.CreateInstance<ObserveEntry>();
            entry.id = "";
            entry.displayName = "Bad Entry";

            bool result = _journal.TryUnlock(entry);
            Assert.IsFalse(result, "Empty ID should return false");

            Object.DestroyImmediate(entry);
        }

        [Test]
        public void TryUnlock_InvalidId_ReturnsFalse()
        {
            var entry = ScriptableObject.CreateInstance<ObserveEntry>();
            entry.id = "Invalid ID With Spaces";
            entry.displayName = "Bad Entry";

            bool result = _journal.TryUnlock(entry);
            Assert.IsFalse(result, "Invalid ID format should return false");

            Object.DestroyImmediate(entry);
        }

        [Test]
        public void Has_UnlockedEntry_ReturnsTrue()
        {
            var entry = ScriptableObject.CreateInstance<ObserveEntry>();
            entry.id = "test_entry_03";
            entry.displayName = "Test Entry";

            _journal.TryUnlock(entry);
            Assert.IsTrue(_journal.Has(entry), "Has should return true for unlocked entry");

            Object.DestroyImmediate(entry);
        }

        [Test]
        public void Has_LockedEntry_ReturnsFalse()
        {
            var entry = ScriptableObject.CreateInstance<ObserveEntry>();
            entry.id = "test_entry_04";
            entry.displayName = "Test Entry";

            Assert.IsFalse(_journal.Has(entry), "Has should return false for locked entry");

            Object.DestroyImmediate(entry);
        }

        [Test]
        public void Has_NullEntry_ReturnsFalse()
        {
            Assert.IsFalse(_journal.Has(null), "Has should return false for null entry");
        }

        [Test]
        public void Count_IncrementsOnNewEntries()
        {
            Assert.AreEqual(0, _journal.Count, "Initial count should be 0");

            var entry1 = ScriptableObject.CreateInstance<ObserveEntry>();
            entry1.id = "test_count_01";
            _journal.TryUnlock(entry1);
            Assert.AreEqual(1, _journal.Count);

            var entry2 = ScriptableObject.CreateInstance<ObserveEntry>();
            entry2.id = "test_count_02";
            _journal.TryUnlock(entry2);
            Assert.AreEqual(2, _journal.Count);

            Object.DestroyImmediate(entry1);
            Object.DestroyImmediate(entry2);
        }

        [Test]
        public void DiscoveryUnlocked_FiresOnNewEntry()
        {
            var entry = ScriptableObject.CreateInstance<ObserveEntry>();
            entry.id = "test_event_01";
            entry.displayName = "Event Test";

            ObserveEntry firedEntry = null;
            _journal.DiscoveryUnlocked += e => firedEntry = e;

            _journal.TryUnlock(entry);

            Assert.IsNotNull(firedEntry, "DiscoveryUnlocked event should fire");
            Assert.AreEqual("test_event_01", firedEntry.id);

            Object.DestroyImmediate(entry);
        }

        [Test]
        public void DiscoveryUnlocked_DoesNotFireOnDuplicate()
        {
            var entry = ScriptableObject.CreateInstance<ObserveEntry>();
            entry.id = "test_event_02";
            entry.displayName = "Event Test";

            int fireCount = 0;
            _journal.DiscoveryUnlocked += _ => fireCount++;

            _journal.TryUnlock(entry);
            _journal.TryUnlock(entry);

            Assert.AreEqual(1, fireCount, "Event should fire only once for duplicate entries");

            Object.DestroyImmediate(entry);
        }
    }
}
