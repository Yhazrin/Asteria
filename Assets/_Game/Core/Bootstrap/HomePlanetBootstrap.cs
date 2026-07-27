using Asteria.Interaction;
using Asteria.Persistence;
using Asteria.Planet;
using UnityEngine;

namespace Asteria.Core
{
    /// <summary>
    /// Sets up the home planet scene. Creates the planet, observatory anchor,
    /// and expedition departure beacon.
    /// </summary>
    public sealed class HomePlanetBootstrap : MonoBehaviour
    {
        [SerializeField] float planetRadius = 180f;

        void Start()
        {
            if (FindFirstObjectByType<PlanetBody>() != null)
            {
                // Already set up
                return;
            }

            BuildHomePlanet();
        }

        void BuildHomePlanet()
        {
            // Create planet
            GameObject planetGo = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            planetGo.name = "HomePlanet";
            planetGo.transform.position = Vector3.zero;
            float scale = planetRadius / 0.5f;
            planetGo.transform.localScale = Vector3.one * scale;

            PlanetBody planet = planetGo.AddComponent<PlanetBody>();
            planet.Configure(planetRadius, 9.81f);

            // Create observatory anchor
            CreateObservatory(planet);

            // Create expedition departure beacon
            CreateDepartureBeacon(planet);

            // Create a simple player if not present
            if (FindFirstObjectByType<Player.SphericalGravityBody>() == null)
            {
                CreateHomePlayer(planet);
            }

            Debug.Log("[Asteria] Home planet built.");
        }

        void CreateObservatory(PlanetBody planet)
        {
            Vector3 dir = Vector3.up;
            Vector3 pos = planet.GetPointOnSurface(dir, 2f);

            GameObject obs = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            obs.name = "Observatory";
            obs.transform.position = pos;
            obs.transform.localScale = new Vector3(6f, 12f, 6f);
            obs.transform.up = dir;

            var renderer = obs.GetComponent<MeshRenderer>();
            Shader shader = Shader.Find("Universal Render Pipeline/Lit");
            if (shader == null) shader = Shader.Find("Sprites/Default");
            Material mat = new(shader);
            Color c = new(0.75f, 0.85f, 0.95f);
            if (mat.HasProperty("_BaseColor")) mat.SetColor("_BaseColor", c);
            mat.color = c;
            renderer.sharedMaterial = mat;

            obs.GetComponent<Collider>().isTrigger = true;

            // Add an ObserveInteractable that shows discoveries
            var entry = ScriptableObject.CreateInstance<Data.ObserveEntry>();
            entry.id = "home.observatory";
            entry.displayName = "观测台";
            entry.description = "家园的观测台。展示从远征带回的发现。";
            entry.promptText = "按 E 查看观测台";

            var observe = obs.AddComponent<ObserveInteractable>();
            observe.Entry = entry;

            // Add trigger for interaction detection
            var triggerGo = new GameObject("Trigger");
            triggerGo.transform.SetParent(obs.transform, false);
            var trigger = triggerGo.AddComponent<SphereCollider>();
            trigger.isTrigger = true;
            trigger.radius = 1.5f;
        }

        void CreateDepartureBeacon(PlanetBody planet)
        {
            Vector3 dir = (Vector3.forward + Vector3.right * 0.5f).normalized;
            Vector3 pos = planet.GetPointOnSurface(dir, 2f);

            GameObject beacon = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            beacon.name = "DepartureBeacon";
            beacon.transform.position = pos;
            beacon.transform.localScale = new Vector3(3f, 8f, 3f);
            beacon.transform.up = dir;

            var renderer = beacon.GetComponent<MeshRenderer>();
            Shader shader = Shader.Find("Universal Render Pipeline/Lit");
            if (shader == null) shader = Shader.Find("Sprites/Default");
            Material mat = new(shader);
            Color c = new(0.95f, 0.82f, 0.42f);
            if (mat.HasProperty("_BaseColor")) mat.SetColor("_BaseColor", c);
            mat.color = c;
            renderer.sharedMaterial = mat;

            beacon.GetComponent<Collider>().isTrigger = true;

            // Add an interactable that starts expedition
            var entry = ScriptableObject.CreateInstance<Data.ObserveEntry>();
            entry.id = "home.departure_beacon";
            entry.displayName = "远征信标";
            entry.description = "从这里出发前往远征星球。";
            entry.promptText = "按 E 出发远征";

            var interactable = beacon.AddComponent<DepartureBeaconInteractable>();
        }

        void CreateHomePlayer(PlanetBody planet)
        {
            GameObject player = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            player.name = "Player";
            Destroy(player.GetComponent<CapsuleCollider>());
            CapsuleCollider col = player.AddComponent<CapsuleCollider>();
            col.height = 2f;
            col.radius = 0.4f;

            Vector3 spawnDir = (Vector3.forward + Vector3.up * 0.3f).normalized;
            player.transform.position = planet.GetPointOnSurface(spawnDir, 1.05f);
            planet.AlignTransformToSurface(player.transform, Vector3.Cross(spawnDir, Vector3.right));

            Rigidbody rb = player.AddComponent<Rigidbody>();
            rb.mass = 80f;
            rb.useGravity = false;
            rb.interpolation = RigidbodyInterpolation.Interpolate;
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            rb.constraints = RigidbodyConstraints.FreezeRotation;

            var gravity = player.AddComponent<Player.SphericalGravityBody>();
            gravity.Planet = planet;

            var motor = player.AddComponent<Player.SphericalMotor>();

            // Input adapter
            var inputAdapter = player.AddComponent<Player.LegacyInputAdapter>();

            // Camera
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

            var orbit = camGo.GetComponent<Player.SphericalThirdPersonCamera>();
            if (orbit == null)
            {
                orbit = camGo.AddComponent<Player.SphericalThirdPersonCamera>();
            }

            orbit.Target = player.transform;
            orbit.Planet = planet;
            motor.SetCamera(camGo.transform);

            // Interaction detector
            player.AddComponent<InteractionDetector>();
        }
    }
}
