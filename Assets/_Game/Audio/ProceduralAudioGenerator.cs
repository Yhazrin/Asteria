using UnityEngine;

namespace Asteria.Audio
{
    /// <summary>
    /// Generates procedural audio clips at runtime.
    /// Creates all game sounds without external audio files.
    /// </summary>
    public static class ProceduralAudioGenerator
    {
        public static AudioClip GenerateTone(float frequency, float duration, float volume = 0.5f, string name = "tone")
        {
            int sampleRate = AudioSettings.outputSampleRate;
            int sampleCount = Mathf.RoundToInt(sampleRate * duration);
            var clip = AudioClip.Create(name, sampleCount, 1, sampleRate, false);

            var samples = new float[sampleCount];
            for (int i = 0; i < sampleCount; i++)
            {
                float t = (float)i / sampleRate;
                float envelope = Mathf.Clamp01(1f - (float)i / sampleCount * 2f); // Fade out
                samples[i] = Mathf.Sin(2f * Mathf.PI * frequency * t) * volume * envelope;
            }

            clip.SetData(samples, 0);
            return clip;
        }

        public static AudioClip GenerateWind(float duration, float intensity = 0.5f)
        {
            int sampleRate = AudioSettings.outputSampleRate;
            int sampleCount = Mathf.RoundToInt(sampleRate * duration);
            var clip = AudioClip.Create("Wind", sampleCount, 1, sampleRate, false);

            var samples = new float[sampleCount];
            var rng = new System.Random(42);

            for (int i = 0; i < sampleCount; i++)
            {
                float t = (float)i / sampleRate;
                float noise = (float)(rng.NextDouble() * 2 - 1);
                float mod = Mathf.Sin(t * 0.5f) * 0.3f + 0.7f;
                samples[i] = noise * intensity * mod * 0.3f;
            }

            clip.SetData(samples, 0);
            return clip;
        }

        public static AudioClip GenerateFootstep(float duration = 0.15f)
        {
            int sampleRate = AudioSettings.outputSampleRate;
            int sampleCount = Mathf.RoundToInt(sampleRate * duration);
            var clip = AudioClip.Create("Footstep", sampleCount, 1, sampleRate, false);

            var samples = new float[sampleCount];
            var rng = new System.Random();

            for (int i = 0; i < sampleCount; i++)
            {
                float t = (float)i / sampleRate;
                float envelope = Mathf.Exp(-t * 20f);
                float noise = (float)(rng.NextDouble() * 2 - 1);
                float tone = Mathf.Sin(t * 200f) * 0.3f;
                samples[i] = (noise * 0.5f + tone) * envelope * 0.4f;
            }

            clip.SetData(samples, 0);
            return clip;
        }

        public static AudioClip GenerateDiscoverySound()
        {
            int sampleRate = AudioSettings.outputSampleRate;
            float duration = 1.5f;
            int sampleCount = Mathf.RoundToInt(sampleRate * duration);
            var clip = AudioClip.Create("Discovery", sampleCount, 1, sampleRate, false);

            var samples = new float[sampleCount];
            float[] frequencies = { 523f, 659f, 784f, 1047f }; // C5, E5, G5, C6

            for (int i = 0; i < sampleCount; i++)
            {
                float t = (float)i / sampleRate;
                float noteIndex = t * 4f;
                int noteIdx = Mathf.Min(Mathf.FloorToInt(noteIndex), frequencies.Length - 1);
                float freq = frequencies[noteIdx];

                float envelope = Mathf.Clamp01(1f - t / duration);
                envelope *= Mathf.Sin(t * Mathf.PI / duration); // Bell shape

                samples[i] = Mathf.Sin(2f * Mathf.PI * freq * t) * envelope * 0.4f;
            }

            clip.SetData(samples, 0);
            return clip;
        }

