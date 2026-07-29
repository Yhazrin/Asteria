using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// General audio configuration database for the game.
    /// Contains all audio parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Audio Config General Database")]
    public sealed class AudioConfigGeneralDatabase : ScriptableObject
    {
        [Header("Master")]
        public float masterVolume = 1f;
        public float musicVolume = 0.7f;
        public float sfxVolume = 0.8f;
        public float ambientVolume = 0.6f;

        [Header("Music")]
        public float musicFadeDuration = 2f;
        public float musicCrossfadeDuration = 4f;

        [Header("SFX")]
        public float sfxCooldown = 0.05f;
        public int sfxPoolSize = 10;

        [Header("Spatial")]
        public bool enableSpatialAudio = true;
        public float spatialBlend = 1f;
        public float minDistance = 5f;
        public float maxDistance = 50f;
    }
}
