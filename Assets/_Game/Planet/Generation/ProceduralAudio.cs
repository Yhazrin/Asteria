using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Generates and manages procedural audio for the planet.
    /// Handles ambient sounds, music, and environmental audio.
    /// </summary>
    public sealed class ProceduralAudio : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float masterVolume = 0.7f;
        [SerializeField] float musicVolume = 0.5f;
        [SerializeField] float ambientVolume = 0.6f;
        [SerializeField] float sfxVolume = 0.8f;

        [Header("Ambient")]
        [SerializeField] float windBaseVolume = 0.3f;
        [SerializeField] float windMaxVolume = 0.8f;
        [SerializeField] float windFrequency = 0.1f;

        [Header("Music")]
        [SerializeField] float musicFadeSpeed = 0.5f;

        [Header("References")]
        [SerializeField] Weather.WeatherSystem weatherSystem;
        [SerializeField] PlanetBody planet;

        AudioSource _windSource;
        AudioSource _musicSource;
        AudioSource _ambientSource;

        float _windVolume;
        float _targetWindVolume;

        void Start()
        {
            if (weatherSystem == null)
                weatherSystem = FindFirstObjectByType<Weather.WeatherSystem>();
            if (planet == null)
                planet = FindFirstObjectByType<PlanetBody>();

            CreateAudioSources();
        }

        void Update()
        {
            UpdateWind();
            UpdateMusic();
        }

        void CreateAudioSources()
        {
            // Wind source
            var windGo = new GameObject("WindAudio");
            windGo.transform.SetParent(transform, false);
            _windSource = windGo.AddComponent<AudioSource>();
            _windSource.loop = true;
            _windSource.playOnAwake = false;
            _windSource.volume = 0f;

            // Music source
            var musicGo = new GameObject("MusicAudio");
            musicGo.transform.SetParent(transform, false);
            _musicSource = musicGo.AddComponent<AudioSource>();
            _musicSource.loop = true;
            _musicSource.playOnAwake = false;
            _musicSource.volume = 0f;

            // Ambient source
            var ambientGo = new GameObject("AmbientAudio");
            ambientGo.transform.SetParent(transform, false);
            _ambientSource = ambientGo.AddComponent<AudioSource>();
            _ambientSource.loop = true;
            _ambientSource.playOnAwake = false;
            _ambientSource.volume = 0f;
        }

        void UpdateWind()
        {
            if (weatherSystem == null) return;

            // Wind volume based on weather
            float weatherWind = weatherSystem.GetWindStrength();
            _targetWindVolume = Mathf.Lerp(windBaseVolume, windMaxVolume, weatherWind);

            // Smooth transition
            _windVolume = Mathf.Lerp(_windVolume, _targetWindVolume, Time.deltaTime * 2f);
            _windSource.volume = _windVolume * ambientVolume * masterVolume;

            // Wind pitch variation
            _windSource.pitch = 1f + Mathf.Sin(Time.time * windFrequency) * 0.1f;

            if (_windVolume > 0.01f && !_windSource.isPlaying)
                _windSource.Play();
            if (_windVolume < 0.01f && _windSource.isPlaying)
                _windSource.Stop();
        }

        void UpdateMusic()
        {
            // Music volume fades based on game state
            float targetVolume = musicVolume * masterVolume;
            _musicSource.volume = Mathf.Lerp(_musicSource.volume, targetVolume, Time.deltaTime * musicFadeSpeed);
        }

        /// <summary>
        /// Play a one-shot sound effect.
        /// </summary>
        public void PlaySFX(AudioClip clip, float volume = 1f, float pitch = 1f)
        {
            if (clip == null) return;

            var source = GetAvailableSFXSource();
            if (source == null) return;

            source.clip = clip;
            source.volume = volume * sfxVolume * masterVolume;
            source.pitch = pitch;
            source.Play();
        }

        /// <summary>
        /// Play a 3D sound effect at a position.
        /// </summary>
        public void PlaySFX3D(AudioClip clip, Vector3 position, float volume = 1f)
        {
            if (clip == null) return;

            var source = GetAvailableSFXSource();
            if (source == null) return;

            source.transform.position = position;
            source.clip = clip;
            source.volume = volume * sfxVolume * masterVolume;
            source.spatialBlend = 1f;
            source.Play();
        }

        AudioSource GetAvailableSFXSource()
        {
            // Find an available source
            var sources = GetComponentsInChildren<AudioSource>();
            foreach (var source in sources)
            {
                if (source != _windSource && source != _musicSource && source != _ambientSource)
                {
                    if (!source.isPlaying) return source;
                }
            }
            return null;
        }

        /// <summary>
        /// Set the master volume.
        /// </summary>
        public void SetMasterVolume(float volume)
        {
            masterVolume = Mathf.Clamp01(volume);
        }

        /// <summary>
        /// Set the music volume.
        /// </summary>
        public void SetMusicVolume(float volume)
        {
            musicVolume = Mathf.Clamp01(volume);
        }

        /// <summary>
        /// Set the ambient volume.
        /// </summary>
        public void SetAmbientVolume(float volume)
        {
            ambientVolume = Mathf.Clamp01(volume);
        }

        /// <summary>
        /// Set the SFX volume.
        /// </summary>
        public void SetSFXVolume(float volume)
        {
            sfxVolume = Mathf.Clamp01(volume);
        }

        /// <summary>
        /// Play ambient sound.
        /// </summary>
        public void PlayAmbient(AudioClip clip, float volume = 1f)
        {
            if (clip == null) return;

            _ambientSource.clip = clip;
            _ambientSource.volume = volume * ambientVolume * masterVolume;
            _ambientSource.Play();
        }

        /// <summary>
        /// Stop ambient sound.
        /// </summary>
        public void StopAmbient()
        {
            _ambientSource.Stop();
        }
    }
}
