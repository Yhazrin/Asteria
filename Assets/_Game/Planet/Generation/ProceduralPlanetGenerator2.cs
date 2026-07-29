using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Alternative planet generator with different approach.
    /// Uses hexagonal grid instead of spherical coordinates.
    /// </summary>
    public sealed class ProceduralPlanetGenerator2 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float planetRadius = 300f;
        [SerializeField] int seed = 42;
        [SerializeField] int hexResolution = 20;

        [Header("References")]
        [SerializeField] PlanetBody planet;

        void Start()
        {
            if (planet == null)
                planet = FindFirstObjectByType<PlanetBody>();
        }

        /// <summary>
        /// Generate planet using hexagonal grid.
        /// </summary>
        public void GenerateHexPlanet()
        {
            Debug.Log("[ProceduralPlanetGenerator2] Generating hex planet...");

            // Create hexagonal grid on sphere
            for (int i = 0; i < hexResolution; i++)
            {
                for (int j = 0; j < hexResolution; j++)
                {
                    // Convert hex coordinates to sphere position
                    float theta = (float)i / hexResolution * Mathf.PI * 2f;
                    float phi = (float)j / hexResolution * Mathf.PI;

                    float x = Mathf.Sin(phi) * Mathf.Cos(theta);
                    float y = Mathf.Cos(phi);
                    float z = Mathf.Sin(phi) * Mathf.Sin(theta);

                    Vector3 direction = new Vector3(x, y, z).normalized;
                    Vector3 position = direction * planetRadius;

                    // Create hex tile
                    var go = new GameObject($"Hex_{i}_{j}");
                    go.transform.SetParent(transform, false);
                    go.transform.position = position;
                    go.transform.up = direction;

                    // Add mesh
                    var filter = go.AddComponent<MeshFilter>();
                    filter.mesh = CreateHexMesh();

                    var renderer = go.AddComponent<MeshRenderer>();
                    renderer.material = Art.MaterialLibrary.TerrainGrass;
                }
            }

            Debug.Log("[ProceduralPlanetGenerator2] Hex planet generation complete.");
        }

        Mesh CreateHexMesh()
        {
            // Create hexagonal mesh
            float radius = planetRadius / hexResolution;
            var verts = new Vector3[7];
            var tris = new int[18];

            verts[0] = Vector3.zero; // Center

            for (int i = 0; i < 6; i++)
            {
                float angle = i * 60f * Mathf.Deg2Rad;
                verts[i + 1] = new Vector3(
                    Mathf.Cos(angle) * radius,
                    0f,
                    Mathf.Sin(angle) * radius);
            }

            int t = 0;
            for (int i = 0; i < 6; i++)
            {
                int next = (i + 1) % 6;
                tris[t++] = 0;
                tris[t++] = i + 1;
                tris[t++] = next + 1;
            }

            var mesh = new Mesh { name = "Hex" };
            mesh.vertices = verts;
            mesh.triangles = tris;
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            return mesh;
        }
    }
}
