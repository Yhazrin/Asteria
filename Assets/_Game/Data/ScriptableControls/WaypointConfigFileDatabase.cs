using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Waypoint configuration file database for the game.
    /// Contains all waypoint parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Waypoint Config File Database")]
    public sealed class WaypointConfigFileDatabase : ScriptableObject
    {
        [Header("Waypoints")]
        public float waypointRadius = 5f;
        public int maxWaypoints = 20;
        public Color waypointColor = new(0.4f, 0.8f, 1f);

        [Header("Display")]
        public float waypointSize = 0.5f;
        public float waypointVisibleDistance = 100f;
    }
}
