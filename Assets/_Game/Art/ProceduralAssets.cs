using UnityEngine;

namespace Asteria.Art
{
    /// <summary>
    /// Creates all visual assets procedurally at runtime.
    /// No external assets needed — everything generated in code.
    /// </summary>
    public static class ProceduralAssets
    {
        // === Color Palette (低饱和三渲二风格) ===

        public static readonly Color GrassGreen = new(0.45f, 0.62f, 0.48f);
        public static readonly Color WarmGray = new(0.7f, 0.65f, 0.6f);
        public static readonly Color SkyBlue = new(0.55f, 0.7f, 0.9f);
        public static readonly Color SunsetOrange = new(0.95f, 0.7f, 0.4f);
        public static readonly Color NightBlue = new(0.15f, 0.15f, 0.3f);
        public static readonly Color WindBellGold = new(0.95f, 0.85f, 0.4f);
        public static readonly Color ResidentWarm = new(0.9f, 0.8f, 0.75f);
        public static readonly Color ResidentCool = new(0.7f, 0.8f, 0.9f);
        public static readonly Color CrystalBlue = new(0.6f, 0.85f, 1f);
        public static readonly Color FlowerPink = new(0.95f, 0.7f, 0.8f);
        public static readonly Color TreeBrown = new(0.5f, 0.35f, 0.2f);
        public static readonly Color TreeGreen = new(0.3f, 0.55f, 0.3f);
        public static readonly Color RockGray = new(0.55f, 0.5f, 0.45f);
        public static readonly Color SnowWhite = new(0.92f, 0.92f, 0.95f);
        public static readonly Color SandYellow = new(0.85f, 0.75f, 0.5f);

        // === Materials ===

        static Material _urpLit;
        static Material URP
        {
            get
            {
                if (_urpLit == null)
                {
                    var shader = Shader.Find("Universal Render Pipeline/Lit")
                                 ?? Shader.Find("Universal Render Pipeline/Simple Lit")
                                 ?? Shader.Find("Sprites/Default")
                                 ?? Shader.Find("Standard");
                    _urpLit = new Material(shader);
                }
                return _urpLit;
            }
        }

        public static Material MakeMat(Color color, float smoothness = 0.2f, float metallic = 0f)
        {
            var mat = new Material(URP);
            if (mat.HasProperty("_BaseColor")) mat.SetColor("_BaseColor", color);
            if (mat.HasProperty("_Color")) mat.SetColor("_Color", color);
            if (mat.HasProperty("_Smoothness")) mat.SetFloat("_Smoothness", smoothness);
            if (mat.HasProperty("_Metallic")) mat.SetFloat("_Metallic", metallic);
            return mat;
        }

        public static Material MakeEmissiveMat(Color color, float intensity = 1f)
        {
            var mat = MakeMat(color);
            if (mat.HasProperty("_EmissionColor"))
            {
                mat.EnableKeyword("_EMISSION");
                mat.SetColor("_EmissionColor", color * intensity);
            }
            return mat;
        }

        // === Meshes ===

        /// <summary>Low-poly tree trunk + crown</summary>
        public static Mesh MakeTreeMesh()
        {
            // Combine cylinder trunk + cone crown
            var trunk = MakeCylinder(0.15f, 1.5f, 6);
            var crown = MakeCone(0.6f, 1.2f, 8);

            // Offset crown up
            var crownVerts = crown.vertices;
            for (int i = 0; i < crownVerts.Length; i++)
                crownVerts[i] += Vector3.up * 1.5f;
            crown.vertices = crownVerts;

            return CombineMeshes(trunk, crown);
        }

        /// <summary>Low-poly rock</summary>
        public static Mesh MakeRockMesh(float size = 1f)
        {
            // Icosahedron with noise displacement
            var mesh = MakeIcosphere(1, size * 0.5f);
            var verts = mesh.vertices;
            var rng = new System.Random(42);

            for (int i = 0; i < verts.Length; i++)
            {
                float noise = (float)(rng.NextDouble() * 0.3 - 0.15);
                verts[i] = verts[i].normalized * (verts[i].magnitude + noise * size);
            }
            mesh.vertices = verts;
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            return mesh;
        }

