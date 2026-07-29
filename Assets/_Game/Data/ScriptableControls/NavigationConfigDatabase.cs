using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Navigation configuration database for the game.
    /// Contains all navigation parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Navigation Config Database")]
    public sealed class NavigationConfigDatabase : ScriptableObject
    {
        [Header("Navigation")]
        public bool useNavMesh = false; // Disabled for spherical planets
        public float navigationRadius = 0.5f;

        [Header("Spherical")]
        public float sphericalNavigationAccuracy = 0.01f;
        public int maxNavigationNodes = 100;

        [Header("Agent")]
        public float agentSpeed = 4f;
        public float agentAngularSpeed = 120f;
        public float agentAcceleration = 8f;
    }
}
