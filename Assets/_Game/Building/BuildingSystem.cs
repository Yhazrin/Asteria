using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Asteria.Building
{
    /// <summary>
    /// Manages the building system on the home planet.
    /// Handles anchor registration, facility placement, and save/load.
    /// </summary>
    public sealed class BuildingSystem : MonoBehaviour
    {
        [SerializeField] FacilityDefinition[] availableFacilities;

        readonly Dictionary<string, BuildAnchor> _anchors = new();
        readonly List<FacilityState> _installedFacilities = new();

        public IReadOnlyList<FacilityState> InstalledFacilities => _installedFacilities;
        public FacilityDefinition[] AvailableFacilities => availableFacilities;

        void Awake()
        {
            // Register all anchors in the scene
            var anchors = FindObjectsByType<BuildAnchor>(FindObjectsSortMode.None);
            foreach (var anchor in anchors)
            {
                RegisterAnchor(anchor);
            }
        }

        public void RegisterAnchor(BuildAnchor anchor)
        {
            if (anchor != null && !_anchors.ContainsKey(anchor.AnchorId))
            {
                _anchors[anchor.AnchorId] = anchor;
            }
        }

        /// <summary>
        /// Get all empty anchors of a given size.
        /// </summary>
        public IEnumerable<BuildAnchor> GetEmptyAnchors(AnchorSize? size = null)
        {
            return _anchors.Values.Where(a => a.IsEmpty && (size == null || a.Size == size));
        }

        /// <summary>
        /// Try to build a facility at a specific anchor.
        /// </summary>
        public bool TryBuild(string facilityId, string anchorId, float rotation = 0f)
        {
            if (!_anchors.TryGetValue(anchorId, out var anchor))
            {
                Debug.LogWarning($"[Asteria] Anchor {anchorId} not found.");
                return false;
            }

            if (availableFacilities == null)
            {
                return false;
            }

            var definition = availableFacilities.FirstOrDefault(f => f.facilityId == facilityId);
            if (definition == null)
            {
                Debug.LogWarning($"[Asteria] Facility definition {facilityId} not found.");
                return false;
            }

            var state = new FacilityState(definition);
            state.SetRotation(rotation);

            if (anchor.TryInstall(state))
            {
                _installedFacilities.Add(state);
                CreateFacilityVisual(state, anchor);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Remove a facility from an anchor.
        /// </summary>
        public void Demolish(string anchorId)
        {
            if (!_anchors.TryGetValue(anchorId, out var anchor))
            {
                return;
            }

            var facility = anchor.InstalledFacility;
            if (facility != null)
            {
                anchor.RemoveFacility();
                _installedFacilities.Remove(facility);
                Debug.Log($"[Asteria] Demolished facility at anchor {anchorId}.");
            }
        }

        void CreateFacilityVisual(FacilityState state, BuildAnchor anchor)
        {
            // Create a visual representation of the facility
            var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            go.name = $"Facility_{state.DisplayName}";
            go.transform.SetParent(anchor.transform, false);
            go.transform.localPosition = Vector3.up * 2f;
            go.transform.localScale = new Vector3(3f, 2f, 3f);
            go.transform.localRotation = Quaternion.Euler(0f, state.RotationAngle, 0f);

            var renderer = go.GetComponent<MeshRenderer>();
            Shader shader = Shader.Find("Universal Render Pipeline/Lit");
            if (shader == null) shader = Shader.Find("Sprites/Default");
            Material mat = new(shader);
            Color c = new(0.75f, 0.7f, 0.65f);
            if (mat.HasProperty("_BaseColor")) mat.SetColor("_BaseColor", c);
            mat.color = c;
            renderer.sharedMaterial = mat;
        }
    }
}
