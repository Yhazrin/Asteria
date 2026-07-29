using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Pathfinding configuration file database for the game.
    /// Contains all pathfinding parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Pathfinding Config File Database")]
    public sealed class PathfindingConfigFileDatabase : ScriptableObject
    {
        [Header("Pathfinding")]
        public int maxIterations = 100;
        public float nodeSize = 1f;
        public float heuristicWeight = 1f;

        [Header("Spherical")]
        public bool useSphericalPathfinding = true;
        public float greatCircleAccuracy = 0.01f;

        [Header("Optimization")]
        public bool smoothPath = true;
        public int maxPathPoints = 50;
    }
}