        /// <summary>Crystal shard</summary>
        public static Mesh MakeCrystalMesh()
        {
            // Elongated octahedron
            var verts = new Vector3[]
            {
                new(0, 1.5f, 0),    // top
                new(0.2f, 0, 0.2f),
                new(-0.2f, 0, 0.2f),
                new(-0.2f, 0, -0.2f),
                new(0.2f, 0, -0.2f),
                new(0, -0.3f, 0),   // bottom
            };

            var tris = new int[]
            {
                0,1,2, 0,2,3, 0,3,4, 0,4,1, // top
                5,2,1, 5,3,2, 5,4,3, 5,1,4, // bottom
            };

            var mesh = new Mesh { name = "Crystal" };
            mesh.vertices = verts;
            mesh.triangles = tris;
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            return mesh;
        }

        /// <summary>Grass blade</summary>
        public static Mesh MakeGrassMesh()
        {
            var verts = new Vector3[]
            {
                new(-0.05f, 0, 0),
                new(0.05f, 0, 0),
                new(0.03f, 0.4f, 0),
                new(-0.03f, 0.4f, 0),
                new(0f, 0.6f, 0),
            };

            var tris = new int[]
            {
                0,1,2, 0,2,3,
                3,2,4,
            };

            var mesh = new Mesh { name = "Grass" };
            mesh.vertices = verts;
            mesh.triangles = tris;
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            return mesh;
        }

        /// <summary>Wind bell stone — glowing sphere with ridges</summary>
        public static Mesh MakeWindBellStoneMesh()
        {
            var mesh = MakeIcosphere(2, 0.8f);
            var verts = mesh.vertices;

            // Add ridge pattern
            for (int i = 0; i < verts.Length; i++)
            {
                float ridge = Mathf.Sin(verts[i].x * 10f + verts[i].y * 5f) * 0.05f;
                verts[i] = verts[i].normalized * (verts[i].magnitude + ridge);
            }

            mesh.vertices = verts;
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            return mesh;
        }

        /// <summary>Beacon pillar</summary>
        public static Mesh MakeBeaconMesh()
        {
            return MakeCylinder(0.3f, 4f, 8);
        }

        /// <summary>Small creature body (capsule-like but cuter)</summary>
        public static Mesh MakeCreatureMesh()
        {
            // Stretched sphere with ears
            var body = MakeIcosphere(2, 0.4f);
            var verts = body.vertices;

            // Stretch vertically
            for (int i = 0; i < verts.Length; i++)
            {
                verts[i].y *= 1.3f;
            }
            body.vertices = verts;

            return body;
        }

        // === Primitive Generators ===

        public static Mesh MakeCylinder(float radius, float height, int segments)
        {
            var verts = new Vector3[segments * 2 + 2];
            var tris = new int[segments * 6];

            // Top and bottom centers
            verts[0] = Vector3.up * height * 0.5f;
            verts[1] = Vector3.down * height * 0.5f;

            for (int i = 0; i < segments; i++)
            {
                float angle = (float)i / segments * Mathf.PI * 2f;
                float x = Mathf.Cos(angle) * radius;
                float z = Mathf.Sin(angle) * radius;

                verts[2 + i] = new Vector3(x, height * 0.5f, z);
                verts[2 + segments + i] = new Vector3(x, -height * 0.5f, z);
            }

            int t = 0;
            for (int i = 0; i < segments; i++)
            {
                int next = (i + 1) % segments;
                // Top cap
                tris[t++] = 0;
                tris[t++] = 2 + i;
                tris[t++] = 2 + next;
                // Bottom cap
                tris[t++] = 1;
                tris[t++] = 2 + segments + next;
                tris[t++] = 2 + segments + i;
                // Side
                tris[t++] = 2 + i;
                tris[t++] = 2 + segments + i;
                tris[t++] = 2 + next;
                tris[t++] = 2 + next;
                tris[t++] = 2 + segments + i;
                tris[t++] = 2 + segments + next;
            }

            var mesh = new Mesh { name = "Cylinder" };
            mesh.vertices = verts;
            mesh.triangles = tris;
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            return mesh;
        }

        public static Mesh MakeCone(float radius, float height, int segments)
        {
            var verts = new Vector3[segments + 2];
            var tris = new int[segments * 3];

            verts[0] = Vector3.up * height; // tip
            verts[1] = Vector3.zero;        // base center

            for (int i = 0; i < segments; i++)
            {
                float angle = (float)i / segments * Mathf.PI * 2f;
                verts[2 + i] = new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);
            }

            int t = 0;
            for (int i = 0; i < segments; i++)
            {
                int next = (i + 1) % segments;
                tris[t++] = 0;
                tris[t++] = 2 + i;
                tris[t++] = 2 + next;
            }

            var mesh = new Mesh { name = "Cone" };
            mesh.vertices = verts;
            mesh.triangles = tris;
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            return mesh;
        }

