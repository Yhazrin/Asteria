using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Audio settings file database for the game.
    /// Contains all Unity AudioSettings parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Audio Settings File Database")]
    public sealed class AudioSettingsFileDatabase : ScriptableObject
    {
        [Header("Audio")]
        public int outputSampleRate = 44100;
        public int dspBufferSize = 1024;
        public string speakerMode = "Stereo";

        [Header("3D Audio")]
        public float defaultSpatialBlend = 1f;
        public AudioRolloffMode defaultRolloff = AudioRolloffMode.Logarithmic;
        public float defaultMinDistance = 1f;
        public float defaultMaxDistance = 500f;

        [Header("Volume")]
        public float masterVolume = 1f;
        public float musicVolume = 0.7f;
        public float sfxVolume = 0.8f;
        public float ambientVolume = 0.6f;
    }
}
