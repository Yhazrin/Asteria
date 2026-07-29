using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// General particle database for the game.
    /// Contains all particle parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Particle General Database")]
    public sealed class ParticleGeneralDatabase : ScriptableObject
    {
        [Header("Wind Particles")]
        public int windParticleCount = 200;
        public float windParticleSpeed = 5f;
        public float windParticleSize = 0.1f;
        public Color windParticleColor = new(1f, 1f, 1f, 0.3f);

        [Header("Firefly Particles")]
        public int fireflyCount = 50;
        public float fireflySpeed = 0.5f;
        public float fireflySize = 0.15f;
        public Color fireflyColor = new(0.9f, 0.9f, 0.3f, 0.8f);

        [Header("Rain Particles")]
        public int rainCount = 500;
        public float rainSpeed = 15f;
        public float rainSize = 0.05f;
        public Color rainColor = new(0.7f, 0.8f, 0.9f, 0.5f);

        [Header("Snow Particles")]
        public int snowCount = 300;
        public float snowSpeed = 2f;
        public float snowSize = 0.1f;
        public Color snowColor = new(0.95f, 0.95f, 0.98f, 0.8f);
    }
}
