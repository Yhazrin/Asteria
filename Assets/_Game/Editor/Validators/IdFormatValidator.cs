#if UNITY_EDITOR
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Asteria.Data;
using UnityEditor;
using UnityEngine;

namespace Asteria.Editor
{
    /// <summary>
    /// Validates all ObserveEntry IDs follow the stable format.
    /// Menu: Asteria/Validate/ID Format
    /// </summary>
    public static class IdFormatValidator
    {
        static readonly Regex IdPattern = new(@"^[a-z][a-z0-9_]*(\.[a-z][a-z0-9_]*)*$", RegexOptions.Compiled);

        [MenuItem("Asteria/Validate/ID Format", priority = 51)]
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
            // Find all ObserveEntry assets
            string[] guids = AssetDatabase.FindAssets("t:ObserveEntry");
            int errors = 0;
            int warnings = 0;
            var seenIds = new HashSet<string>();

            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                var entry = AssetDatabase.LoadAssetAtPath<ObserveEntry>(path);

                if (entry == null)
                {
                    continue;
                }

                if (string.IsNullOrWhiteSpace(entry.id))
                {
                    Debug.LogError($"[Validator] {path}: ID is empty");
                    errors++;
                    continue;
                }

                if (!IdPattern.IsMatch(entry.id))
                {
                    Debug.LogError($"[Validator] {path}: Invalid ID format '{entry.id}'");
                    errors++;
                }

                if (!seenIds.Add(entry.id))
                {
                    Debug.LogError($"[Validator] {path}: Duplicate ID '{entry.id}'");
                    errors++;
                }
            }

            Debug.Log($"[Validator] ID format validation complete: {errors} errors, {warnings} warnings, {seenIds.Count} unique IDs");
        }
    }
}
#endif
