using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Manages visual effects for different biomes.
    /// Handles colors, particles, fog, and ambient sounds per biome.
    /// </summary>
    public sealed class BiomeVisuals : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] BiomeMapper biomeMapper;
        [SerializeField] PlanetBody planet;

        [Header("Biome Colors")]
        [SerializeField] Color grasslandColor = new(0.45f, 0.62f, 0.48f);
        [SerializeField] Color forestColor = new(0.2f, 0.4f, 0.2f);
        [SerializeField] Color desertColor = new(0.8f, 0.7f, 0.4f);
        [SerializeField] Color snowColor = new(0.9f, 0.9f, 0.95f);
        [SerializeField] Color swampColor = new(0.3f, 0.4f, 0.25f);

        [Header("Biome Fog")]
        [SerializeField] float grasslandFog = 0.001f;
        [SerializeField] float forestFog = 0.003f;
        [SerializeField] float desertFog = 0.002f;
        [SerializeField] float snowFog = 0.004f;

        [Header("Particles")]
        [SerializeField] ParticleSystem grassParticles;
        [SerializeField] ParticleSystem forestParticles;
        [SerializeField] ParticleSystem desertParticles;
        [SerializeField] ParticleSystem snowParticles;

        BiomeMapper.BiomeType _currentBiome;

        void Start()
        {
            if (biomeMapper == null)
                biomeMapper = FindFirstObjectByType<BiomeMapper>();
            if (planet == null)
                planet = FindFirstObjectByType<PlanetBody>();
        }

        void Update()
        {
            if (biomeMapper == null || planet == null) return;

            // Get current biome from player position
            var player = FindFirstObjectByType<Player.SphericalGravityBody>();
            if (player == null) return;

            var newBiome = biomeMapper.GetBiome(player.transform.position.normalized, planet.Radius);
            if (newBiome != _currentBiome)
            {
                TransitionToBiome(newBiome);
                _currentBiome = newBiome;
            }
        }

        void TransitionToBiome(BiomeMapper.BiomeType biome)
        {
            Debug.Log($"[BiomeVisuals] Transitioning to {biome}");

            // Update fog
            UpdateFog(biome);

            // Update particles
            UpdateParticles(biome);

            // Update ambient color
            UpdateAmbientColor(biome);
        }

        void UpdateFog(BiomeMapper.BiomeType biome)
        {
            float density = biome switch
            {
                BiomeMapper.BiomeType.Forest => forestFog,
                BiomeMapper.BiomeType.Swamp => forestFog * 1.5f,
                BiomeMapper.BiomeType.Desert => desertFog,
                BiomeMapper.BiomeType.Snowy => snowFog,
                BiomeMapper.BiomeType.Tundra => snowFog * 0.8f,
                _ => grasslandFog
            };

            RenderSettings.fogDensity = density;
        }

        void UpdateParticles(BiomeMapper.BiomeType biome)
        {
            // Disable all
            SetParticleEmission(grassParticles, 0);
            SetParticleEmission(forestParticles, 0);
            SetParticleEmission(desertParticles, 0);
            SetParticleEmission(snowParticles, 0);

            // Enable current biome
            switch (biome)
            {
                case BiomeMapper.BiomeType.Plains:
                case BiomeMapper.BiomeType.Savanna:
                    SetParticleEmission(grassParticles, 20);
                    break;
                case BiomeMapper.BiomeType.Forest:
                case BiomeMapper.BiomeType.Woodland:
                case BiomeMapper.BiomeType.Jungle:
                    SetParticleEmission(forestParticles, 30);
                    break;
                case BiomeMapper.BiomeType.Desert:
                case BiomeMapper.BiomeType.Badlands:
                    SetParticleEmission(desertParticles, 15);
                    break;
                case BiomeMapper.BiomeType.Snowy:
                case BiomeMapper.BiomeType.Tundra:
                    SetParticleEmission(snowParticles, 25);
                    break;
            }
        }

        void SetParticleEmission(ParticleSystem ps, float rate)
        {
            if (ps == null) return;
            var emission = ps.emission;
            emission.rateOverTime = rate;
        }

        void UpdateAmbientColor(BiomeMapper.BiomeType biome)
        {
            Color skyColor = biome switch
            {
                BiomeMapper.BiomeType.Forest => new Color(0.3f, 0.5f, 0.3f),
                BiomeMapper.BiomeType.Desert => new Color(0.8f, 0.7f, 0.5f),
                BiomeMapper.BiomeType.Snowy => new Color(0.7f, 0.8f, 0.9f),
                BiomeMapper.BiomeType.Swamp => new Color(0.3f, 0.4f, 0.3f),
                _ => new Color(0.5f, 0.7f, 0.9f)
            };

            RenderSettings.ambientSkyColor = skyColor;
        }

        /// <summary>
        /// Get the current biome.
        /// </summary>
        public BiomeMapper.BiomeType GetCurrentBiome()
        {
            return _currentBiome;
        }
    }
}
