using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Mini-map configuration file database for the game.
    /// Contains all mini-map parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/MiniMap Config File Database")]
    public sealed class MiniMapConfigFileDatabase : ScriptableObject
    {
        [Header("MiniMap")]
        public float mapSize = 150f;
        public float updateInterval = 0.1f;
        public int textureResolution = 256;

        [Header("Camera")]
        public float orthographicSize = 200f;

        [Header("Markers")]
        public float markerSize = 8f;
        public Color playerMarkerColor = Color.white;
        public Color poiMarkerColor = new(1f, 0.8f, 0.3f);
    }
}
