using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Planet generator with mesh export.
    /// </summary>
    public sealed class ProceduralPlanetGenerator23 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float planetRadius = 300f;
        [SerializeField] int seed = 42;
        [SerializeField] int resolution = 128;

        [Header("References")]
        [SerializeField] PlanetBody planet;

        void Start()
        {
            if (planet == null)
                planet = FindFirstObjectByType<PlanetBody>();
        }

        /// <summary>
        /// Export mesh to OBJ format (simplified).
        /// </summary>
        public string ExportToOBJ(Mesh mesh)
        {
            if (mesh == null) return "";

            var sb = new System.Text.StringBuilder();
            sb.AppendLine("# Asteria Planet Mesh Export");
            sb.AppendLine($"# Vertices: {mesh.vertexCount}");
            sb.AppendLine($"# Triangles: {mesh.triangles.Length / 3}");
            sb.AppendLine();

            // Vertices
            foreach (var vertex in mesh.vertices)
            {
                sb.AppendLine($"v {vertex.x} {vertex.y} {vertex.z}");
            }

            // Normals
            foreach (var normal in mesh.normals)
            {
                sb.AppendLine($"vn {normal.x} {normal.y} {normal.z}");
            }

            // UVs
            foreach (var uv in mesh.uv)
            {
                sb.AppendLine($"vt {uv.x} {uv.y}");
            }

            // Triangles
            var triangles = mesh.triangles;
            for (int i = 0; i < triangles.Length; i += 3)
            {
                int a = triangles[i] + 1;
                int b = triangles[i + 1] + 1;
                int c = triangles[i + 2] + 1;
                sb.AppendLine($"f {a}/{a}/{a} {b}/{b}/{b} {c}/{c}/{c}");
            }

            return sb.ToString();
        }
    }
}