        public static Mesh MakeIcosphere(int subdivisions, float radius)
        {
            // Start with icosahedron
            float t = (1f + Mathf.Sqrt(5f)) / 2f;

            var verts = new System.Collections.Generic.List<Vector3>
            {
                new(-1, t, 0).normalized * radius,
                new(1, t, 0).normalized * radius,
                new(-1, -t, 0).normalized * radius,
                new(1, -t, 0).normalized * radius,
                new(0, -1, t).normalized * radius,
                new(0, 1, t).normalized * radius,
                new(0, -1, -t).normalized * radius,
                new(0, 1, -t).normalized * radius,
                new(t, 0, -1).normalized * radius,
                new(t, 0, 1).normalized * radius,
                new(-t, 0, -1).normalized * radius,
                new(-t, 0, 1).normalized * radius,
            };

            var tris = new System.Collections.Generic.List<int>
            {
                0,11,5, 0,5,1, 0,1,7, 0,7,10, 0,10,11,
                1,5,9, 5,11,4, 11,10,2, 10,7,6, 7,1,8,
                3,9,4, 3,4,2, 3,2,6, 3,6,8, 3,8,9,
                4,9,5, 2,4,11, 6,2,10, 8,6,7, 9,8,1,
            };

            // Subdivide
            for (int s = 0; s < subdivisions; s++)
            {
                var newTris = new System.Collections.Generic.List<int>();
                var midCache = new System.Collections.Generic.Dictionary<long, int>();

                for (int i = 0; i < tris.Count; i += 3)
                {
                    int a = tris[i], b = tris[i + 1], c = tris[i + 2];
                    int ab = GetMidpoint(a, b, verts, midCache, radius);
                    int bc = GetMidpoint(b, c, verts, midCache, radius);
                    int ca = GetMidpoint(c, a, verts, midCache, radius);

                    newTris.AddRange(new[] { a, ab, ca, b, bc, ab, c, ca, bc, ab, bc, ca });
                }
                tris = newTris;
            }

            var mesh = new Mesh { name = "Icosphere" };
            mesh.SetVertices(verts);
            mesh.SetTriangles(tris, 0);
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            return mesh;
        }

        static int GetMidpoint(int a, int b, System.Collections.Generic.List<Vector3> verts,
            System.Collections.Generic.Dictionary<long, int> cache, float radius)
        {
            long key = ((long)Mathf.Min(a, b) << 32) | (long)Mathf.Max(a, b);
            if (cache.TryGetValue(key, out int idx)) return idx;

            Vector3 mid = ((verts[a] + verts[b]) * 0.5f).normalized * radius;
            idx = verts.Count;
            verts.Add(mid);
            cache[key] = idx;
            return idx;
        }

        public static Mesh CombineMeshes(Mesh a, Mesh b)
        {
            var verts = new Vector3[a.vertexCount + b.vertexCount];
            a.vertices.CopyTo(verts, 0);
            b.vertices.CopyTo(verts, a.vertexCount);

            var normals = new Vector3[a.vertexCount + b.vertexCount];
            a.normals.CopyTo(normals, 0);
            b.normals.CopyTo(normals, a.vertexCount);

            var tris = new int[a.triangles.Length + b.triangles.Length];
            a.triangles.CopyTo(tris, 0);
            var bTris = b.triangles;
            for (int i = 0; i < bTris.Length; i++)
                tris[a.triangles.Length + i] = bTris[i] + a.vertexCount;

            var mesh = new Mesh { name = "Combined" };
            mesh.vertices = verts;
            mesh.normals = normals;
            mesh.triangles = tris;
            mesh.RecalculateBounds();
            return mesh;
        }

        // === Prefab Builders (runtime) ===

        public static GameObject MakeTree(Vector3 position, Quaternion rotation, float scale = 1f)
        {
            var go = new GameObject("Tree");
            go.transform.position = position;
            go.transform.rotation = rotation;
            go.transform.localScale = Vector3.one * scale;

            var filter = go.AddComponent<MeshFilter>();
            filter.mesh = MakeTreeMesh();

            var renderer = go.AddComponent<MeshRenderer>();
            renderer.material = MakeMat(TreeGreen);

            // Trunk child
            var trunk = new GameObject("Trunk");
            trunk.transform.SetParent(go.transform, false);
            var trunkFilter = trunk.AddComponent<MeshFilter>();
            trunkFilter.mesh = MakeCylinder(0.15f, 1.5f, 6);
            var trunkRenderer = trunk.AddComponent<MeshRenderer>();
            trunkRenderer.material = MakeMat(TreeBrown);

            return go;
        }

