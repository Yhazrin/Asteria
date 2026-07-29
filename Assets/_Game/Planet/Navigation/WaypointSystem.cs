using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Planet.Navigation
{
    /// <summary>
    /// Waypoint system for navigation on the spherical planet.
    /// Supports pathfinding, markers, and route visualization.
    /// </summary>
    public sealed class WaypointSystem : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float waypointRadius = 5f;
        [SerializeField] int maxWaypoints = 20;
        [SerializeField] Color waypointColor = new(0.4f, 0.8f, 1f);

        readonly Dictionary<string, Waypoint> _waypoints = new();
        readonly List<string> _activeRoute = new();

        /// <summary>
        /// Create a waypoint at a position.
        /// </summary>
        public Waypoint CreateWaypoint(string id, Vector3 position, string label = "")
        {
            if (_waypoints.Count >= maxWaypoints) return null;

            var waypoint = new Waypoint
            {
                id = id,
                position = position,
                label = label,
                isActive = true
            };

            _waypoints[id] = waypoint;
            CreateWaypointVisual(waypoint);

            return waypoint;
        }

        /// <summary>
        /// Remove a waypoint.
        /// </summary>
        public void RemoveWaypoint(string id)
        {
            if (_waypoints.TryGetValue(id, out var waypoint))
            {
                if (waypoint.visual != null)
                    Destroy(waypoint.visual);
                _waypoints.Remove(id);
            }
        }

        /// <summary>
        /// Find the shortest path between two points on the sphere.
        /// Uses great circle distance.
        /// </summary>
        public List<string> FindPath(Vector3 start, Vector3 end, PlanetBody planet)
        {
            var path = new List<string>();

            // Simple direct path (great circle)
            float distance = Vector3.Distance(start, end);

            // Add intermediate waypoints if distance is large
            if (distance > planet.Radius * 0.5f)
            {
                // Add midpoint on sphere
                Vector3 mid = ((start + end) * 0.5f).normalized * planet.Radius;
                string midId = $"path_mid_{path.Count}";
                CreateWaypoint(midId, mid, "中继点");
                path.Add(midId);
            }

            return path;
        }

        /// <summary>
        /// Get the nearest waypoint to a position.
        /// </summary>
        public Waypoint GetNearestWaypoint(Vector3 position)
        {
            Waypoint nearest = null;
            float minDist = float.MaxValue;

            foreach (var kvp in _waypoints)
            {
                if (!kvp.Value.isActive) continue;

                float dist = Vector3.Distance(position, kvp.Value.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    nearest = kvp.Value;
                }
            }

            return nearest;
        }

        /// <summary>
        /// Get all waypoints within radius.
        /// </summary>
        public List<Waypoint> GetWaypointsInRadius(Vector3 position, float radius)
        {
            var result = new List<Waypoint>();

            foreach (var kvp in _waypoints)
            {
                if (!kvp.Value.isActive) continue;

                float dist = Vector3.Distance(position, kvp.Value.position);
                if (dist <= radius)
                {
                    result.Add(kvp.Value);
                }
            }

            return result;
        }

        /// <summary>
        /// Set an active route between waypoints.
        /// </summary>
        public void SetRoute(List<string> waypointIds)
        {
            _activeRoute.Clear();
            _activeRoute.AddRange(waypointIds);
        }

        /// <summary>
        /// Clear the active route.
        /// </summary>
        public void ClearRoute()
        {
            _activeRoute.Clear();
        }

        void CreateWaypointVisual(Waypoint waypoint)
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            go.name = $"Waypoint_{waypoint.id}";
            go.transform.position = waypoint.position;
            go.transform.localScale = Vector3.one * 0.5f;

            var renderer = go.GetComponent<MeshRenderer>();
            var mat = new Material(Shader.Find("Universal Render Pipeline/Lit")
                ?? Shader.Find("Sprites/Default"));
            if (mat.HasProperty("_BaseColor")) mat.SetColor("_BaseColor", waypointColor);
            mat.color = waypointColor;
            renderer.material = mat;

            // Remove collider
            var col = go.GetComponent<Collider>();
            if (col != null) Destroy(col);

            waypoint.visual = go;
        }

        void OnDestroy()
        {
            foreach (var kvp in _waypoints)
            {
                if (kvp.Value.visual != null)
                    Destroy(kvp.Value.visual);
            }
        }

        [System.Serializable]
        public class Waypoint
        {
            public string id;
            public Vector3 position;
            public string label;
            public bool isActive;
            public GameObject visual;
        }
    }
}
