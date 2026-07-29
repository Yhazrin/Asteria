using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// General waypoint configuration database for the game.
    /// Contains all waypoint parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Waypoint Config General Database")]
    public sealed class WaypointConfigGeneralDatabase : ScriptableObject
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
