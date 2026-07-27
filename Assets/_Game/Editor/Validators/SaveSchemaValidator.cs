#if UNITY_EDITOR
using Asteria.Persistence;
using UnityEditor;
using UnityEngine;

namespace Asteria.Editor
{
    /// <summary>
    /// Validates the current save schema.
    /// Menu: Asteria/Validate/Save Schema
    /// </summary>
    public static class SaveSchemaValidator
    {
        [MenuItem("Asteria/Validate/Save Schema", priority = 50)]
        public static void RunFromMenu()
        {
            RunInternal();
        }

        public static void RunFromBatch()
        {
            RunInternal();
        }

        static void RunInternal()
        {
            var service = new SaveService();
            service.LoadOrCreate();

            var save = service.Current;
            int errors = 0;
            int warnings = 0;

            // Check schema version
            if (save.schemaVersion <= 0)
            {
                Debug.LogError("[Validator] schemaVersion must be > 0");
                errors++;
            }

            // Check profile
            if (string.IsNullOrWhiteSpace(save.profileId))
            {
                Debug.LogError("[Validator] profileId is required");
                errors++;
            }

            // Check discoveries
            foreach (var disc in save.discoveries)
            {
                if (string.IsNullOrWhiteSpace(disc.id))
                {
                    Debug.LogError("[Validator] Discovery has empty ID");
                    errors++;
                }

                if (string.IsNullOrWhiteSpace(disc.displayName))
                {
                    Debug.LogWarning($"[Validator] Discovery {disc.id} has no displayName");
                    warnings++;
                }
            }

            // Check residents
            foreach (var res in save.residents)
            {
                if (string.IsNullOrWhiteSpace(res.residentId))
                {
                    Debug.LogError("[Validator] Resident has empty ID");
                    errors++;
                }
            }

            Debug.Log($"[Validator] Save schema validation complete: {errors} errors, {warnings} warnings");
        }
    }
}
#endif
