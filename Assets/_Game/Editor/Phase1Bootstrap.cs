using System.IO;
using Asteria.Planet;
using Asteria.Player;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

namespace Asteria.Editor
{
    /// <summary>
    /// One-shot setup for Phase 0.5 + Phase 1 demo content.
    /// Menu: Asteria / Setup Phase 1 Demo
    /// Batchmode: -executeMethod Asteria.Editor.Phase1Bootstrap.RunFromBatch
    /// </summary>
    public static class Phase1Bootstrap
    {
        const string Root = "Assets/_Game";
        const string SettingsDir = Root + "/Core/Settings";
        const string MaterialsDir = Root + "/Environment/Materials";
        const string PrefabsDir = Root + "/Player/Prefabs";
        const string ScenesDir = Root + "/Planet/Scenes";
        const string ScenePath = ScenesDir + "/SphereMoveDemo.unity";
        const string UrpAssetPath = SettingsDir + "/Asteria_URP.asset";
        const string UrpRendererPath = SettingsDir + "/Asteria_URP_Renderer.asset";

        const float PlanetRadius = 300f;

        [MenuItem("Asteria/Setup Phase 1 Demo", priority = 0)]
        public static void RunFromMenu()
        {
            if (!RunInternal(showDialogs: true))
            {
                return;
            }

            EditorUtility.DisplayDialog(
                "Asteria Phase 1",
                "SphereMoveDemo 已生成。\n\n打开场景后按 Play：\nWASD 移动 · Shift 奔跑 · Space 跳跃 · 鼠标视角",
                "OK");
        }

        public static void RunFromBatch()
        {
            bool ok = RunSilent();
            if (!ok)
            {
                throw new System.Exception("Phase1Bootstrap failed.");
            }

            Debug.Log("[Asteria] Phase 1 bootstrap completed: " + ScenePath);
        }

        public static bool RunSilent()
        {
            return RunInternal(showDialogs: false);
        }

        static bool RunInternal(bool showDialogs)
        {
            EnsureFolders();
            if (!EnsureUrpPipeline())
            {
                if (showDialogs)
                {
                    EditorUtility.DisplayDialog(
                        "Asteria",
                        "URP 尚未就绪。请等 Package 导入完成后再执行 Asteria/Setup Phase 1 Demo。",
                        "OK");
                }

                Debug.LogError("[Asteria] URP package not ready.");
                return false;
            }

            ConfigureProjectSettings();
            CreateMaterials();
            CreateDemoScene();
            AddSceneToBuildSettings();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            return true;
        }

