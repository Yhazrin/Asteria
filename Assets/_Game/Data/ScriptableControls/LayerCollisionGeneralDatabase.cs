using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// General layer collision database for the game.
    /// Contains all layer collision matrix settings.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Layer Collision General Database")]
    public sealed class LayerCollisionGeneralDatabase : ScriptableObject
    {
        [Header("Collision Matrix")]
        public int defaultCollisionMatrix = -1;

        [Header("Layer Names")]
        public string layerDefault = "Default";
        public string layerPlanet = "Planet";
        public string layerPlayer = "Player";
        public string layerResident = "Resident";
        public string layerCreature = "Creature";
        public string layerPOI = "POI";
        public string layerTool = "Tool";
        public string layerUI = "UI";

        [Header("Collision Rules")]
        public bool playerCollidesWithPlanet = true;
        public bool playerCollidesWithResident = true;
        public bool playerCollidesWithCreature = true;
        public bool playerCollidesWithPOI = true;
        public bool residentCollidesWithPlanet = true;
        public bool creatureCollidesWithPlanet = true;
    }
}
