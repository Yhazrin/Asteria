using System.IO;
using Asteria.Data;
using Asteria.Interaction;
using Asteria.Planet;
using Asteria.Player;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Asteria.Editor
{
    /// <summary>
    /// Adds Observe POI + configs into the existing SphereMoveDemo without rebuilding the whole scene.
    /// Menu: Asteria / Upgrade Demo With Observe
    /// Batch: -executeMethod Asteria.Editor.Phase1ObserveUpgrade.RunFromBatch
    /// </summary>
    public static class Phase1ObserveUpgrade
    {
        const string DataDir = "Assets/_Game/Data/ScriptableObjects";
        const string AssetsDir = "Assets/_Game/Data/Assets";
        const string MaterialsDir = "Assets/_Game/Environment/Materials";
        const string ScenePath = "Assets/_Game/Planet/Scenes/SphereMoveDemo.unity";
        const string MotorConfigPath = AssetsDir + "/PlayerMotorConfig.asset";
        const string ObserveEntryPath = AssetsDir + "/Observe_WindBellStone.asset";
        const string TraceLimitsPath = AssetsDir + "/TraceLimitsConfig.asset";
        const string PoiMatPath = MaterialsDir + "/M_POI_Observe.mat";

        [MenuItem("Asteria/Upgrade Demo With Observe", priority = 1)]
        public static void RunFromMenu()
        {
            if (!RunInternal())
            {
                EditorUtility.DisplayDialog("Asteria", "升级失败，请查看 Console。", "OK");
                return;
            }

            EditorUtility.DisplayDialog(
                "Asteria",
                "已升级 SphereMoveDemo：\n· 风铃石 Observe POI\n· 移动配置 SO\n· 交互检测与 HUD\n\nPlay 后走向亮色石头，按 E 观察。",
                "OK");
        }

        public static void RunFromBatch()
        {
            if (!RunSilent())
            {
                throw new System.Exception("Phase1ObserveUpgrade failed.");
            }

            Debug.Log("[Asteria] Observe upgrade completed.");
        }

        public static bool RunSilent()
        {
            return RunInternal();
        }

        static bool RunInternal()
        {
            EnsureFolder(DataDir);
            EnsureFolder(AssetsDir);
            EnsureFolder(MaterialsDir);

            PlayerMotorConfig motorConfig = LoadOrCreateMotorConfig();
            ObserveEntry entry = LoadOrCreateObserveEntry();
            LoadOrCreateTraceLimits();
            Material poiMat = LoadOrCreatePoiMaterial();

            if (!File.Exists(ScenePath))
            {
                Debug.LogError("[Asteria] Scene missing: " + ScenePath);
                return false;
            }

            var scene = EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);

            if (Object.FindFirstObjectByType<DiscoveryJournal>() == null)
            {
                new GameObject("DiscoveryJournal").AddComponent<DiscoveryJournal>();
            }

            GameHud hud = Object.FindFirstObjectByType<GameHud>();
            if (hud == null)
            {
                new GameObject("GameHud").AddComponent<GameHud>();
            }

            SphereMoveDemoHud legacy = Object.FindFirstObjectByType<SphereMoveDemoHud>();
            if (legacy != null)
            {
                legacy.enabled = false;
            }

            SphericalMotor motor = Object.FindFirstObjectByType<SphericalMotor>();
            if (motor == null)
            {
                Debug.LogError("[Asteria] Player motor not found in scene.");
                return false;
            }

            SerializedObject motorSo = new SerializedObject(motor);
            motorSo.FindProperty("config").objectReferenceValue = motorConfig;
            motorSo.ApplyModifiedPropertiesWithoutUndo();

            if (motor.GetComponent<InteractionDetector>() == null)
            {
                motor.gameObject.AddComponent<InteractionDetector>();
            }

            PrefabUtility.RecordPrefabInstancePropertyModifications(motor);

            ObserveInteractable existing = Object.FindFirstObjectByType<ObserveInteractable>();
            if (existing == null)
            {
                PlanetBody planet = Object.FindFirstObjectByType<PlanetBody>();
                if (planet == null)
                {
                    Debug.LogError("[Asteria] PlanetBody missing.");
                    return false;
                }

                Vector3 dir = (Vector3.forward + Vector3.right * 0.35f).normalized;
                GameObject poi = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                poi.name = "POI_WindBellStone";
                poi.transform.position = planet.GetPointOnSurface(dir, 2.2f);
                poi.transform.localScale = Vector3.one * 4f;
                poi.transform.up = dir;
                poi.GetComponent<MeshRenderer>().sharedMaterial = poiMat;

                var triggerGo = new GameObject("InteractTrigger");
                triggerGo.transform.SetParent(poi.transform, false);
                SphereCollider trigger = triggerGo.AddComponent<SphereCollider>();
                trigger.isTrigger = true;
                trigger.radius = 1.2f;

                ObserveInteractable observe = poi.AddComponent<ObserveInteractable>();
                SerializedObject obsSo = new SerializedObject(observe);
                obsSo.FindProperty("entry").objectReferenceValue = entry;
                obsSo.ApplyModifiedPropertiesWithoutUndo();
            }

            ObserveDemoBootstrap bootstrap = Object.FindFirstObjectByType<ObserveDemoBootstrap>();
            if (bootstrap == null)
            {
                bootstrap = new GameObject("ObserveDemoBootstrap").AddComponent<ObserveDemoBootstrap>();
            }

            SerializedObject bootSo = new SerializedObject(bootstrap);
            bootSo.FindProperty("fallbackEntry").objectReferenceValue = entry;
            bootSo.FindProperty("motorConfig").objectReferenceValue = motorConfig;
            bootSo.FindProperty("buildIfMissing").boolValue = true;
            bootSo.ApplyModifiedPropertiesWithoutUndo();

            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.SaveScene(scene);
            AssetDatabase.SaveAssets();
            return true;
        }

        static PlayerMotorConfig LoadOrCreateMotorConfig()
        {
            var asset = AssetDatabase.LoadAssetAtPath<PlayerMotorConfig>(MotorConfigPath);
            if (asset != null)
            {
                return asset;
            }

            asset = ScriptableObject.CreateInstance<PlayerMotorConfig>();
            AssetDatabase.CreateAsset(asset, MotorConfigPath);
            return asset;
        }

        static ObserveEntry LoadOrCreateObserveEntry()
        {
            var asset = AssetDatabase.LoadAssetAtPath<ObserveEntry>(ObserveEntryPath);
            if (asset != null)
            {
                return asset;
            }

            asset = ScriptableObject.CreateInstance<ObserveEntry>();
            asset.id = "wind_bell_stone";
            asset.displayName = "风铃石";
            asset.description = "一块被风长期打磨的石头。靠近时能听见很轻的金属颤音。";
            asset.promptText = "按 E 观察 · 风铃石";
            AssetDatabase.CreateAsset(asset, ObserveEntryPath);
            return asset;
        }

        static TraceLimitsConfig LoadOrCreateTraceLimits()
        {
            var asset = AssetDatabase.LoadAssetAtPath<TraceLimitsConfig>(TraceLimitsPath);
            if (asset != null)
            {
                return asset;
            }

            asset = ScriptableObject.CreateInstance<TraceLimitsConfig>();
            AssetDatabase.CreateAsset(asset, TraceLimitsPath);
            return asset;
        }

        static Material LoadOrCreatePoiMaterial()
        {
            var mat = AssetDatabase.LoadAssetAtPath<Material>(PoiMatPath);
            Shader shader = Shader.Find("Universal Render Pipeline/Lit");
            if (shader == null)
            {
                shader = Shader.Find("Universal Render Pipeline/Simple Lit");
            }

            Color c = new Color(0.95f, 0.82f, 0.42f);
            if (mat == null)
            {
                mat = new Material(shader);
                AssetDatabase.CreateAsset(mat, PoiMatPath);
            }
            else
            {
                mat.shader = shader;
            }

            if (mat.HasProperty("_BaseColor"))
            {
                mat.SetColor("_BaseColor", c);
            }

            mat.color = c;
            EditorUtility.SetDirty(mat);
            return mat;
        }

        static void EnsureFolder(string assetPath)
        {
            if (AssetDatabase.IsValidFolder(assetPath))
            {
                return;
            }

            string parent = Path.GetDirectoryName(assetPath)?.Replace('\\', '/');
            string name = Path.GetFileName(assetPath);
            if (string.IsNullOrEmpty(parent) || string.IsNullOrEmpty(name))
            {
                return;
            }

            if (!AssetDatabase.IsValidFolder(parent))
            {
                EnsureFolder(parent);
            }

            AssetDatabase.CreateFolder(parent, name);
        }
    }
}
