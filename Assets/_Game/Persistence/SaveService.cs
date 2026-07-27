using System;
using System.IO;
using UnityEngine;

namespace Asteria.Persistence
{
    /// <summary>
    /// Handles JSON save/load with atomic writes, backups, and schema versioning.
    /// </summary>
    public sealed class SaveService : ISaveService
    {
        const int CurrentSchemaVersion = 1;
        const string SaveFileName = "save.json";
        const string BackupExtension = ".bak";
        const string Backup2Extension = ".bak2";

        SaveRoot _current;
        readonly string _saveDir;

        public SaveRoot Current => _current;

        public SaveService()
        {
            _saveDir = Path.Combine(Application.persistentDataPath, "Saves", "slot_0");
            Directory.CreateDirectory(_saveDir);
        }

        public void LoadOrCreate()
        {
            string savePath = Path.Combine(_saveDir, SaveFileName);

            if (File.Exists(savePath))
            {
                try
                {
                    string json = File.ReadAllText(savePath);
                    _current = JsonUtility.FromJson<SaveRoot>(json);

                    if (_current == null)
                    {
                        throw new Exception("Deserialized save is null");
                    }

                    // Migrate if needed
                    if (_current.schemaVersion < CurrentSchemaVersion)
                    {
                        Migrate(_current);
                    }

                    Debug.Log($"[Asteria] Save loaded: {_current.discoveries.Count} discoveries");
                    return;
                }
                catch (Exception ex)
                {
                    Debug.LogWarning($"[Asteria] Failed to load save: {ex.Message}. Trying backup...");

                    if (TryLoadBackup())
                    {
                        return;
                    }

                    Debug.LogWarning("[Asteria] All saves corrupted. Creating new save.");
                }
            }

            CreateNew();
        }

        public void Save()
        {
            if (_current == null)
            {
                return;
            }

            _current.saveTimestamp = DateTime.UtcNow.ToString("o");
            _current.gameVersion = Application.version;

            string savePath = Path.Combine(_saveDir, SaveFileName);
            string tmpPath = savePath + ".tmp";

            try
            {
                string json = JsonUtility.ToJson(_current, true);
                File.WriteAllText(tmpPath, json);

                // Atomic replace
                string bakPath = savePath + BackupExtension;
                string bak2Path = savePath + Backup2Extension;

                // Rotate backups
                if (File.Exists(bakPath))
                {
                    if (File.Exists(bak2Path))
                    {
                        File.Delete(bak2Path);
                    }

                    File.Move(bakPath, bak2Path);
                }

                if (File.Exists(savePath))
                {
                    File.Move(savePath, bakPath);
                }

                File.Move(tmpPath, savePath);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[Asteria] Failed to save: {ex.Message}");

                if (File.Exists(tmpPath))
                {
                    try { File.Delete(tmpPath); }
                    catch { /* ignore cleanup errors */ }
                }
            }
        }

        void CreateNew()
        {
            _current = new SaveRoot
            {
                schemaVersion = CurrentSchemaVersion,
                saveTimestamp = DateTime.UtcNow.ToString("o"),
                gameVersion = Application.version,
                profileId = "default",
                playerName = "Explorer"
            };

            Save();
            Debug.Log("[Asteria] New save created.");
        }

        bool TryLoadBackup()
        {
            string savePath = Path.Combine(_saveDir, SaveFileName);
            string[] backups = { savePath + BackupExtension, savePath + Backup2Extension };

            foreach (string backupPath in backups)
            {
                if (!File.Exists(backupPath))
                {
                    continue;
                }

                try
                {
                    string json = File.ReadAllText(backupPath);
                    _current = JsonUtility.FromJson<SaveRoot>(json);

                    if (_current != null)
                    {
                        Debug.Log($"[Asteria] Recovered from backup: {backupPath}");
                        Save(); // Re-save as primary
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogWarning($"[Asteria] Backup {backupPath} also corrupted: {ex.Message}");
                }
            }

            return false;
        }

        void Migrate(SaveRoot save)
        {
            // Future migrations go here.
            // Example: if (save.schemaVersion == 1) { migrate to 2; save.schemaVersion = 2; }
            save.schemaVersion = CurrentSchemaVersion;
            Debug.Log($"[Asteria] Save migrated to schema v{CurrentSchemaVersion}");
        }
    }
}
