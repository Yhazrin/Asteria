using UnityEngine;

namespace Asteria.Expedition
{
    /// <summary>
    /// A temporary tool placed during an expedition (beacon, warm light, etc.).
    /// Respects TraceLimitsConfig for count limits.
    /// </summary>
    public sealed class PlacedTool : MonoBehaviour
    {
        [SerializeField] string toolId = "beacon";
        [SerializeField] string displayName = "信标";
        [SerializeField] float lifetime = 600f;

        float _timer;
        string _placedBy;

        public string ToolId => toolId;
        public string DisplayName => displayName;
        public string PlacedBy => _placedBy;

        public void Initialize(string placerId)
        {
            _placedBy = placerId;
            _timer = lifetime;
        }

        void Update()
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0f)
            {
                Debug.Log($"[Asteria] {displayName} expired.");
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// Get the effect radius of this tool.
        /// </summary>
        public float GetEffectRadius()
        {
            return toolId switch
            {
                "warm_light" => 12f,
                "beacon" => 8f,
                "rope_point" => 5f,
                _ => 5f
            };
        }

        /// <summary>
        /// Check if this tool counteracts a given pressure state.
        /// </summary>
        public bool Counteracts(string pressureState)
        {
            return (toolId, pressureState) switch
            {
                ("warm_light", "cold") => true,
                ("beacon", _) => true, // beacons help with all pressures (safe zone)
                _ => false
            };
        }
    }
}
