using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Chunk configuration database for the game.
    /// Contains all chunk parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Chunk Config Database")]
    public sealed class ChunkConfigDatabase : ScriptableObject
    {
        [Header("Chunk Settings")]
        public int chunkSize = 64;
        public int chunksPerRing = 8;
        public int rings = 6;
        public int chunkResolution = 32;

        [Header("Streaming")]
        public float loadDistance = 500f;
        public float unloadDistance = 800f;
        public int maxActiveChunks = 50;
        public int chunksPerFrame = 2;

        [Header("LOD")]
        public bool enableChunkLOD = true;
        public float lodDistanceMultiplier = 1.5f;
    }
}
