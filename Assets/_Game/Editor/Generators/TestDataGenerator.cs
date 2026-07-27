#if UNITY_EDITOR
using System.IO;
using Asteria.Persistence;
using UnityEditor;
using UnityEngine;

namespace Asteria.Editor
{
    /// <summary>
    /// Generates test data fixtures for EditMode/PlayMode tests.
    /// Menu: Asteria/Generate/Test Data
    /// </summary>
    public static class TestDataGenerator
    {
        const string FixtureDir = "Assets/_Game/Tests/Fixtures";

        [MenuItem("Asteria/Generate/Test Data", priority = 70)]
        public static void RunFromMenu()
        {
            if (!EditorUtility.DisplayDialog(
                "Generate Test Data",
                "This will create test fixture files. Continue?",
                "Yes", "Cancel"))
            {
                return;
            }

            RunInternal();
        }

        public static void RunFromBatch()
        {
            RunInternal();
        }

        static void RunInternal()
        {
            Directory.CreateDirectory(FixtureDir);

            CreateBasicSave();
            CreateFullSave();

            AssetDatabase.Refresh();
            Debug.Log($"[Generator] Test data generated in {FixtureDir}");
        }

        static void CreateBasicSave()
        {
            var save = new SaveRoot
            {
                schemaVersion = 1,
                profileId = "test_basic",
                playerName = "TestPlayer"
            };

            string json = JsonUtility.ToJson(save, true);
            File.WriteAllText(Path.Combine(FixtureDir, "save_v1_basic.json"), json);
        }

        static void CreateFullSave()
        {
            var save = new SaveRoot
            {
                schemaVersion = 1,
                profileId = "test_full",
                playerName = "FullPlayer"
            };

            save.discoveries.Add(new DiscoveryRecordDTO
            {
                id = "observe_wind_bell_01",
                displayName = "风铃石",
                timestamp = System.DateTime.UtcNow.ToString("o")
            });

            save.residents.Add(new ResidentStateDTO
            {
                residentId = "lian",
                affinity = 0.7f,
                trust = 0.5f
            });

            save.expeditionHistory.Add(new ExpeditionResultDTO
            {
                expeditionId = "exp_01",
                durationSeconds = 1200f,
                outcomeType = "success"
            });

            string json = JsonUtility.ToJson(save, true);
            File.WriteAllText(Path.Combine(FixtureDir, "save_v1_full.json"), json);
        }
    }
}
#endif
