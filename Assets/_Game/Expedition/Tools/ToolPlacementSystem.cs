using System.Collections.Generic;
using System.Linq;
using Asteria.Data;
using Asteria.Planet;
using UnityEngine;

namespace Asteria.Expedition
{
    /// <summary>
    /// Manages placement of temporary tools during an expedition.
    /// Enforces TraceLimitsConfig limits.
    /// </summary>
    public sealed class ToolPlacementSystem : MonoBehaviour
    {
        [SerializeField] TraceLimitsConfig limits;
        [SerializeField] PlanetBody planet;
        [SerializeField] GameObject beaconPrefab;
        [SerializeField] GameObject warmLightPrefab;

        readonly List<PlacedTool> _placedTools = new();

        public int PlacedCount => _placedTools.Count;

        void Start()
        {
            if (planet == null)
            {
                planet = FindFirstObjectByType<PlanetBody>();
            }
        }

        /// <summary>
        /// Place a tool at the player's current position.
        /// </summary>
        public bool TryPlace(string toolId, Vector3 position, string placerId)
        {
            if (!CanPlace(toolId))
            {
                Debug.Log($"[Asteria] Cannot place {toolId}: limit reached.");
                return false;
            }

            // Create tool object
            GameObject prefab = toolId switch
            {
                "beacon" => beaconPrefab,
                "warm_light" => warmLightPrefab,
                _ => null
            };

            GameObject toolGo;
            if (prefab != null)
            {
                toolGo = Instantiate(prefab, position, Quaternion.identity);
            }
            else
            {
                toolGo = CreateDefaultTool(toolId, position);
            }

            var tool = toolGo.GetComponent<PlacedTool>();
            if (tool == null)
            {
                tool = toolGo.AddComponent<PlacedTool>();
            }

            tool.Initialize(placerId);
            _placedTools.Add(tool);

            Debug.Log($"[Asteria] Placed {toolId} at {position}. Total: {_placedTools.Count}");
            return true;
        }

        bool CanPlace(string toolId)
        {
            if (limits == null)
            {
                return true;
            }

            CleanExpiredTools();

            int maxForType = toolId switch
            {
                "beacon" => limits.maxWaymarks,
                "warm_light" => limits.maxCampLights,
                _ => 5
            };

            int currentCount = _placedTools.Count(t => t != null && t.ToolId == toolId);
            return currentCount < maxForType;
        }

        void CleanExpiredTools()
        {
            _placedTools.RemoveAll(t => t == null);
        }

        GameObject CreateDefaultTool(string toolId, Vector3 position)
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            go.name = $"PlacedTool_{toolId}";
            go.transform.position = position;
            go.transform.localScale = new Vector3(0.5f, 2f, 0.5f);
            go.GetComponent<Collider>().isTrigger = true;

            Color c = toolId == "warm_light" ? new Color(1f, 0.9f, 0.6f) : new Color(0.5f, 0.8f, 1f);
            MaterialHelper.ApplyColor(go.GetComponent<MeshRenderer>(), c);

            return go;
        }
    }
}
