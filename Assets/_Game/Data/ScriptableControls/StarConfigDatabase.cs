using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Star configuration database for the game.
    /// Contains all star parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Star Config Database")]
    public sealed class StarConfigDatabase : ScriptableObject
    {
        [Header("Stars")]
        public int starCount = 500;
        public float sphereRadius = 1000f;
        public Color starColor = new(1f, 0.98f, 0.9f);

        [Header("Appearance")]
        public float minSize = 0.5f;
        public float maxSize = 2f;
        public float minBrightness = 0.3f;
        public float maxBrightness = 1f;

        [Header("Animation")]
        public float twinkleSpeed = 2f;
        public float rotationSpeed = 0.01f;
    }
}
