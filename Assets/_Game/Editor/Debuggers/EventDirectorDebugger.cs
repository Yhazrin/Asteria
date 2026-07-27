#if UNITY_EDITOR
using Asteria.Expedition;
using UnityEditor;
using UnityEngine;

namespace Asteria.Editor
{
    /// <summary>
    /// Editor window to inspect event director state at runtime.
    /// Menu: Asteria/Debug/Event Director
    /// </summary>
    public class EventDirectorDebugger : EditorWindow
    {
        [MenuItem("Asteria/Debug/Event Director", priority = 101)]
        static void ShowWindow()
        {
            GetWindow<EventDirectorDebugger>("Event Director");
        }

        void OnGUI()
        {
            if (!Application.isPlaying)
            {
                EditorGUILayout.HelpBox("Enter Play mode to inspect event director.", MessageType.Info);
                return;
            }

            var director = FindFirstObjectByType<EventDirectorMinimal>();
            if (director == null)
            {
                EditorGUILayout.HelpBox("No EventDirectorMinimal found in scene.", MessageType.Warning);
                return;
            }

            EditorGUILayout.LabelField("Event Director", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            EditorGUILayout.LabelField($"Pressure Active: {director.IsPressureActive}");

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Actions:", EditorStyles.boldLabel);

            if (GUILayout.Button("Force End Pressure"))
            {
                // This would need a public method on EventDirectorMinimal
                Debug.Log("[Debugger] Force end pressure requested");
            }
        }

        void OnInspectorUpdate()
        {
            Repaint();
        }
    }
}
#endif