        public static AudioClip GenerateRestoreSound()
        {
            int sampleRate = AudioSettings.outputSampleRate;
            float duration = 2f;
            int sampleCount = Mathf.RoundToInt(sampleRate * duration);
            var clip = AudioClip.Create("Restore", sampleCount, 1, sampleRate, false);

            var samples = new float[sampleCount];

            for (int i = 0; i < sampleCount; i++)
            {
                float t = (float)i / sampleRate;
                float envelope = Mathf.Clamp01(1f - t / duration);
                float freq = 200f + t * 300f; // Rising pitch
                samples[i] = Mathf.Sin(2f * Mathf.PI * freq * t) * envelope * 0.3f;
            }

            clip.SetData(samples, 0);
            return clip;
        }

        public static AudioClip GenerateCooperateSound()
        {
            int sampleRate = AudioSettings.outputSampleRate;
            float duration = 2f;
            int sampleCount = Mathf.RoundToInt(sampleRate * duration);
            var clip = AudioClip.Create("Cooperate", sampleCount, 1, sampleRate, false);

            var samples = new float[sampleCount];

            for (int i = 0; i < sampleCount; i++)
            {
                float t = (float)i / sampleRate;
                float envelope = Mathf.Exp(-t * 0.5f);
                float freq1 = 440f;
                float freq2 = 554f; // Harmony
                float harmony = Mathf.Sin(2f * Mathf.PI * freq1 * t) + Mathf.Sin(2f * Mathf.PI * freq2 * t);
                samples[i] = harmony * envelope * 0.25f;
            }

            clip.SetData(samples, 0);
            return clip;
        }

        public static AudioClip GenerateUIClick()
        {
            int sampleRate = AudioSettings.outputSampleRate;
            float duration = 0.08f;
            int sampleCount = Mathf.RoundToInt(sampleRate * duration);
            var clip = AudioClip.Create("UIClick", sampleCount, 1, sampleRate, false);

            var samples = new float[sampleCount];

            for (int i = 0; i < sampleCount; i++)
            {
                float t = (float)i / sampleRate;
                float envelope = Mathf.Exp(-t * 50f);
                samples[i] = Mathf.Sin(2f * Mathf.PI * 800f * t) * envelope * 0.3f;
            }

            clip.SetData(samples, 0);
            return clip;
        }

        public static AudioClip GenerateRain(float duration = 5f)
        {
            int sampleRate = AudioSettings.outputSampleRate;
            int sampleCount = Mathf.RoundToInt(sampleRate * duration);
            var clip = AudioClip.Create("Rain", sampleCount, 1, sampleRate, false);

            var samples = new float[sampleCount];
            var rng = new System.Random(123);

            for (int i = 0; i < sampleCount; i++)
            {
                float noise = (float)(rng.NextDouble() * 2 - 1);
                float filtered = noise * 0.15f; // White noise filtered
                samples[i] = filtered;
            }

            clip.SetData(samples, 0);
            return clip;
        }

        public static AudioClip GenerateCreatureCall(string creatureType)
        {
            int sampleRate = AudioSettings.outputSampleRate;
            float duration = 1f;
            int sampleCount = Mathf.RoundToInt(sampleRate * duration);
            var clip = AudioClip.Create($"Creature_{creatureType}", sampleCount, 1, sampleRate, false);

            var samples = new float[sampleCount];
            float baseFreq = creatureType switch
            {
                "curious" => 600f,
                "shy" => 300f,
                "guide" => 500f,
                "disturbing" => 200f,
                _ => 400f
            };

            for (int i = 0; i < sampleCount; i++)
            {
                float t = (float)i / sampleRate;
                float envelope = Mathf.Sin(t * Mathf.PI / duration);
                float vibrato = Mathf.Sin(t * 8f) * 20f;
                samples[i] = Mathf.Sin(2f * Mathf.PI * (baseFreq + vibrato) * t) * envelope * 0.25f;
            }

            clip.SetData(samples, 0);
            return clip;
        }
    }
}
