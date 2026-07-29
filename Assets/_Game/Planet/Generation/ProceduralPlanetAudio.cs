using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Audio system for procedural planets.
    /// Handles ambient sounds, music, and environmental audio.
    /// </summary>
    public sealed class ProceduralPlanetAudio : MonoBehaviour
    {
        [Header("Ambient")]
        [SerializeField] float ambientVolume = 0.6f;
        [SerializeField] float ambientFadeSpeed = 0.5f;

        [Header("Music")]
        [SerializeField] float musicVolume = 0.5f;
        [SerializeField] float musicFadeSpeed = 0.3f;

        [Header("References")]
        [SerializeField] Weather.WeatherSystem weatherSystem;
        [SerializeField] PlanetBody planet;

        AudioSource _ambientSource;
        AudioSource _musicSource;
        float _currentAmbientVolume;
        float _currentMusicVolume;

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
            UpdateAmbient();
            UpdateMusic();
        }

        void CreateAudioSources()
        {
            // Ambient source
            var ambientGo = new GameObject("AmbientAudio");
            ambientGo.transform.SetParent(transform, false);
            _ambientSource = ambientGo.AddComponent<AudioSource>();
            _ambientSource.loop = true;
            _ambientSource.playOnAwake = false;
            _ambientSource.volume = 0f;

            // Music source
            var musicGo = new GameObject("MusicAudio");
            musicGo.transform.SetParent(transform, false);
            _musicSource = musicGo.AddComponent<AudioSource>();
            _musicSource.loop = true;
            _musicSource.playOnAwake = false;
            _musicSource.volume = 0f;
        }

        void UpdateAmbient()
        {
            if (weatherSystem == null) return;

            float targetVolume = ambientVolume;

            // Adjust based on weather
            if (weatherSystem.CurrentWeather == Weather.WeatherType.Storm)
                targetVolume *= 1.5f;
            else if (weatherSystem.CurrentWeather == Weather.WeatherType.Fog)
                targetVolume *= 0.8f;

            _currentAmbientVolume = Mathf.Lerp(_currentAmbientVolume, targetVolume, Time.deltaTime * ambientFadeSpeed);
            _ambientSource.volume = _currentAmbientVolume;

            if (_currentAmbientVolume > 0.01f && !_ambientSource.isPlaying)
                _ambientSource.Play();
            if (_currentAmbientVolume < 0.01f && _ambientSource.isPlaying)
                _ambientSource.Stop();
        }

        void UpdateMusic()
        {
            float targetVolume = musicVolume;
            _currentMusicVolume = Mathf.Lerp(_currentMusicVolume, targetVolume, Time.deltaTime * musicFadeSpeed);
            _musicSource.volume = _currentMusicVolume;
        }

        /// <summary>
        /// Play ambient sound.
        /// </summary>
        public void PlayAmbient(AudioClip clip, float volume = 1f)
        {
            if (clip == null) return;

            _ambientSource.clip = clip;
            _ambientSource.volume = volume * ambientVolume;
            _ambientSource.Play();
        }

        /// <summary>
        /// Stop ambient sound.
        /// </summary>
        public void StopAmbient()
        {
            _ambientSource.Stop();
        }

        /// <summary>
        /// Play music.
        /// </summary>
        public void PlayMusic(AudioClip clip, float volume = 1f)
        {
            if (clip == null) return;

            _musicSource.clip = clip;
            _musicSource.volume = volume * musicVolume;
            _musicSource.Play();
        }

        /// <summary>
        /// Stop music.
        /// </summary>
        public void StopMusic()
        {
            _musicSource.Stop();
        }

        /// <summary>
        /// Set ambient volume.
        /// </summary>
        public void SetAmbientVolume(float volume)
        {
            ambientVolume = Mathf.Clamp01(volume);
        }

        /// <summary>
        /// Set music volume.
        /// </summary>
        public void SetMusicVolume(float volume)
        {
            musicVolume = Mathf.Clamp01(volume);
        }
    }
}
