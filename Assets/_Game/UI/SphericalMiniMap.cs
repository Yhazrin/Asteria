using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Asteria.UI
{
    /// <summary>
    /// Spherical mini-map showing the planet from above with player and POI markers.
    /// Renders a top-down view of the planet surface.
    /// </summary>
    public sealed class SphericalMiniMap : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float mapSize = 150f;
        [SerializeField] float updateInterval = 0.1f;
        [SerializeField] int textureResolution = 256;

        [Header("References")]
        [SerializeField] Transform player;
        [SerializeField] Planet.PlanetBody planet;
        [SerializeField] Camera miniMapCamera;

        readonly Dictionary<string, MiniMapMarker> _markers = new();
        GameObject _mapRoot;
        RawImage _mapImage;
        RenderTexture _renderTexture;
        float _updateTimer;

        void Start()
        {
            BuildMiniMapUI();
            SetupMiniMapCamera();
        }

        void Update()
        {
            _updateTimer -= Time.deltaTime;
            if (_updateTimer <= 0f)
            {
                _updateTimer = updateInterval;
                UpdateMiniMap();
            }
        }

        void BuildMiniMapUI()
        {
            var canvas = FindFirstObjectByType<Canvas>();
            if (canvas == null) return;

            _mapRoot = new GameObject("MiniMap");
            _mapRoot.transform.SetParent(canvas.transform, false);

            var rect = _mapRoot.AddComponent<RectTransform>();
            rect.anchorMin = new Vector2(1, 0);
            rect.anchorMax = new Vector2(1, 0);
            rect.pivot = new Vector2(1, 0);
            rect.anchoredPosition = new Vector2(-10, 10);
            rect.sizeDelta = new Vector2(mapSize, mapSize);

            // Background circle
            var bg = _mapRoot.AddComponent<Image>();
            bg.color = new Color(0.1f, 0.15f, 0.2f, 0.8f);

            // Map image
            var mapImageGo = new GameObject("MapImage");
            mapImageGo.transform.SetParent(_mapRoot.transform, false);

            var mapRect = mapImageGo.AddComponent<RectTransform>();
            mapRect.anchorMin = Vector2.zero;
            mapRect.anchorMax = Vector2.one;
            mapRect.offsetMin = new Vector2(5, 5);
            mapRect.offsetMax = new Vector2(-5, -5);

            _mapImage = mapImageGo.AddComponent<RawImage>();

            // Create render texture
            _renderTexture = new RenderTexture(textureResolution, textureResolution, 16);
            _mapImage.texture = _renderTexture;
        }

        void SetupMiniMapCamera()
        {
            if (miniMapCamera == null)
            {
                var camGo = new GameObject("MiniMapCamera");
                miniMapCamera = camGo.AddComponent<Camera>();
                miniMapCamera.orthographic = true;
                miniMapCamera.orthographicSize = planet != null ? planet.Radius * 1.2f : 200f;
                miniMapCamera.cullingMask = LayerMask.GetMask("Default");
                miniMapCamera.clearFlags = CameraClearFlags.SolidColor;
                miniMapCamera.backgroundColor = new Color(0.1f, 0.15f, 0.2f);
                miniMapCamera.targetTexture = _renderTexture;
                miniMapCamera.depth = -1; // Render before main camera
            }
        }

        void UpdateMiniMap()
        {
            if (miniMapCamera == null || player == null || planet == null) return;

            // Position camera above player looking down
            Vector3 playerUp = planet.GetSurfaceUp(player.position);
            Vector3 cameraPos = player.position + playerUp * planet.Radius * 2f;
            miniMapCamera.transform.position = cameraPos;
            miniMapCamera.transform.LookAt(planet.Center);

            // Update marker positions
            foreach (var kvp in _markers)
            {
                var marker = kvp.Value;
                if (marker.target == null || marker.image == null) continue;

                // Project target position onto map
                Vector3 toTarget = marker.target.position - planet.Center;
                Vector3 playerToTarget = toTarget - (player.position - planet.Center);

                // Convert to 2D map coordinates (simplified projection)
                Vector3 right = Vector3.Cross(playerUp, player.forward).normalized;
                Vector3 forward = Vector3.Cross(right, playerUp).normalized;

                float x = Vector3.Dot(playerToTarget, right);
                float y = Vector3.Dot(playerToTarget, forward);

                // Normalize to map size
                float mapRadius = mapSize / 2f - 10f;
                float planetRadius = planet.Radius;
                float normalizedX = (x / planetRadius) * mapRadius;
                float normalizedY = (y / planetRadius) * mapRadius;

                // Clamp to map bounds
                Vector2 mapPos = new Vector2(normalizedX, normalizedY);
                if (mapPos.magnitude > mapRadius)
                {
                    mapPos = mapPos.normalized * mapRadius;
                }

                marker.rect.anchoredPosition = mapPos;
            }
        }

        /// <summary>
        /// Register a marker on the mini-map.
        /// </summary>
        public void RegisterMarker(string id, Transform target, Color color, float size = 8f)
        {
            if (_markers.ContainsKey(id)) return;

            var markerGo = new GameObject($"MapMarker_{id}");
            markerGo.transform.SetParent(_mapRoot.transform, false);

            var markerRect = markerGo.AddComponent<RectTransform>();
            markerRect.sizeDelta = new Vector2(size, size);

            var img = markerGo.AddComponent<Image>();
            img.color = color;

            _markers[id] = new MiniMapMarker
            {
                root = markerGo,
                target = target,
                rect = markerRect,
                image = img
            };
        }

        /// <summary>
        /// Remove a marker from the mini-map.
        /// </summary>
        public void RemoveMarker(string id)
        {
            if (_markers.TryGetValue(id, out var marker))
            {
                if (marker.root != null) Destroy(marker.root);
                _markers.Remove(id);
            }
        }

        void OnDestroy()
        {
            if (_renderTexture != null)
            {
                _renderTexture.Release();
            }
        }

        class MiniMapMarker
        {
            public GameObject root;
            public Transform target;
            public RectTransform rect;
            public Image image;
        }
    }
}
