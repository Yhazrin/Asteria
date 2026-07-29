#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using System.IO;

namespace Asteria.Editor
{
    /// <summary>
    /// Editor tool to import free assets for Asteria.
    /// Uses Unity Package Manager to install free packages with sample assets.
    /// Menu: Asteria/Import/Free Assets
    /// </summary>
    public static class AssetImporter
    {
        [MenuItem("Asteria/Import/Free Assets/1. TextMeshPro (UI Text)", priority = 10)]
        public static void ImportTextMeshPro()
        {
            InstallPackage("com.unity.textmeshpro", "TextMeshPro");
        }

        [MenuItem("Asteria/Import/Free Assets/2. Post Processing (Visual Effects)", priority = 11)]
        public static void ImportPostProcessing()
        {
            InstallPackage("com.unity.postprocessing", "Post Processing");
        }

        [MenuItem("Asteria/Import/Free Assets/3. Cinemachine (Camera)", priority = 12)]
        public static void ImportCinemachine()
        {
            InstallPackage("com.unity.cinemachine", "Cinemachine");
        }

        [MenuItem("Asteria/Import/Free Assets/4. Input System (New Input)", priority = 13)]
        public static void ImportInputSystem()
        {
            InstallPackage("com.unity.inputsystem", "Input System");
        }

        [MenuItem("Asteria/Import/Free Assets/5. ProBuilder (3D Modeling)", priority = 14)]
        public static void ImportProBuilder()
        {
            InstallPackage("com.unity.probuilder", "ProBuilder");
        }

        [MenuItem("Asteria/Import/Free Assets/6. Shader Graph (Visual Shaders)", priority = 15)]
        public static void ImportShaderGraph()
        {
            InstallPackage("com.unity.shadergraph", "Shader Graph");
        }

        [MenuItem("Asteria/Import/Free Assets/7. VFX Graph (Particle Effects)", priority = 16)]
        public static void ImportVFXGraph()
        {
            InstallPackage("com.unity.visualeffectgraph", "VFX Graph");
        }

        [MenuItem("Asteria/Import/Free Assets/8. All Recommended", priority = 20)]
        public static void ImportAllRecommended()
        {
            if (!EditorUtility.DisplayDialog(
                "Import All Recommended Assets",
                "This will install:\n" +
                "- TextMeshPro (UI)\n" +
                "- Post Processing\n" +
                "- Cinemachine\n" +
                "- Input System\n" +
                "- ProBuilder\n" +
                "- Shader Graph\n" +
                "- VFX Graph\n\n" +
                "Continue?",
                "Yes", "Cancel"))
            {
                return;
            }

            InstallPackage("com.unity.textmeshpro", "TextMeshPro");
            InstallPackage("com.unity.postprocessing", "Post Processing");
            InstallPackage("com.unity.cinemachine", "Cinemachine");
            InstallPackage("com.unity.inputsystem", "Input System");
            InstallPackage("com.unity.probuilder", "ProBuilder");
            InstallPackage("com.unity.shadergraph", "Shader Graph");
            InstallPackage("com.unity.visualeffectgraph", "VFX Graph");

            EditorUtility.DisplayDialog(
                "Import Complete",
                "All recommended packages have been installed.\n\n" +
                "You may need to restart Unity for some packages to take effect.",
                "OK");
        }

        [MenuItem("Asteria/Import/Free Assets/9. Create Material Pack", priority = 30)]
        public static void CreateMaterialPack()
        {
            string matDir = "Assets/_Game/Art/Materials";
            if (!AssetDatabase.IsValidFolder(matDir))
            {
                AssetDatabase.CreateFolder("Assets/_Game/Art", "Materials");
            }

            // Create terrain materials
            CreateMaterial(matDir + "/M_Grass.mat", new Color(0.45f, 0.62f, 0.48f), "Terrain");
            CreateMaterial(matDir + "/M_Rock.mat", new Color(0.55f, 0.5f, 0.45f), "Terrain");
            CreateMaterial(matDir + "/M_Snow.mat", new Color(0.92f, 0.92f, 0.95f), "Terrain");
            CreateMaterial(matDir + "/M_Sand.mat", new Color(0.85f, 0.75f, 0.5f), "Terrain");
            CreateMaterial(matDir + "/M_Water.mat", new Color(0.3f, 0.5f, 0.7f), "Water");

            // Create object materials
            CreateMaterial(matDir + "/M_Tree_Trunk.mat", new Color(0.5f, 0.35f, 0.2f), "Nature");
            CreateMaterial(matDir + "/M_Tree_Leaves.mat", new Color(0.3f, 0.55f, 0.3f), "Nature");
            CreateMaterial(matDir + "/M_Crystal.mat", new Color(0.6f, 0.85f, 1f), "Crystal");
            CreateMaterial(matDir + "/M_WindBell.mat", new Color(0.95f, 0.85f, 0.4f), "Special");
            CreateMaterial(matDir + "/M_Beacon.mat", new Color(0.95f, 0.7f, 0.3f), "Special");

            // Create character materials
            CreateMaterial(matDir + "/M_Resident.mat", new Color(0.9f, 0.8f, 0.75f), "Character");
            CreateMaterial(matDir + "/M_Creature.mat", new Color(0.8f, 0.75f, 0.7f), "Character");

            AssetDatabase.Refresh();
            EditorUtility.DisplayDialog("Material Pack Created", $"Materials saved to {matDir}", "OK");
        }

