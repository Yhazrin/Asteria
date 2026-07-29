using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// General navigation configuration database for the game.
    /// Contains all navigation parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Navigation Config General Database")]
    public sealed class NavigationConfigGeneralDatabase : ScriptableObject
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
