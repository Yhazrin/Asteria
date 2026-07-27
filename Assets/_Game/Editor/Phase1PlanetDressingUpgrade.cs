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
    /// Makes the demo planet smoother, textured, and more populated.
    /// Menu: Asteria / Upgrade Planet Visuals And Scatter
    /// Batch: -executeMethod Asteria.Editor.Phase1PlanetDressingUpgrade.RunFromBatch
    /// </summary>
    public static class Phase1PlanetDressingUpgrade
    {
        const string ScenePath = "Assets/_Game/Planet/Scenes/SphereMoveDemo.unity";
        const string MeshPath = "Assets/_Game/Planet/Meshes/PlanetSmooth_96x64.asset";
        const string TexPath = "Assets/_Game/Environment/Textures/T_PlanetSurface.png";
        const string MatPath = "Assets/_Game/Environment/Materials/M_PlanetSurface.mat";
        const string RockMatPath = "Assets/_Game/Environment/Materials/M_ScatterRock.mat";
        const string PlantMatPath = "Assets/_Game/Environment/Materials/M_ScatterPlant.mat";
        const string BeaconMatPath = "Assets/_Game/Environment/Materials/M_ScatterBeacon.mat";
        const string PoiMatPath = "Assets/_Game/Environment/Materials/M_POI_Observe.mat";
        const string ObserveEntryPath = "Assets/_Game/Data/Assets/Observe_WindBellStone.asset";
        const string MotorConfigPath = "Assets/_Game/Data/Assets/PlayerMotorConfig.asset";

        const int LonSegments = 96;
        const int LatSegments = 64;
        const int ScatterCount = 140;
        const int ObserveCount = 8;
        const int BeaconTrailCount = 18;
        const int RandomSeed = 20260726;

        [MenuItem("Asteria/Upgrade Planet Visuals And Scatter", priority = 2)]
        public static void RunFromMenu()
        {
            if (!RunSilent())
            {
                EditorUtility.DisplayDialog("Asteria", "星球视觉升级失败，请看 Console。", "OK");
                return;
            }

            EditorUtility.DisplayDialog(
                "Asteria",
                "已升级：\n· 更圆滑的高分段星球网格\n· 地表纹理\n· 随机散布装饰物 + 多个可观察点\n· 出生点附近信标路径\n\nPlay 后到处走走，找亮色石头按 E。",
                "OK");
        }

        public static void RunFromBatch()
        {
            if (!RunSilent())
            {
                throw new System.Exception("Phase1PlanetDressingUpgrade failed.");
            }

            Debug.Log("[Asteria] Planet dressing upgrade completed.");
        }

        public static bool RunSilent()
        {
            return RunInternal();
        }

        static bool RunInternal()
        {
            EnsureFolder("Assets/_Game/Planet/Meshes");
            EnsureFolder("Assets/_Game/Environment/Textures");
            EnsureFolder("Assets/_Game/Environment/Materials");
            EnsureFolder("Assets/_Game/Data/Assets");

            // Also ensure Observe slice exists.
            if (!Phase1ObserveUpgrade.RunSilent())
            {
                Debug.LogWarning("[Asteria] Observe upgrade reported failure; continuing planet dressing.");
            }

            if (!File.Exists(ScenePath))
            {
                Debug.LogError("[Asteria] Missing scene: " + ScenePath);
                return false;
            }

            Mesh planetMesh = CreateOrUpdatePlanetMesh();
            Texture2D planetTex = CreateOrUpdatePlanetTexture();
            Material planetMat = ApplyPlanetMaterial(planetTex);
            Material rockMat = CreateColorMaterial(RockMatPath, new Color(0.55f, 0.5f, 0.42f));
            Material plantMat = CreateColorMaterial(PlantMatPath, new Color(0.35f, 0.55f, 0.38f));
            Material beaconMat = CreateColorMaterial(BeaconMatPath, new Color(1f, 0.85f, 0.35f), emission: true);
            Material poiMat = CreateColorMaterial(PoiMatPath, new Color(1f, 0.78f, 0.28f), emission: true);

            var scene = EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);
            PlanetBody planet = Object.FindFirstObjectByType<PlanetBody>();
            if (planet == null)
            {
                Debug.LogError("[Asteria] PlanetBody not found.");
                return false;
            }

            UpgradePlanetVisual(planet, planetMesh, planetMat);

            Transform dressingRoot = EnsureChild(planet.transform.root, "PlanetDressing").transform;
            ClearChildren(dressingRoot);

            Random.InitState(RandomSeed);
            ScatterDecor(planet, dressingRoot, rockMat, plantMat);
            PlaceObservePois(planet, dressingRoot, poiMat);
            PlaceBeaconTrail(planet, dressingRoot, beaconMat);
            EnsurePlayerInteraction();

            EditorSceneManager.MarkSceneDirty(scene);
            bool saved = EditorSceneManager.SaveScene(scene);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log(
                $"[Asteria] Planet dressing done. mesh={LonSegments}x{LatSegments}, " +
                $"scatter={ScatterCount}, observe={ObserveCount}, beacons={BeaconTrailCount}, saved={saved}");
            return saved;
        }

        static void UpgradePlanetVisual(PlanetBody planet, Mesh mesh, Material mat)
        {
            GameObject planetGo = planet.gameObject;
            MeshFilter filter = planetGo.GetComponent<MeshFilter>();
            if (filter == null)
            {
                filter = planetGo.AddComponent<MeshFilter>();
            }

            filter.sharedMesh = mesh;

            MeshRenderer renderer = planetGo.GetComponent<MeshRenderer>();
            if (renderer == null)
            {
                renderer = planetGo.AddComponent<MeshRenderer>();
            }

            renderer.sharedMaterial = mat;

            // Keep analytical SphereCollider for stable physics (not mesh collider).
            SphereCollider sphereCollider = planetGo.GetComponent<SphereCollider>();
            if (sphereCollider == null)
            {
                // Replace mesh collider if present.
                MeshCollider meshCollider = planetGo.GetComponent<MeshCollider>();
                if (meshCollider != null)
                {
                    Object.DestroyImmediate(meshCollider);
                }

                sphereCollider = planetGo.AddComponent<SphereCollider>();
            }

            // Unity default sphere mesh radius is 0.5; we keep the same local mesh radius.
            sphereCollider.radius = 0.5f;
            float worldRadius = planet.Radius;
            float scale = worldRadius / 0.5f;
            planetGo.transform.localScale = Vector3.one * scale;
        }

        static void ScatterDecor(PlanetBody planet, Transform root, Material rockMat, Material plantMat)
        {
            Transform decorRoot = EnsureChild(root, "Decor").transform;
            for (int i = 0; i < ScatterCount; i++)
            {
                Vector3 dir = Random.onUnitSphere;
                float height = Random.Range(0.4f, 2.8f);
                Vector3 pos = planet.GetPointOnSurface(dir, height);

                bool plant = Random.value > 0.45f;
                PrimitiveType type = plant
                    ? (Random.value > 0.5f ? PrimitiveType.Cylinder : PrimitiveType.Capsule)
                    : (Random.value > 0.5f ? PrimitiveType.Cube : PrimitiveType.Sphere);

                GameObject go = GameObject.CreatePrimitive(type);
                go.name = plant ? $"Plant_{i:000}" : $"Rock_{i:000}";
                go.transform.SetParent(decorRoot, true);
                go.transform.position = pos;
                go.transform.rotation = Quaternion.FromToRotation(Vector3.up, dir) *
                                        Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);

                if (plant)
                {
                    go.transform.localScale = new Vector3(
                        Random.Range(0.6f, 1.6f),
                        Random.Range(2.5f, 7f),
                        Random.Range(0.6f, 1.6f));
                    go.GetComponent<MeshRenderer>().sharedMaterial = plantMat;
                }
                else
                {
                    float s = Random.Range(1.2f, 4.5f);
                    go.transform.localScale = new Vector3(s, s * Random.Range(0.5f, 1.2f), s);
                    go.GetComponent<MeshRenderer>().sharedMaterial = rockMat;
                }

                // Decor should not block movement harshly.
                Collider col = go.GetComponent<Collider>();
                if (col != null)
                {
                    col.isTrigger = true;
                }
            }
        }

        static void PlaceObservePois(PlanetBody planet, Transform root, Material poiMat)
        {
            Transform poiRoot = EnsureChild(root, "ObservePOIs").transform;
            ObserveEntry entry = AssetDatabase.LoadAssetAtPath<ObserveEntry>(ObserveEntryPath);
            if (entry == null)
            {
                entry = ScriptableObject.CreateInstance<ObserveEntry>();
                entry.id = "wind_bell_stone";
                entry.displayName = "风铃石";
                entry.description = "一块被风长期打磨的石头。靠近时能听见很轻的金属颤音。";
                entry.promptText = "按 E 观察 · 风铃石";
                AssetDatabase.CreateAsset(entry, ObserveEntryPath);
            }

            // Remove old single POI if present at scene root.
            ObserveInteractable[] old = Object.FindObjectsByType<ObserveInteractable>(FindObjectsSortMode.None);
            foreach (ObserveInteractable o in old)
            {
                if (o != null)
                {
                    Object.DestroyImmediate(o.gameObject);
                }
            }

            // First POI near common spawn direction so players find it quickly.
            Vector3[] dirs = new Vector3[ObserveCount];
            dirs[0] = (Vector3.up + Vector3.forward * 0.8f + Vector3.right * 0.2f).normalized;
            for (int i = 1; i < ObserveCount; i++)
            {
                dirs[i] = Random.onUnitSphere;
            }

            for (int i = 0; i < ObserveCount; i++)
            {
                Vector3 dir = dirs[i];
                GameObject poi = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                poi.name = $"POI_WindBellStone_{i:00}";
                poi.transform.SetParent(poiRoot, true);
                poi.transform.position = planet.GetPointOnSurface(dir, 3.5f);
                poi.transform.localScale = Vector3.one * Random.Range(5.5f, 8.5f);
                poi.transform.up = dir;
                poi.GetComponent<MeshRenderer>().sharedMaterial = poiMat;

                var triggerGo = new GameObject("InteractTrigger");
                triggerGo.transform.SetParent(poi.transform, false);
                SphereCollider trigger = triggerGo.AddComponent<SphereCollider>();
                trigger.isTrigger = true;
                trigger.radius = 1.4f;

                ObserveInteractable observe = poi.AddComponent<ObserveInteractable>();
                SerializedObject so = new SerializedObject(observe);
                so.FindProperty("entry").objectReferenceValue = entry;
                // Allow re-observe toast after first unlock: keep oneShot true but prompt still shows.
                so.ApplyModifiedPropertiesWithoutUndo();

                // Small pole to make silhouette obvious from far away.
                GameObject pole = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                pole.name = "BeaconPole";
                pole.transform.SetParent(poi.transform, false);
                pole.transform.localPosition = new Vector3(0f, 1.2f, 0f);
                pole.transform.localScale = new Vector3(0.12f, 1.4f, 0.12f);
                pole.GetComponent<MeshRenderer>().sharedMaterial = poiMat;
                Object.DestroyImmediate(pole.GetComponent<Collider>());
            }
        }

        static void PlaceBeaconTrail(PlanetBody planet, Transform root, Material beaconMat)
        {
            Transform trailRoot = EnsureChild(root, "BeaconTrail").transform;
            Vector3 start = (Vector3.up + Vector3.forward).normalized;
            Vector3 end = (Vector3.up + Vector3.forward * 0.8f + Vector3.right * 0.2f).normalized;

            for (int i = 0; i < BeaconTrailCount; i++)
            {
                float t = (i + 1f) / (BeaconTrailCount + 1f);
                Vector3 dir = Vector3.Slerp(start, end, t).normalized;
                GameObject beacon = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                beacon.name = $"Beacon_{i:00}";
                beacon.transform.SetParent(trailRoot, true);
                beacon.transform.position = planet.GetPointOnSurface(dir, 1.8f);
                beacon.transform.rotation = Quaternion.FromToRotation(Vector3.up, dir);
                beacon.transform.localScale = new Vector3(0.7f, 2.2f, 0.7f);
                beacon.GetComponent<MeshRenderer>().sharedMaterial = beaconMat;
                beacon.GetComponent<Collider>().isTrigger = true;
            }
        }

        static void EnsurePlayerInteraction()
        {
            if (Object.FindFirstObjectByType<DiscoveryJournal>() == null)
            {
                new GameObject("DiscoveryJournal").AddComponent<DiscoveryJournal>();
            }

            if (Object.FindFirstObjectByType<GameHud>() == null)
            {
                new GameObject("GameHud").AddComponent<GameHud>();
            }

            SphericalMotor motor = Object.FindFirstObjectByType<SphericalMotor>();
            if (motor != null && motor.GetComponent<InteractionDetector>() == null)
            {
                motor.gameObject.AddComponent<InteractionDetector>();
            }

            PlayerMotorConfig config = AssetDatabase.LoadAssetAtPath<PlayerMotorConfig>(MotorConfigPath);
            if (motor != null && config != null)
            {
                SerializedObject motorSo = new SerializedObject(motor);
                motorSo.FindProperty("config").objectReferenceValue = config;
                motorSo.ApplyModifiedPropertiesWithoutUndo();
            }
        }

        static Mesh CreateOrUpdatePlanetMesh()
        {
            Mesh mesh = PlanetMeshFactory.CreateUvSphere(LonSegments, LatSegments, 0.5f);
            Mesh existing = AssetDatabase.LoadAssetAtPath<Mesh>(MeshPath);
            if (existing == null)
            {
                AssetDatabase.CreateAsset(mesh, MeshPath);
                return AssetDatabase.LoadAssetAtPath<Mesh>(MeshPath);
            }

            existing.Clear();
            existing.indexFormat = mesh.indexFormat;
            existing.vertices = mesh.vertices;
            existing.normals = mesh.normals;
            existing.uv = mesh.uv;
            existing.triangles = mesh.triangles;
            existing.RecalculateBounds();
            existing.name = mesh.name;
            EditorUtility.SetDirty(existing);
            Object.DestroyImmediate(mesh);
            return existing;
        }

        static Texture2D CreateOrUpdatePlanetTexture()
        {
            const int size = 1024;
            var tex = new Texture2D(size, size, TextureFormat.RGBA32, true, false)
            {
                name = "T_PlanetSurface",
                wrapMode = TextureWrapMode.Repeat,
                filterMode = FilterMode.Bilinear,
                anisoLevel = 4
            };

            // Soft low-saturation planet look: moss / mist / warm dust.
            Color lowland = new Color(0.42f, 0.55f, 0.40f);
            Color highland = new Color(0.58f, 0.62f, 0.48f);
            Color dust = new Color(0.62f, 0.55f, 0.42f);
            Color mist = new Color(0.48f, 0.58f, 0.62f);

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    float u = x / (float)(size - 1);
                    float v = y / (float)(size - 1);

                    float n1 = Mathf.PerlinNoise(u * 4.0f + 11.3f, v * 4.0f + 7.1f);
                    float n2 = Mathf.PerlinNoise(u * 11.0f + 3.7f, v * 11.0f + 19.2f);
                    float n3 = Mathf.PerlinNoise(u * 28.0f + 1.1f, v * 28.0f + 2.4f);
                    float ridge = Mathf.Abs(n1 - 0.5f) * 2f;

                    Color c = Color.Lerp(lowland, highland, n1);
                    c = Color.Lerp(c, dust, Mathf.SmoothStep(0.55f, 0.85f, n2) * 0.55f);
                    c = Color.Lerp(c, mist, Mathf.SmoothStep(0.65f, 0.95f, ridge) * 0.35f);
                    c += (n3 - 0.5f) * 0.04f * Color.white;
                    c.a = 1f;
                    tex.SetPixel(x, y, c);
                }
            }

            tex.Apply(true);

            byte[] png = tex.EncodeToPNG();
            Object.DestroyImmediate(tex);

            EnsureFolder("Assets/_Game/Environment/Textures");
            File.WriteAllBytes(Path.GetFullPath(TexPath), png);
            AssetDatabase.ImportAsset(TexPath, ImportAssetOptions.ForceUpdate);

            TextureImporter importer = AssetImporter.GetAtPath(TexPath) as TextureImporter;
            if (importer != null)
            {
                importer.textureType = TextureImporterType.Default;
                importer.sRGBTexture = true;
                importer.mipmapEnabled = true;
                importer.wrapMode = TextureWrapMode.Repeat;
                importer.filterMode = FilterMode.Bilinear;
                importer.anisoLevel = 8;
                importer.maxTextureSize = 2048;
                importer.SaveAndReimport();
            }

            return AssetDatabase.LoadAssetAtPath<Texture2D>(TexPath);
        }

        static Material ApplyPlanetMaterial(Texture2D tex)
        {
            Shader shader = Shader.Find("Universal Render Pipeline/Lit");
            if (shader == null)
            {
                shader = Shader.Find("Universal Render Pipeline/Simple Lit");
            }

            Material mat = AssetDatabase.LoadAssetAtPath<Material>(MatPath);
            if (mat == null)
            {
                mat = new Material(shader);
                AssetDatabase.CreateAsset(mat, MatPath);
            }
            else
            {
                mat.shader = shader;
            }

            Color tint = new Color(0.92f, 0.95f, 0.88f);
            if (mat.HasProperty("_BaseColor"))
            {
                mat.SetColor("_BaseColor", tint);
            }

            mat.color = tint;

            if (mat.HasProperty("_BaseMap"))
            {
                mat.SetTexture("_BaseMap", tex);
            }

            if (mat.HasProperty("_MainTex"))
            {
                mat.SetTexture("_MainTex", tex);
            }

            if (mat.HasProperty("_Smoothness"))
            {
                mat.SetFloat("_Smoothness", 0.18f);
            }

            EditorUtility.SetDirty(mat);
            return mat;
        }

        static Material CreateColorMaterial(string path, Color color, bool emission = false)
        {
            Shader shader = Shader.Find("Universal Render Pipeline/Lit");
            if (shader == null)
            {
                shader = Shader.Find("Universal Render Pipeline/Simple Lit");
            }

            Material mat = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (mat == null)
            {
                mat = new Material(shader);
                AssetDatabase.CreateAsset(mat, path);
            }
            else
            {
                mat.shader = shader;
            }

            if (mat.HasProperty("_BaseColor"))
            {
                mat.SetColor("_BaseColor", color);
            }

            mat.color = color;

            if (emission && mat.HasProperty("_EmissionColor"))
            {
                mat.EnableKeyword("_EMISSION");
                mat.globalIlluminationFlags = MaterialGlobalIlluminationFlags.None;
                mat.SetColor("_EmissionColor", color * 1.8f);
            }

            if (mat.HasProperty("_Smoothness"))
            {
                mat.SetFloat("_Smoothness", emission ? 0.35f : 0.12f);
            }

            EditorUtility.SetDirty(mat);
            return mat;
        }

        static GameObject EnsureChild(Transform parent, string name)
        {
            Transform existing = parent.Find(name);
            if (existing != null)
            {
                return existing.gameObject;
            }

            // Also search scene root.
            GameObject rootObj = GameObject.Find(name);
            if (rootObj != null)
            {
                return rootObj;
            }

            var go = new GameObject(name);
            go.transform.SetParent(parent, false);
            return go;
        }

        static void ClearChildren(Transform root)
        {
            for (int i = root.childCount - 1; i >= 0; i--)
            {
                Object.DestroyImmediate(root.GetChild(i).gameObject);
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
    }
}
