using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Route configuration file database for the game.
    /// Contains all route parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Route Config File Database")]
    public sealed class RouteConfigFileDatabase : ScriptableObject
    {
        [Header("Routes")]
        public int maxRoutePoints = 10;
        public float routeLineWidth = 0.1f;
        public Color routeColor = new(0.4f, 0.8f, 1f, 0.5f);

        [Header("Pathfinding")]
        public float pathNodeRadius = 1f;
        public int maxPathNodes = 50;
    }
}
