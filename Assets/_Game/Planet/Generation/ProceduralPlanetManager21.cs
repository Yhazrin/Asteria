using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Manager with planet import.
    /// </summary>
    public sealed class ProceduralPlanetManager21 : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] Transform player;

        void Start()
        {
            if (player == null)
            {
                var playerBody = FindFirstObjectByType<Player.SphericalGravityBody>();
                player = playerBody?.transform;
            }
        }

        /// <summary>
        /// Import planet from JSON.
        /// </summary>
        public bool ImportPlanet(string json)
        {
            try
            {
                var data = JsonUtility.FromJson<PlanetImport>(json);
                Debug.Log($"[ProceduralPlanetManager21] Imported planet: {data.name}");
                return true;
            }
            catch
            {
                Debug.LogError("[ProceduralPlanetManager21] Failed to import planet.");
                return false;
            }
        }

        [System.Serializable]
        public class PlanetImport
        {
            public string name;
            public float radius;
            public int seed;
        }
    }
}
