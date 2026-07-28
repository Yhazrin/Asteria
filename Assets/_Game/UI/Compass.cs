using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Asteria.UI
{
    /// <summary>
    /// Spherical compass that shows direction to points of interest.
    /// Works on the curved surface of the planet.
    /// </summary>
    public sealed class Compass : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float compassRadius = 80f;
        [SerializeField] float markerSize = 20f;
        [SerializeField] float updateInterval = 0.2f;

        [Header("References")]
        [SerializeField] Transform player;
        [SerializeField] Planet.PlanetBody planet;

        readonly Dictionary<string, CompassMarker> _markers = new();
        GameObject _compassRoot;
        float _updateTimer;

        void Start()
        {
            BuildCompassUI();
        }

        void Update()
        {
            _updateTimer -= Time.deltaTime;
            if (_updateTimer <= 0f)
            {
                _updateTimer = updateInterval;
                UpdateMarkers();
            }
        }

        void BuildCompassUI()
        {
            var canvas = FindFirstObjectByType<Canvas>();
            if (canvas == null) return;

            _compassRoot = new GameObject("Compass");
            _compassRoot.transform.SetParent(canvas.transform, false);

            var rect = _compassRoot.AddComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.5f, 1);
            rect.anchorMax = new Vector2(0.5f, 1);
            rect.pivot = new Vector2(0.5f, 1);
            rect.anchoredPosition = new Vector2(0, -20);
            rect.sizeDelta = new Vector2(compassRadius * 2, 40);

            // Background
            var bg = _compassRoot.AddComponent<Image>();
            bg.color = new Color(0, 0, 0, 0.3f);
        }

        /// <summary>
        /// Register a point of interest on the compass.
        /// </summary>
        public void RegisterMarker(string id, Transform target, Color color, string label = "")
        {
            if (_markers.ContainsKey(id)) return;

            var markerGo = new GameObject($"Marker_{id}");
            markerGo.transform.SetParent(_compassRoot.transform, false);

            var markerRect = markerGo.AddComponent<RectTransform>();
            markerRect.sizeDelta = new Vector2(markerSize, markerSize);

            var img = markerGo.AddComponent<Image>();
            img.color = color;

            var marker = new CompassMarker
            {
                root = markerGo,
                target = target,
                rect = markerRect,
                image = img
            };

            _markers[id] = marker;
        }

        /// <summary>
        /// Remove a marker from the compass.
        /// </summary>
        public void RemoveMarker(string id)
        {
            if (_markers.TryGetValue(id, out var marker))
            {
                if (marker.root != null) Destroy(marker.root);
                _markers.Remove(id);
            }
        }

        void UpdateMarkers()
        {
            if (player == null || planet == null) return;

            Vector3 playerUp = planet.GetSurfaceUp(player.position);
            Vector3 playerForward = Vector3.ProjectOnPlane(player.forward, playerUp).normalized;

            foreach (var kvp in _markers)
            {
                var marker = kvp.Value;
                if (marker.target == null || marker.rect == null) continue;

                // Calculate direction on sphere surface
                Vector3 toTarget = marker.target.position - player.position;
                Vector3 surfaceDir = Vector3.ProjectOnPlane(toTarget, playerUp).normalized;

                // Calculate angle relative to player forward
                float angle = Vector3.SignedAngle(playerForward, surfaceDir, playerUp);

                // Map to compass position
                float normalizedAngle = angle / 180f; // -1 to 1
                float xPos = normalizedAngle * compassRadius;

                marker.rect.anchoredPosition = new Vector2(xPos, 0);

                // Fade out markers at edges
                float edgeFade = 1f - Mathf.Abs(normalizedAngle);
                var color = marker.image.color;
                color.a = Mathf.Lerp(0.2f, 1f, edgeFade);
                marker.image.color = color;
            }
        }

        class CompassMarker
        {
            public GameObject root;
            public Transform target;
            public RectTransform rect;
            public Image image;
        }
    }
}
