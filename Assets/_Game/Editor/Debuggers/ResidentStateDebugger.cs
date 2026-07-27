#if UNITY_EDITOR
using Asteria.Residents;
using UnityEditor;
using UnityEngine;

namespace Asteria.Editor
{
    /// <summary>
    /// Editor window to inspect resident states at runtime.
    /// Menu: Asteria/Debug/Resident State
    /// </summary>
    public class ResidentStateDebugger : EditorWindow
    {
        [MenuItem("Asteria/Debug/Resident State", priority = 100)]
        static void ShowWindow()
        {
            GetWindow<ResidentStateDebugger>("Resident State");
        }

        void OnGUI()
        {
            if (!Application.isPlaying)
            {
                EditorGUILayout.HelpBox("Enter Play mode to inspect resident states.", MessageType.Info);
                return;
            }

            var manager = FindFirstObjectByType<ResidentManager>();
            if (manager == null)
            {
                EditorGUILayout.HelpBox("No ResidentManager found in scene.", MessageType.Warning);
                return;
            }

            EditorGUILayout.LabelField($"Residents: {manager.Agents.Count}", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            foreach (var agent in manager.Agents)
            {
                if (agent == null || agent.Definition == null)
                {
                    continue;
                }

                EditorGUILayout.BeginVertical("box");
                EditorGUILayout.LabelField(agent.Definition.DisplayName, EditorStyles.boldLabel);

                if (agent.State != null)
                {
                    EditorGUI.indentLevel++;
                    EditorGUILayout.LabelField($"Activity: {agent.State.currentActivity}");
                    EditorGUILayout.LabelField($"Destination: {agent.State.currentDestination}");
                    EditorGUILayout.LabelField($"Affinity: {agent.State.affinity:F2}");
                    EditorGUILayout.LabelField($"Trust: {agent.State.trust:F2}");
                    EditorGUILayout.LabelField($"Tension: {agent.State.tension:F2}");
                    EditorGUILayout.LabelField($"Memories: {agent.State.memories.Count}");
                    EditorGUI.indentLevel--;
                }

                EditorGUILayout.EndVertical();
            }
        }

        void OnInspectorUpdate()
        {
            Repaint();
        }
    }
}
#endif