        public static GameObject MakeRock(Vector3 position, float scale = 1f)
        {
            var go = new GameObject("Rock");
            go.transform.position = position;
            go.transform.localScale = Vector3.one * scale;

            var filter = go.AddComponent<MeshFilter>();
            filter.mesh = MakeRockMesh(scale);

            var renderer = go.AddComponent<MeshRenderer>();
            renderer.material = MakeMat(RockGray);

            go.AddComponent<MeshCollider>().sharedMesh = filter.mesh;

            return go;
        }

        public static GameObject MakeCrystal(Vector3 position, Color? color = null, float scale = 1f)
        {
            var go = new GameObject("Crystal");
            go.transform.position = position;
            go.transform.localScale = Vector3.one * scale;

            var filter = go.AddComponent<MeshFilter>();
            filter.mesh = MakeCrystalMesh();

            var renderer = go.AddComponent<MeshRenderer>();
            renderer.material = MakeEmissiveMat(color ?? CrystalBlue, 2f);

            return go;
        }

        public static GameObject MakeWindBellStone(Vector3 position, float scale = 1f)
        {
            var go = new GameObject("WindBellStone");
            go.transform.position = position;
            go.transform.localScale = Vector3.one * scale;

            var filter = go.AddComponent<MeshFilter>();
            filter.mesh = MakeWindBellStoneMesh();

            var renderer = go.AddComponent<MeshRenderer>();
            renderer.material = MakeEmissiveMat(WindBellGold, 3f);

            // Add trigger for interaction
            var trigger = go.AddComponent<SphereCollider>();
            trigger.isTrigger = true;
            trigger.radius = 1.5f;

            return go;
        }

        public static GameObject MakeBeacon(Vector3 position, Color? color = null, float scale = 1f)
        {
            var go = new GameObject("Beacon");
            go.transform.position = position;
            go.transform.localScale = Vector3.one * scale;

            var filter = go.AddComponent<MeshFilter>();
            filter.mesh = MakeBeaconMesh();

            var renderer = go.AddComponent<MeshRenderer>();
            renderer.material = MakeEmissiveMat(color ?? SunsetOrange, 2f);

            go.AddComponent<SphereCollider>().isTrigger = true;

            return go;
        }

        public static GameObject MakeCreature(Vector3 position, Color? color = null, float scale = 1f)
        {
            var go = new GameObject("Creature");
            go.transform.position = position;
            go.transform.localScale = Vector3.one * scale;

            var filter = go.AddComponent<MeshFilter>();
            filter.mesh = MakeCreatureMesh();

            var renderer = go.AddComponent<MeshRenderer>();
            renderer.material = MakeMat(color ?? ResidentWarm);

            // Eyes
            var leftEye = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            leftEye.transform.SetParent(go.transform, false);
            leftEye.transform.localPosition = new Vector3(-0.12f, 0.2f, 0.3f);
            leftEye.transform.localScale = Vector3.one * 0.15f;
            leftEye.GetComponent<MeshRenderer>().material = MakeMat(Color.white);
            Object.Destroy(leftEye.GetComponent<Collider>());

            var rightEye = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            rightEye.transform.SetParent(go.transform, false);
            rightEye.transform.localPosition = new Vector3(0.12f, 0.2f, 0.3f);
            rightEye.transform.localScale = Vector3.one * 0.15f;
            rightEye.GetComponent<MeshRenderer>().material = MakeMat(Color.white);
            Object.Destroy(rightEye.GetComponent<Collider>());

            // Pupils
            var leftPupil = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            leftPupil.transform.SetParent(leftEye.transform, false);
            leftPupil.transform.localPosition = new Vector3(0, 0, 0.5f);
            leftPupil.transform.localScale = Vector3.one * 0.5f;
            leftPupil.GetComponent<MeshRenderer>().material = MakeMat(new Color(0.1f, 0.1f, 0.15f));
            Object.Destroy(leftPupil.GetComponent<Collider>());

            var rightPupil = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            rightPupil.transform.SetParent(rightEye.transform, false);
            rightPupil.transform.localPosition = new Vector3(0, 0, 0.5f);
            rightPupil.transform.localScale = Vector3.one * 0.5f;
            rightPupil.GetComponent<MeshRenderer>().material = MakeMat(new Color(0.1f, 0.1f, 0.15f));
            Object.Destroy(rightPupil.GetComponent<Collider>());

            return go;
        }
    }
}
