using Asteria.Data;
using Asteria.Interaction;
using Asteria.Planet;
using Asteria.Player;
using UnityEngine;

namespace Asteria.Player
{
    /// <summary>
    /// Ensures Phase 1 Observe content exists at runtime if the scene was not upgraded yet.
    /// Safe to keep in the demo scene: it no-ops when content already exists.
    /// </summary>
    public sealed class ObserveDemoBootstrap : MonoBehaviour
    {
        [SerializeField] ObserveEntry fallbackEntry;
        [SerializeField] PlayerMotorConfig motorConfig;
        [SerializeField] bool buildIfMissing = true;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void AutoEnsure()
        {
            if (FindFirstObjectByType<PlanetBody>() == null)
            {
                return;
            }

            if (FindFirstObjectByType<ObserveDemoBootstrap>() != null)
            {
                return;
            }

            var go = new GameObject("ObserveDemoBootstrap");
            go.AddComponent<ObserveDemoBootstrap>().EnsureObserveSlice();
        }

        void Awake()
        {
            if (buildIfMissing)
            {
                EnsureObserveSlice();
            }
        }

        public void EnsureObserveSlice()
        {
            _ = DiscoveryJournal.Instance;
            _ = GameHud.Instance;

            SphereMoveDemoHud legacyHud = FindFirstObjectByType<SphereMoveDemoHud>();
            if (legacyHud != null)
            {
                legacyHud.enabled = false;
            }

            SphericalMotor motor = FindFirstObjectByType<SphericalMotor>();
            if (motor != null)
            {
                if (motorConfig != null)
                {
                    motor.SetConfig(motorConfig);
                }

                if (motor.GetComponent<InteractionDetector>() == null)
                {
                    motor.gameObject.AddComponent<InteractionDetector>();
                }
            }

            if (FindFirstObjectByType<ObserveInteractable>() != null)
            {
                return;
            }

            PlanetBody planet = FindFirstObjectByType<PlanetBody>();
            if (planet == null)
            {
                return;
            }

            ObserveEntry entry = fallbackEntry;
            if (entry == null)
            {
                entry = ScriptableObject.CreateInstance<ObserveEntry>();
                entry.id = "wind_bell_stone";
                entry.displayName = "风铃石";
                entry.description = "一块被风长期打磨的石头。靠近时能听见很轻的金属颤音。";
                entry.promptText = "按 E 观察 · 风铃石";
            }

            Vector3 dir = (Vector3.forward + Vector3.right * 0.35f).normalized;
            GameObject poi = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            poi.name = "POI_WindBellStone";
            poi.transform.position = planet.GetPointOnSurface(dir, 2.2f);
            poi.transform.localScale = Vector3.one * 4f;
            poi.transform.up = dir;

            var renderer = poi.GetComponent<MeshRenderer>();
            renderer.sharedMaterial = CreatePoiMaterial();

            SphereCollider col = poi.GetComponent<SphereCollider>();
            col.isTrigger = false;
            col.radius = 0.5f;

            // Larger trigger volume for interaction detection.
            var triggerGo = new GameObject("InteractTrigger");
            triggerGo.transform.SetParent(poi.transform, false);
            triggerGo.transform.localPosition = Vector3.zero;
            SphereCollider trigger = triggerGo.AddComponent<SphereCollider>();
            trigger.isTrigger = true;
            trigger.radius = 1.2f;

            ObserveInteractable observe = poi.AddComponent<ObserveInteractable>();
            observe.Entry = entry;

            Debug.Log("[Asteria] Observe slice ensured (runtime). Walk toward the bright stone and press E.");
        }

        static Material CreatePoiMaterial()
        {
            Shader shader = Shader.Find("Universal Render Pipeline/Lit");
            if (shader == null)
            {
                shader = Shader.Find("Sprites/Default");
            }

            var mat = new Material(shader);
            Color c = new Color(0.95f, 0.82f, 0.42f);
            if (mat.HasProperty("_BaseColor"))
            {
                mat.SetColor("_BaseColor", c);
            }

            mat.color = c;
            return mat;
        }
    }
}
