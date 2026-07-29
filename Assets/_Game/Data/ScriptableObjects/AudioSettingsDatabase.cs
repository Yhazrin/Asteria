using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Audio settings database.
    /// Contains all audio parameters for the game.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Audio Settings Database")]
    public sealed class AudioSettingsDatabase : ScriptableObject
    {
        [Header("Volume")]
        public float masterVolume = 1f;
        public float musicVolume = 0.7f;
        public float sfxVolume = 0.8f;
        public float ambientVolume = 0.6f;
        public float voiceVolume = 0.9f;

        [Header("Spatial Audio")]
        public bool enableSpatialAudio = true;
        public float spatialBlend = 1f;
        public float minDistance = 5f;
        public float maxDistance = 50f;

        [Header("Music")]
        public float musicFadeDuration = 2f;
        public float musicCrossfadeDuration = 4f;

        [Header("SFX")]
        public float sfxCooldown = 0.05f;
        public int sfxPoolSize = 10;

        [Header("Ambient")]
        public float ambientFadeDuration = 1f;
        public float windVolumeMultiplier = 0.5f;
    }
}
