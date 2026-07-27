using UnityEditor;
using UnityEngine;

namespace Asteria.Editor
{
    /// <summary>
    /// After scripts compile, upgrade the demo scene with Observe content once.
    /// </summary>
    [InitializeOnLoad]
    public static class Phase1ObserveAutoUpgrade
    {
        const string PrefKey = "Asteria.Phase1ObserveUpgrade.Completed.v1";
        const int MaxRetries = 40;
        static int _retries;

        static Phase1ObserveAutoUpgrade()
        {
            if (EditorPrefs.GetBool(PrefKey, false))
            {
                return;
            }

            EditorApplication.delayCall += TryUpgrade;
        }

        static void TryUpgrade()
        {
            if (EditorPrefs.GetBool(PrefKey, false))
            {
                return;
            }

            if (EditorApplication.isCompiling || EditorApplication.isUpdating)
            {
                EditorApplication.delayCall += TryUpgrade;
                return;
            }

            if (Shader.Find("Universal Render Pipeline/Lit") == null)
            {
                _retries++;
                if (_retries <= MaxRetries)
                {
                    EditorApplication.delayCall += TryUpgrade;
                }

                return;
            }

            try
            {
                if (Phase1ObserveUpgrade.RunSilent())
                {
                    EditorPrefs.SetBool(PrefKey, true);
                    Debug.Log(
                        "[Asteria] Observe auto-upgrade complete. " +
                        "Play SphereMoveDemo, walk to the bright stone, press E.");
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogWarning("[Asteria] Observe auto-upgrade failed: " + ex.Message);
            }
        }

        [MenuItem("Asteria/Reset Observe Auto Upgrade Flag", priority = 101)]
        static void ResetFlag()
        {
            EditorPrefs.DeleteKey(PrefKey);
            Debug.Log("[Asteria] Observe auto-upgrade flag cleared.");
        }
    }
}
