using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Material file database for the game.
    /// Contains all material references.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Material File Database")]
    public sealed class MaterialFileDatabase : ScriptableObject
    {
        [Header("Terrain")]
        public Material terrainGrass;
        public Material terrainRock;
        public Material terrainSnow;
        public Material terrainSand;

        [Header("Nature")]
        public Material treeTrunk;
        public Material treeLeaves;
        public Material crystal;
        public Material windBell;

        [Header("Structures")]
        public Material beacon;
        public Material facility;
        public Material water;

        [Header("Characters")]
        public Material resident;
        public Material creature;

        [Header("Environment")]
        public Material atmosphere;
        public Material cloud;
        public Material fog;
    }
}
