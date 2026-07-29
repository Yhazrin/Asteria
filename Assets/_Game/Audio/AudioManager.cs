using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Audio
{
    /// <summary>
    /// Central audio manager for music, SFX, and ambient sounds.
    /// Handles volume control, crossfading, and spatial audio.
    /// </summary>
    public sealed class AudioManager : MonoBehaviour
    {
        static AudioManager _instance;

        [Header("Sources")]
        [SerializeField] AudioSource musicSource;
        [SerializeField] AudioSource sfxSource;
        [SerializeField] AudioSource ambientSource;

        [Header("Settings")]
        [SerializeField] float crossfadeDuration = 2f;
        [SerializeField] float sfxCooldown = 0.05f;

        // Volume multipliers
        float _masterVolume = 1f;
        float _musicVolume = 0.7f;
        float _sfxVolume = 0.8f;
        float _ambientVolume = 0.5f;

        // SFX pool
        readonly Queue<AudioSource> _sfxPool = new();
        readonly Dictionary<string, float> _sfxCooldowns = new();

        // Music state
        AudioClip _currentMusic;
        float _crossfadeTimer;
        bool _isCrossfading;

        public static AudioManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    var go = new GameObject("AudioManager");
                    _instance = go.AddComponent<AudioManager>();
                }
                return _instance;
            }
        }

        void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }
            _instance = this;
            DontDestroyOnLoad(gameObject);
            Initialize();
        }

        void Initialize()
        {
            // Create audio sources if not assigned
            if (musicSource == null)
            {
                musicSource = gameObject.AddComponent<AudioSource>();
                musicSource.loop = true;
                musicSource.playOnAwake = false;
            }

            if (sfxSource == null)
            {
                sfxSource = gameObject.AddComponent<AudioSource>();
                sfxSource.playOnAwake = false;
            }

            if (ambientSource == null)
            {
                ambientSource = gameObject.AddComponent<AudioSource>();
                ambientSource.loop = true;
                ambientSource.playOnAwake = false;
            }

            // Create SFX pool
            for (int i = 0; i < 10; i++)
            {
                var poolGo = new GameObject($"SFXPool_{i}");
                poolGo.transform.SetParent(transform, false);
                var source = poolGo.AddComponent<AudioSource>();
                source.playOnAwake = false;
                _sfxPool.Enqueue(source);
            }

            // Load saved volumes
            _musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.7f);
            _sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 0.8f);
        }

        void Update()
        {
            if (_isCrossfading)
            {
                UpdateCrossfade();
            }
        }

        // === Music ===

        /// <summary>
        /// Play music with crossfade.
        /// </summary>
        public void PlayMusic(AudioClip clip, float volume = 1f)
        {
            if (clip == null || clip == _currentMusic) return;

            _currentMusic = clip;
            _crossfadeTimer = 0f;
            _isCrossfading = true;

            // Start new music at 0 volume
            musicSource.clip = clip;
            musicSource.volume = 0f;
            musicSource.Play();
        }

        /// <summary>
        /// Stop music with fade out.
        /// </summary>
        public void StopMusic(float fadeDuration = 1f)
        {
            StartCoroutine(FadeOutMusic(fadeDuration));
        }

        System.Collections.IEnumerator FadeOutMusic(float duration)
        {
            float startVol = musicSource.volume;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                musicSource.volume = Mathf.Lerp(startVol, 0f, elapsed / duration);
                yield return null;
            }

            musicSource.Stop();
            _currentMusic = null;
        }

        void UpdateCrossfade()
        {
            _crossfadeTimer += Time.deltaTime;
            float t = Mathf.Clamp01(_crossfadeTimer / crossfadeDuration);

            musicSource.volume = t * _musicVolume * _masterVolume;

            if (t >= 1f)
            {
                _isCrossfading = false;
            }
        }

        // === SFX ===

        /// <summary>
        /// Play a sound effect.
        /// </summary>
        public void PlaySFX(AudioClip clip, float volume = 1f, float pitch = 1f)
        {
            if (clip == null) return;

            // Cooldown check
            if (_sfxCooldowns.TryGetValue(clip.name, out float lastPlay))
            {
                if (Time.time - lastPlay < sfxCooldown) return;
            }

            _sfxCooldowns[clip.name] = Time.time;

            // Get pooled source
            var source = GetPooledSFXSource();
            if (source == null) return;

            source.clip = clip;
            source.volume = volume * _sfxVolume * _masterVolume;
            source.pitch = pitch;
            source.Play();
        }

        /// <summary>
        /// Play a sound effect at a world position (3D).
        /// </summary>
        public void PlaySFXAtPosition(AudioClip clip, Vector3 position, float volume = 1f)
        {
            if (clip == null) return;

            var source = GetPooledSFXSource();
            if (source == null) return;

            source.transform.position = position;
            source.clip = clip;
            source.volume = volume * _sfxVolume * _masterVolume;
            source.spatialBlend = 1f; // 3D
            source.minDistance = 5f;
            source.maxDistance = 50f;
            source.Play();
        }

        AudioSource GetPooledSFXSource()
        {
            foreach (var source in _sfxPool)
            {
                if (!source.isPlaying) return source;
            }
            return _sfxPool.Peek(); // Reuse oldest
        }

        // === Ambient ===

        /// <summary>
        /// Play ambient sound.
        /// </summary>
        public void PlayAmbient(AudioClip clip, float volume = 1f)
        {
            if (clip == null) return;

            ambientSource.clip = clip;
            ambientSource.volume = volume * _ambientVolume * _masterVolume;
            ambientSource.Play();
        }

        /// <summary>
        /// Stop ambient sound.
        /// </summary>
        public void StopAmbient()
        {
            ambientSource.Stop();
        }

        // === Volume Control ===

        public void SetMasterVolume(float volume)
        {
            _masterVolume = Mathf.Clamp01(volume);
            UpdateAllVolumes();
        }

        public void SetMusicVolume(float volume)
        {
            _musicVolume = Mathf.Clamp01(volume);
            PlayerPrefs.SetFloat("MusicVolume", _musicVolume);
            UpdateAllVolumes();
        }

        public void SetSFXVolume(float volume)
        {
            _sfxVolume = Mathf.Clamp01(volume);
            PlayerPrefs.SetFloat("SFXVolume", _sfxVolume);
        }

        public void SetAmbientVolume(float volume)
        {
            _ambientVolume = Mathf.Clamp01(volume);
            UpdateAllVolumes();
        }

        void UpdateAllVolumes()
        {
            if (musicSource != null)
                musicSource.volume = _musicVolume * _masterVolume;
            if (ambientSource != null)
                ambientSource.volume = _ambientVolume * _masterVolume;
        }

        // === Utility ===

        /// <summary>
        /// Generate a simple tone (for UI feedback).
        /// </summary>
        public void PlayTone(float frequency, float duration, float volume = 0.5f)
        {
            int sampleRate = AudioSettings.outputSampleRate;
            int sampleCount = Mathf.RoundToInt(sampleRate * duration);
            var clip = AudioClip.Create("Tone", sampleCount, 1, sampleRate, false);

            var samples = new float[sampleCount];
            for (int i = 0; i < sampleCount; i++)
            {
                float t = (float)i / sampleRate;
                samples[i] = Mathf.Sin(2f * Mathf.PI * frequency * t) * volume;
                // Fade out
                samples[i] *= 1f - (float)i / sampleCount;
            }

            clip.SetData(samples, 0);
            PlaySFX(clip, volume);
        }

        /// <summary>
        /// Play a UI click sound.
        /// </summary>
        public void PlayUIClick()
        {
            PlayTone(800f, 0.05f, 0.3f);
        }

        /// <summary>
        /// Play a discovery sound.
        /// </summary>
        public void PlayDiscovery()
        {
            PlayTone(523f, 0.1f, 0.4f); // C5
            // Could chain: PlayTone(659f, 0.1f, 0.4f); // E5
        }

        void OnDestroy()
        {
            if (_instance == this) _instance = null;
        }
    }
}
