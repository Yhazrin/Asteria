using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// General vegetation configuration database for the game.
    /// Contains all vegetation parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Vegetation Config General Database")]
    public sealed class VegetationConfigGeneralDatabase : ScriptableObject
    {
        [Header("Trees")]
        public int maxTrees = 100;
        public float treeDensity = 0.3f;
        public float treeMinScale = 0.8f;
        public float treeMaxScale = 1.2f;

        [Header("Grass")]
        public int maxGrass = 500;
        public float grassDensity = 0.6f;
        public float grassMinScale = 0.3f;
        public float grassMaxScale = 0.7f;

        [Header("Flowers")]
        public int maxFlowers = 50;
        public float flowerDensity = 0.2f;
        public float flowerMinScale = 0.2f;
        public float flowerMaxScale = 0.4f;

        [Header("Spawn")]
        public float spawnRadius = 200f;
        public float despawnDistance = 250f;
        public int spawnBatchSize = 5;
    }
}
