using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Cache database for the game.
    /// Contains all cached data.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Cache Database")]
    public sealed class CacheDatabase : ScriptableObject
    {
        [Header("Mesh Cache")]
        public int maxCachedMeshes = 50;
        public int cachedMeshCount = 0;

        [Header("Texture Cache")]
        public int maxCachedTextures = 100;
        public int cachedTextureCount = 0;

        [Header("Material Cache")]
        public int maxCachedMaterials = 50;
        public int cachedMaterialCount = 0;

        [Header("Audio Cache")]
        public int maxCachedAudio = 30;
        public int cachedAudioCount = 0;
    }
}
