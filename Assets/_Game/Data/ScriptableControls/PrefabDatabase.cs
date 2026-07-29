using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Prefab configuration database for the game.
    /// Contains all prefab references.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Prefab Database")]
    public sealed class PrefabDatabase : ScriptableObject
    {
        [Header("Characters")]
        public GameObject residentPrefab;
        public GameObject creaturePrefab;
        public GameObject playerPrefab;

        [Header("Environment")]
        public GameObject treePrefab;
        public GameObject rockPrefab;
        public GameObject crystalPrefab;
        public GameObject flowerPrefab;

        [Header("Structures")]
        public GameObject beaconPrefab;
        public GameObject facilityPrefab;
        public GameObject observatoryPrefab;

        [Header("Tools")]
        public GameObject toolPrefab;
        public GameObject projectilePrefab;

        [Header("UI")]
        public GameObject uiCanvasPrefab;
        public GameObject popupPrefab;
        public GameObject tooltipPrefab;
    }
}
