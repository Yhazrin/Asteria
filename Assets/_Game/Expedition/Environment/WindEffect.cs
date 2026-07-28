using UnityEngine;

namespace Asteria.Expedition
{
    /// <summary>
    /// Applies wind effects to objects in the scene.
    /// Simulates grass movement, particle drift, and player pushback.
    /// </summary>
    public sealed class WindEffect : MonoBehaviour
    {
        [Header("Wind Settings")]
        [SerializeField] float windStrength = 5f;
        [SerializeField] float windVariation = 2f;
        [SerializeField] float gustFrequency = 0.3f;
        [SerializeField] float gustStrength = 3f;

        [Header("Affected Objects")]
        [SerializeField] Transform[] affectedObjects;
        [SerializeField] ParticleSystem[] affectedParticles;

        float _time;
        Vector3 _windDirection;
        Vector3 _currentWind;

        void Start()
        {
            _windDirection = Random.onUnitSphere;
            _windDirection.y = 0;
            _windDirection.Normalize();
        }

        void Update()
        {
            _time += Time.deltaTime;

            // Calculate wind with variation and gusts
            float baseWind = windStrength + Mathf.Sin(_time * windVariation) * windVariation;
            float gust = Mathf.PerlinNoise(_time * gustFrequency, 0) * gustStrength;
            float totalWind = baseWind + gust;

            _currentWind = _windDirection * totalWind;

            // Apply to affected objects (grass, plants)
            foreach (var obj in affectedObjects)
            {
                if (obj == null) continue;
                ApplyWindToObject(obj, totalWind);
            }

            // Apply to particles
            foreach (var ps in affectedParticles)
            {
                if (ps == null) continue;
                ApplyWindToParticles(ps, _currentWind);
            }
        }

        void ApplyWindToObject(Transform obj, float strength)
        {
            // Simulate bending with rotation
            float bendAngle = Mathf.Sin(_time * 2f + obj.position.x * 0.5f) * strength * 2f;
            obj.localRotation = Quaternion.Euler(bendAngle, 0, 0);
        }

        void ApplyWindToParticles(ParticleSystem ps, Vector3 wind)
        {
            var velocityModule = ps.velocityOverLifetime;
            velocityModule.enabled = true;
            velocityModule.x = wind.x;
            velocityModule.z = wind.z;
        }

        /// <summary>
        /// Get the wind force at a specific position (for player pushback).
        /// </summary>
        public Vector3 GetWindAtPosition(Vector3 position)
        {
            float noise = Mathf.PerlinNoise(position.x * 0.01f + _time * 0.5f, position.z * 0.01f);
            return _currentWind * (0.8f + noise * 0.4f);
        }

        /// <summary>
        /// Set wind direction (used by event director).
        /// </summary>
        public void SetWindDirection(Vector3 direction)
        {
            _windDirection = direction.normalized;
        }

        /// <summary>
        /// Set wind strength (used by event director).
        /// </summary>
        public void SetWindStrength(float strength)
        {
            windStrength = strength;
        }
    }
}
