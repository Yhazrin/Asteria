using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Navigation configuration file database for the game.
    /// Contains all navigation parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Navigation Config File Database")]
    public sealed class NavigationConfigFileDatabase : ScriptableObject
    {
        [Header("Navigation")]
        public bool useNavMesh = false;
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