        static void EnsureFolders()
        {
            string[] folders =
            {
                Root,
                Root + "/Core",
                Root + "/Core/Scripts",
                SettingsDir,
                Root + "/Player",
                Root + "/Player/Scripts",
                PrefabsDir,
                Root + "/Planet",
                Root + "/Planet/Scripts",
                Root + "/Planet/Prefabs",
                ScenesDir,
                Root + "/Interaction",
                Root + "/Interaction/Scripts",
                Root + "/Multiplayer",
                Root + "/Multiplayer/Scripts",
                Root + "/Environment",
                MaterialsDir,
                Root + "/UI",
                Root + "/UI/Prefabs",
                Root + "/Audio",
                Root + "/Editor"
            };

            foreach (string folder in folders)
            {
                EnsureFolder(folder);
            }
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

        static bool EnsureUrpPipeline()
        {
            if (Shader.Find("Universal Render Pipeline/Lit") == null)
            {
                return false;
            }

            UniversalRendererData renderer = AssetDatabase.LoadAssetAtPath<UniversalRendererData>(UrpRendererPath);
            if (renderer == null)
            {
                renderer = ScriptableObject.CreateInstance<UniversalRendererData>();
                AssetDatabase.CreateAsset(renderer, UrpRendererPath);
            }

            UniversalRenderPipelineAsset pipeline =
                AssetDatabase.LoadAssetAtPath<UniversalRenderPipelineAsset>(UrpAssetPath);
            if (pipeline == null)
            {
                pipeline = UniversalRenderPipelineAsset.Create(renderer);
                AssetDatabase.CreateAsset(pipeline, UrpAssetPath);
            }

            GraphicsSettings.defaultRenderPipeline = pipeline;
            QualitySettings.renderPipeline = pipeline;
            EditorUtility.SetDirty(pipeline);
            EditorUtility.SetDirty(renderer);
            return true;
        }

        static void ConfigureProjectSettings()
        {
            // Linear color space for URP.
            PlayerSettings.colorSpace = ColorSpace.Linear;

            // Spherical gravity is applied per-body; keep global gravity mild as fallback.
            Physics.gravity = new Vector3(0f, -9.81f, 0f);
        }

        static void CreateMaterials()
        {
            CreateLitMaterial(MaterialsDir + "/M_PlanetSurface.mat", new Color(0.45f, 0.62f, 0.48f));
            CreateLitMaterial(MaterialsDir + "/M_EquatorBand.mat", new Color(0.85f, 0.72f, 0.35f));
            CreateLitMaterial(MaterialsDir + "/M_NorthPole.mat", new Color(0.55f, 0.75f, 0.95f));
            CreateLitMaterial(MaterialsDir + "/M_SouthPole.mat", new Color(0.95f, 0.55f, 0.45f));
            CreateLitMaterial(MaterialsDir + "/M_Player.mat", new Color(0.95f, 0.92f, 0.88f));
            CreateLitMaterial(MaterialsDir + "/M_Marker.mat", new Color(1f, 0.85f, 0.35f));
        }

        static Material CreateLitMaterial(string path, Color color)
        {
            Material existing = AssetDatabase.LoadAssetAtPath<Material>(path);
            Shader shader = Shader.Find("Universal Render Pipeline/Lit");
            if (shader == null)
            {
                shader = Shader.Find("Universal Render Pipeline/Simple Lit");
            }

            if (existing != null)
            {
                existing.shader = shader;
                existing.color = color;
                if (existing.HasProperty("_BaseColor"))
                {
                    existing.SetColor("_BaseColor", color);
                }

                EditorUtility.SetDirty(existing);
                return existing;
            }

            Material mat = new Material(shader) { color = color };
            if (mat.HasProperty("_BaseColor"))
            {
                mat.SetColor("_BaseColor", color);
            }

            AssetDatabase.CreateAsset(mat, path);
            return mat;
        }

        static void CreateDemoScene()
        {
            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            // Lighting
            GameObject lightGo = new GameObject("Directional Light");
            Light light = lightGo.AddComponent<Light>();
            light.type = LightType.Directional;
            light.intensity = 1.15f;
            light.color = new Color(1f, 0.97f, 0.92f);
            light.shadows = LightShadows.Soft;
            lightGo.transform.rotation = Quaternion.Euler(45f, -30f, 0f);

            RenderSettings.ambientMode = AmbientMode.Trilight;
            RenderSettings.ambientSkyColor = new Color(0.55f, 0.7f, 0.9f);
            RenderSettings.ambientEquatorColor = new Color(0.45f, 0.5f, 0.45f);
            RenderSettings.ambientGroundColor = new Color(0.2f, 0.18f, 0.15f);
            RenderSettings.fog = true;
            RenderSettings.fogMode = FogMode.ExponentialSquared;
            RenderSettings.fogDensity = 0.0008f;
            RenderSettings.fogColor = new Color(0.55f, 0.68f, 0.82f);

            // Planet
            GameObject planetGo = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            planetGo.name = "Planet";
            planetGo.transform.position = Vector3.zero;
            // Default Unity sphere mesh radius = 0.5
            float scale = PlanetRadius / 0.5f;
            planetGo.transform.localScale = Vector3.one * scale;
            planetGo.GetComponent<MeshRenderer>().sharedMaterial =
                AssetDatabase.LoadAssetAtPath<Material>(MaterialsDir + "/M_PlanetSurface.mat");

            PlanetBody planet = planetGo.AddComponent<PlanetBody>();
            planet.Configure(PlanetRadius, 9.81f);

            CreateLandmark(
                "EquatorMarker_A",
                planet.GetPointOnSurface(Vector3.forward, 2f),
                Vector3.forward,
                MaterialsDir + "/M_EquatorBand.mat",
                new Vector3(8f, 16f, 8f));

            CreateLandmark(
                "EquatorMarker_B",
                planet.GetPointOnSurface(-Vector3.forward, 2f),
                -Vector3.forward,
                MaterialsDir + "/M_EquatorBand.mat",
                new Vector3(8f, 16f, 8f));

            CreateLandmark(
                "NorthPole",
                planet.GetPointOnSurface(Vector3.up, 4f),
                Vector3.up,
                MaterialsDir + "/M_NorthPole.mat",
                new Vector3(12f, 24f, 12f));

            CreateLandmark(
                "SouthPole",
                planet.GetPointOnSurface(Vector3.down, 4f),
                Vector3.down,
                MaterialsDir + "/M_SouthPole.mat",
                new Vector3(12f, 24f, 12f));

            // Player
            GameObject player = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            player.name = "Player";
            Object.DestroyImmediate(player.GetComponent<CapsuleCollider>());
            CapsuleCollider col = player.AddComponent<CapsuleCollider>();
            col.height = 2f;
            col.radius = 0.4f;
            col.center = new Vector3(0f, 0f, 0f);

            player.GetComponent<MeshRenderer>().sharedMaterial =
                AssetDatabase.LoadAssetAtPath<Material>(MaterialsDir + "/M_Player.mat");

            Vector3 spawnDir = (Vector3.up + Vector3.forward).normalized;
            player.transform.position = planet.GetPointOnSurface(spawnDir, 1.05f);
            planet.AlignTransformToSurface(player.transform, Vector3.Cross(spawnDir, Vector3.right));

            Rigidbody rb = player.AddComponent<Rigidbody>();
            rb.mass = 80f;
            rb.useGravity = false;
            rb.interpolation = RigidbodyInterpolation.Interpolate;
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            rb.constraints = RigidbodyConstraints.FreezeRotation;

            SphericalGravityBody gravity = player.AddComponent<SphericalGravityBody>();
            SerializedObject gravitySo = new SerializedObject(gravity);
            gravitySo.FindProperty("planet").objectReferenceValue = planet;
            gravitySo.ApplyModifiedPropertiesWithoutUndo();

            SphericalMotor motor = player.AddComponent<SphericalMotor>();

            // Camera
            GameObject camGo = new GameObject("Main Camera");
            camGo.tag = "MainCamera";
            Camera cam = camGo.AddComponent<Camera>();
            cam.nearClipPlane = 0.1f;
            cam.farClipPlane = 2500f;
            cam.clearFlags = CameraClearFlags.Skybox;
            camGo.AddComponent<AudioListener>();

            SphericalThirdPersonCamera orbit = camGo.AddComponent<SphericalThirdPersonCamera>();
            SerializedObject camSo = new SerializedObject(orbit);
            camSo.FindProperty("target").objectReferenceValue = player.transform;
            camSo.FindProperty("planet").objectReferenceValue = planet;
            camSo.ApplyModifiedPropertiesWithoutUndo();

            SerializedObject motorSo = new SerializedObject(motor);
            motorSo.FindProperty("cameraTransform").objectReferenceValue = camGo.transform;
            motorSo.FindProperty("gravityBody").objectReferenceValue = gravity;
            motorSo.ApplyModifiedPropertiesWithoutUndo();

            Vector3 up = planet.GetSurfaceUp(player.transform.position);
            camGo.transform.position = player.transform.position + up * 2f - player.transform.forward * 7f;
            camGo.transform.LookAt(player.transform.position + up * 1.4f, up);

            // HUD helper
            GameObject hud = new GameObject("DemoHUD");
            hud.AddComponent<SphereMoveDemoHud>();

            PrefabUtility.SaveAsPrefabAsset(player, PrefabsDir + "/Player.prefab");

            Directory.CreateDirectory(Path.GetDirectoryName(ScenePath)!);
            EditorSceneManager.SaveScene(scene, ScenePath);
            EditorSceneManager.OpenScene(ScenePath);
        }

        static void CreateLandmark(
            string name,
            Vector3 position,
            Vector3 outward,
            string materialPath,
            Vector3 scale)
        {
            GameObject marker = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            marker.name = name;
            marker.transform.position = position;
            marker.transform.localScale = scale;
            marker.transform.up = outward.normalized;
            marker.GetComponent<MeshRenderer>().sharedMaterial =
                AssetDatabase.LoadAssetAtPath<Material>(materialPath);
            marker.GetComponent<Collider>().isTrigger = true;
        }

        static void AddSceneToBuildSettings()
        {
            EditorBuildSettingsScene[] existing = EditorBuildSettings.scenes;
            foreach (EditorBuildSettingsScene s in existing)
            {
                if (s.path == ScenePath)
                {
                    return;
                }
            }

            var list = new EditorBuildSettingsScene[existing.Length + 1];
            existing.CopyTo(list, 0);
            list[^1] = new EditorBuildSettingsScene(ScenePath, true);
            EditorBuildSettings.scenes = list;
        }
    }
}
