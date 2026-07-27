using Asteria.Planet;
using UnityEngine;

namespace Asteria.Player
{
    /// <summary>
    /// Builds a playable spherical demo at runtime if the scene is empty / incomplete.
    /// Used as a safety net when the Editor bootstrap has not been run yet.
    /// </summary>
    public sealed class SphereMoveDemoBuilder : MonoBehaviour
    {
        [SerializeField] float planetRadius = 300f;
        [SerializeField] bool buildOnAwake = true;

        void Awake()
        {
            if (buildOnAwake && FindFirstObjectByType<PlanetBody>() == null)
            {
                Build();
            }
        }

        [ContextMenu("Build Demo Now")]
        public void Build()
        {
            // Lighting
            if (FindFirstObjectByType<Light>() == null)
            {
                GameObject lightGo = new GameObject("Directional Light");
                Light light = lightGo.AddComponent<Light>();
                light.type = LightType.Directional;
                light.intensity = 1.15f;
                light.color = new Color(1f, 0.97f, 0.92f);
                light.shadows = LightShadows.Soft;
                lightGo.transform.rotation = Quaternion.Euler(45f, -30f, 0f);
            }

            RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Trilight;
            RenderSettings.ambientSkyColor = new Color(0.55f, 0.7f, 0.9f);
            RenderSettings.ambientEquatorColor = new Color(0.45f, 0.5f, 0.45f);
            RenderSettings.ambientGroundColor = new Color(0.2f, 0.18f, 0.15f);
            RenderSettings.fog = true;
            RenderSettings.fogMode = FogMode.ExponentialSquared;
            RenderSettings.fogDensity = 0.0008f;
            RenderSettings.fogColor = new Color(0.55f, 0.68f, 0.82f);

            Material planetMat = CreateColorMaterial(new Color(0.45f, 0.62f, 0.48f));
            Material equatorMat = CreateColorMaterial(new Color(0.85f, 0.72f, 0.35f));
            Material northMat = CreateColorMaterial(new Color(0.55f, 0.75f, 0.95f));
            Material southMat = CreateColorMaterial(new Color(0.95f, 0.55f, 0.45f));
            Material playerMat = CreateColorMaterial(new Color(0.95f, 0.92f, 0.88f));

            GameObject planetGo = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            planetGo.name = "Planet";
            planetGo.transform.position = Vector3.zero;
            planetGo.transform.localScale = Vector3.one * (planetRadius / 0.5f);
            planetGo.GetComponent<MeshRenderer>().sharedMaterial = planetMat;

            PlanetBody planet = planetGo.AddComponent<PlanetBody>();
            planet.Configure(planetRadius, 9.81f);

            CreateLandmark("EquatorMarker_A", planet, Vector3.forward, equatorMat, new Vector3(8f, 16f, 8f));
            CreateLandmark("EquatorMarker_B", planet, -Vector3.forward, equatorMat, new Vector3(8f, 16f, 8f));
            CreateLandmark("NorthPole", planet, Vector3.up, northMat, new Vector3(12f, 24f, 12f));
            CreateLandmark("SouthPole", planet, Vector3.down, southMat, new Vector3(12f, 24f, 12f));

            GameObject player = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            player.name = "Player";
            Destroy(player.GetComponent<CapsuleCollider>());
            CapsuleCollider col = player.AddComponent<CapsuleCollider>();
            col.height = 2f;
            col.radius = 0.4f;
            player.GetComponent<MeshRenderer>().sharedMaterial = playerMat;

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
            gravity.Planet = planet;

            SphericalMotor motor = player.AddComponent<SphericalMotor>();

            Camera existingCam = Camera.main;
            GameObject camGo = existingCam != null ? existingCam.gameObject : new GameObject("Main Camera");
            if (existingCam == null)
            {
                camGo.tag = "MainCamera";
                Camera cam = camGo.AddComponent<Camera>();
                cam.nearClipPlane = 0.1f;
                cam.farClipPlane = 2500f;
                camGo.AddComponent<AudioListener>();
            }
            else
            {
                existingCam.nearClipPlane = 0.1f;
                existingCam.farClipPlane = 2500f;
            }

            SphericalThirdPersonCamera orbit = camGo.GetComponent<SphericalThirdPersonCamera>();
            if (orbit == null)
            {
                orbit = camGo.AddComponent<SphericalThirdPersonCamera>();
            }

            orbit.Target = player.transform;
            orbit.Planet = planet;
            motor.SetCamera(camGo.transform);

            Vector3 up = planet.GetSurfaceUp(player.transform.position);
            camGo.transform.position = player.transform.position + up * 2f - player.transform.forward * 7f;
            camGo.transform.LookAt(player.transform.position + up * 1.4f, up);

            if (FindFirstObjectByType<SphereMoveDemoHud>() == null)
            {
                new GameObject("DemoHUD").AddComponent<SphereMoveDemoHud>();
            }

            Debug.Log("[Asteria] SphereMoveDemo built at runtime.");
        }

        static void CreateLandmark(
            string name,
            PlanetBody planet,
            Vector3 direction,
            Material material,
            Vector3 scale)
        {
            GameObject marker = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            marker.name = name;
            marker.transform.position = planet.GetPointOnSurface(direction, 2f);
            marker.transform.localScale = scale;
            marker.transform.up = direction.normalized;
            marker.GetComponent<MeshRenderer>().sharedMaterial = material;
            marker.GetComponent<Collider>().isTrigger = true;
        }

        static Material CreateColorMaterial(Color color)
        {
            Shader shader = Shader.Find("Universal Render Pipeline/Lit");
            if (shader == null)
            {
                shader = Shader.Find("Universal Render Pipeline/Simple Lit");
            }

            if (shader == null)
            {
                shader = Shader.Find("Sprites/Default");
            }

            if (shader == null)
            {
                shader = Shader.Find("Standard");
            }

            Material mat = new Material(shader);
            if (mat.HasProperty("_BaseColor"))
            {
                mat.SetColor("_BaseColor", color);
            }

            mat.color = color;
            return mat;
        }
    }
}
