using UnityEditor;
using UnityEngine;

namespace Asteria.Editor
{
    /// <summary>
    /// When the Editor finishes importing URP, automatically run Phase 1 setup once.
    /// </summary>
    [InitializeOnLoad]
    public static class Phase1AutoSetup
    {
        const string PrefKey = "Asteria.Phase1Setup.Completed.v2";
        const int MaxRetries = 60;
        static int _retries;

        static Phase1AutoSetup()
        {
            if (EditorPrefs.GetBool(PrefKey, false))
            {
                return;
            }

            EditorApplication.delayCall += TrySetup;
        }

        static void TrySetup()
        {
            if (EditorPrefs.GetBool(PrefKey, false))
            {
                return;
            }

            if (EditorApplication.isCompiling || EditorApplication.isUpdating)
            {
                EditorApplication.delayCall += TrySetup;
                return;
            }

            if (Shader.Find("Universal Render Pipeline/Lit") == null)
            {
                _retries++;
                if (_retries <= MaxRetries)
                {
                    EditorApplication.delayCall += TrySetup;
                }
                else
                {
                    Debug.LogWarning(
                        "[Asteria] URP Lit shader not found after waiting. " +
                        "Open Package Manager, confirm URP is installed, then run Asteria/Setup Phase 1 Demo.");
                }

                return;
            }

            bool ok = false;
            try
            {
                ok = Phase1Bootstrap.RunSilent();
            }
            catch (System.Exception ex)
            {
                Debug.LogError("[Asteria] Auto setup failed: " + ex);
            }

            if (ok)
            {
                EditorPrefs.SetBool(PrefKey, true);
                Debug.Log("[Asteria] Phase 1 demo auto-setup complete. Open SphereMoveDemo and press Play.");
            }
            else
            {
                Debug.LogWarning("[Asteria] Auto setup did not complete. Use menu: Asteria/Setup Phase 1 Demo");
            }
        }

        [MenuItem("Asteria/Reset Phase 1 Auto Setup Flag", priority = 100)]
        static void ResetFlag()
        {
            EditorPrefs.DeleteKey(PrefKey);
            Debug.Log("[Asteria] Auto setup flag cleared. Reimport or recompile to retry.");
        }
    }
}
