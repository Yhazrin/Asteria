using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Object pool database for the game.
    /// Contains all object pool configurations.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Pool Database")]
    public sealed class PoolDatabase : ScriptableObject
    {
        [Header("Particle Pools")]
        public int windParticlePoolSize = 10;
        public int discoverySparklePoolSize = 20;
        public int fireflyPoolSize = 30;
        public int rainPoolSize = 50;

        [Header("Audio Pools")]
        public int sfxPoolSize = 10;
        public int ambientPoolSize = 4;

        [Header("Object Pools")]
        public int creaturePoolSize = 30;
        public int vegetationPoolSize = 100;
        public int chunkPoolSize = 20;
    }
}
