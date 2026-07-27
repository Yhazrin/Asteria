using Asteria.Core;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Asteria.Editor
{
    /// <summary>
    /// Creates the Bootstrap and HomePlanet scenes for Milestone B.
    /// Menu: Asteria/Setup Milestone B Scenes
    /// Batchmode: -executeMethod Asteria.Editor.MilestoneBSceneSetup.RunFromBatch
    /// </summary>
    public static class MilestoneBSceneSetup
    {
        const string BootstrapScenePath = "Assets/_Game/Core/Scenes/Bootstrap.unity";
        const string HomePlanetScenePath = "Assets/_Game/Core/Scenes/HomePlanet.unity";

        [MenuItem("Asteria/Setup Milestone B Scenes", priority = 10)]
        public static void RunFromMenu()
        {
            if (!EditorUtility.DisplayDialog(
                "Asteria Milestone B",
                "This will create Bootstrap and HomePlanet scenes.\n\n" +
                "Existing scenes at the same path will be overwritten.\n\nContinue?",
                "Yes", "Cancel"))
            {
                return;
            }

            RunInternal();
            EditorUtility.DisplayDialog(
                "Asteria Milestone B",
                "Bootstrap and HomePlanet scenes created.\n\n" +
                "Build Settings updated. Press Play to test the flow.",
                "OK");
        }

        public static void RunFromBatch()
        {
            RunInternal();
            Debug.Log("[Asteria] Milestone B scene setup completed.");
        }

        static void RunInternal()
        {
            EnsureFolders();
            CreateBootstrapScene();
            CreateHomePlanetScene();
            UpdateBuildSettings();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        static void EnsureFolders()
        {
            string[] folders =
            {
                "Assets/_Game/Core/Scenes",
                "Assets/_Game/Persistence",
                "Assets/_Game/Persistence/SaveData",
                "Assets/_Game/Persistence/Repositories"
            };

            foreach (string folder in folders)
            {
                if (!AssetDatabase.IsValidFolder(folder))
                {
                    string parent = System.IO.Path.GetDirectoryName(folder)?.Replace('\\', '/');
                    string name = System.IO.Path.GetFileName(folder);
                    if (!string.IsNullOrEmpty(parent) && !string.IsNullOrEmpty(name))
                    {
                        if (!AssetDatabase.IsValidFolder(parent))
                        {
                            AssetDatabase.CreateFolder(
                                System.IO.Path.GetDirectoryName(parent)?.Replace('\\', '/'),
                                System.IO.Path.GetFileName(parent));
                        }

                        AssetDatabase.CreateFolder(parent, name);
                    }
                }
            }
        }

        static void CreateBootstrapScene()
        {
            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            // GameBootstrap - persists across scenes
            GameObject bootstrapGo = new GameObject("GameBootstrap");
            bootstrapGo.AddComponent<GameBootstrap>();

            // SceneFlowManager
            GameObject flowGo = new GameObject("SceneFlowManager");
            flowGo.AddComponent<SceneFlowManager>();

            // Add a camera for the bootstrap scene
            GameObject camGo = new GameObject("BootstrapCamera");
            camGo.tag = "MainCamera";
            camGo.AddComponent<Camera>();
            camGo.AddComponent<AudioListener>();

            System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(BootstrapScenePath)!);
            EditorSceneManager.SaveScene(scene, BootstrapScenePath);
        }

        static void CreateHomePlanetScene()
        {
            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            // Lighting
            GameObject lightGo = new GameObject("Directional Light");
            Light light = lightGo.AddComponent<Light>();
            light.type = LightType.Directional;
            light.intensity = 1.0f;
            light.color = new Color(1f, 0.97f, 0.92f);
            light.shadows = LightShadows.Soft;
            lightGo.transform.rotation = Quaternion.Euler(45f, -30f, 0f);

            // HomePlanetBootstrap
            GameObject bootstrapGo = new GameObject("HomePlanetBootstrap");
            bootstrapGo.AddComponent<HomePlanetBootstrap>();

            System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(HomePlanetScenePath)!);
            EditorSceneManager.SaveScene(scene, HomePlanetScenePath);
        }

        static void UpdateBuildSettings()
        {
            var scenes = new System.Collections.Generic.List<EditorBuildSettingsScene>();

            // Bootstrap scene must be first
            scenes.Add(new EditorBuildSettingsScene(BootstrapScenePath, true));
            scenes.Add(new EditorBuildSettingsScene(HomePlanetScenePath, true));

            // Keep existing scenes
            foreach (var existing in EditorBuildSettings.scenes)
            {
                if (existing.path != BootstrapScenePath && existing.path != HomePlanetScenePath)
                {
                    scenes.Add(existing);
                }
            }

            EditorBuildSettings.scenes = scenes.ToArray();
        }
    }
}