        [MenuItem("Asteria/Import/Free Assets/10. Create Prefab Pack", priority = 31)]
        public static void CreatePrefabPack()
        {
            string prefabDir = "Assets/_Game/Art/Prefabs";
            if (!AssetDatabase.IsValidFolder(prefabDir))
            {
                AssetDatabase.CreateFolder("Assets/_Game/Art", "Prefabs");
            }

            // Create tree prefab
            CreatePrefabFromPrimitive(prefabDir + "/Tree.prefab", PrimitiveType.Cylinder,
                new Vector3(0.5f, 2f, 0.5f), new Color(0.3f, 0.55f, 0.3f));

            // Create rock prefab
            CreatePrefabFromPrimitive(prefabDir + "/Rock.prefab", PrimitiveType.Sphere,
                new Vector3(1f, 0.6f, 1f), new Color(0.55f, 0.5f, 0.45f));

            // Create crystal prefab
            CreatePrefabFromPrimitive(prefabDir + "/Crystal.prefab", PrimitiveType.Cube,
                new Vector3(0.3f, 1.5f, 0.3f), new Color(0.6f, 0.85f, 1f));

            // Create beacon prefab
            CreatePrefabFromPrimitive(prefabDir + "/Beacon.prefab", PrimitiveType.Cylinder,
                new Vector3(0.5f, 3f, 0.5f), new Color(0.95f, 0.7f, 0.3f));

            AssetDatabase.Refresh();
            EditorUtility.DisplayDialog("Prefab Pack Created", $"Prefabs saved to {prefabDir}", "OK");
        }

        static void InstallPackage(string packageName, string displayName)
        {
            Debug.Log($"[Asteria] Installing {displayName} ({packageName})...");
            var request = Client.Add(packageName);
            EditorApplication.update += () => PollRequest(request, displayName);
        }

        static void PollRequest(AddRequest request, string displayName)
        {
            if (request.IsCompleted)
            {
                if (request.Status == StatusCode.Success)
                {
                    Debug.Log($"[Asteria] {displayName} installed successfully.");
                }
                else if (request.Status >= StatusCode.Failure)
                {
                    Debug.LogError($"[Asteria] Failed to install {displayName}: {request.Error.message}");
                }
                EditorApplication.update -= () => PollRequest(request, displayName);
            }
        }

        static void CreateMaterial(string path, Color color, string category)
        {
            var existing = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (existing != null) return;

            Shader shader = Shader.Find("Universal Render Pipeline/Lit")
                ?? Shader.Find("Universal Render Pipeline/Simple Lit")
                ?? Shader.Find("Standard");

            var mat = new Material(shader);
            mat.name = Path.GetFileNameWithoutExtension(path);

            if (mat.HasProperty("_BaseColor")) mat.SetColor("_BaseColor", color);
            if (mat.HasProperty("_Color")) mat.SetColor("_Color", color);
            mat.color = color;

            if (mat.HasProperty("_Smoothness")) mat.SetFloat("_Smoothness", 0.2f);

            AssetDatabase.CreateAsset(mat, path);
            Debug.Log($"[Asteria] Created material: {path}");
        }

        static void CreatePrefabFromPrimitive(string path, PrimitiveType type, Vector3 scale, Color color)
        {
            var existing = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (existing != null) return;

            var go = GameObject.CreatePrimitive(type);
            go.transform.localScale = scale;

            var renderer = go.GetComponent<MeshRenderer>();
            if (renderer != null)
            {
                Shader shader = Shader.Find("Universal Render Pipeline/Lit")
                    ?? Shader.Find("Universal Render Pipeline/Simple Lit")
                    ?? Shader.Find("Standard");
                var mat = new Material(shader);
                if (mat.HasProperty("_BaseColor")) mat.SetColor("_BaseColor", color);
                mat.color = color;
                renderer.sharedMaterial = mat;
            }

            PrefabUtility.SaveAsPrefabAsset(go, path);
            Object.DestroyImmediate(go);
            Debug.Log($"[Asteria] Created prefab: {path}");
        }
    }
}
#endif
