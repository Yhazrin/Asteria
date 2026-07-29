using UnityEngine;

namespace Asteria.Art
{
    /// <summary>
    /// Pre-built material library for Asteria's visual style.
    /// All materials generated procedurally with consistent style.
    /// </summary>
    public static class MaterialLibrary
    {
        // Cached materials
        static Material _terrainGrass;
        static Material _terrainRock;
        static Material _terrainSnow;
        static Material _terrainSand;
        static Material _treeTrunk;
        static Material _treeLeaves;
        static Material _crystal;
        static Material _windBell;
        static Material _beacon;
        static Material _resident;
        static Material _creature;
        static Material _water;
        static Material _atmosphere;
        static Material _cloud;

        public static Material TerrainGrass => _terrainGrass ??= MakeTerrainGrass();
        public static Material TerrainRock => _terrainRock ??= MakeTerrainRock();
        public static Material TerrainSnow => _terrainSnow ??= MakeTerrainSnow();
        public static Material TerrainSand => _terrainSand ??= MakeTerrainSand();
        public static Material TreeTrunk => _treeTrunk ??= MakeTreeTrunk();
        public static Material TreeLeaves => _treeLeaves ??= MakeTreeLeaves();
        public static Material Crystal => _crystal ??= MakeCrystal();
        public static Material WindBell => _windBell ??= MakeWindBell();
        public static Material Beacon => _beacon ??= MakeBeacon();
        public static Material Resident => _resident ??= MakeResident();
        public static Material Creature => _creature ??= MakeCreature();
        public static Material Water => _water ??= MakeWater();
        public static Material Atmosphere => _atmosphere ??= MakeAtmosphere();
        public static Material Cloud => _cloud ??= MakeCloud();

        static Material BaseLit
        {
            get
            {
                var shader = Shader.Find("Universal Render Pipeline/Lit")
                    ?? Shader.Find("Universal Render Pipeline/Simple Lit")
                    ?? Shader.Find("Standard");
                return new Material(shader);
            }
        }

        static Material BaseUnlit
        {
            get
            {
                var shader = Shader.Find("Universal Render Pipeline/Unlit")
                    ?? Shader.Find("Sprites/Default");
                return new Material(shader);
            }
        }

        static Material MakeTerrainGrass()
        {
            var mat = BaseLit;
            mat.name = "M_Terrain_Grass";
            SetColor(mat, new Color(0.45f, 0.62f, 0.48f));
            SetFloat(mat, "_Smoothness", 0.15f);
            SetFloat(mat, "_Metallic", 0f);
            return mat;
        }

        static Material MakeTerrainRock()
        {
            var mat = BaseLit;
            mat.name = "M_Terrain_Rock";
            SetColor(mat, new Color(0.55f, 0.5f, 0.45f));
            SetFloat(mat, "_Smoothness", 0.1f);
            SetFloat(mat, "_Metallic", 0.05f);
            return mat;
        }

        static Material MakeTerrainSnow()
        {
            var mat = BaseLit;
            mat.name = "M_Terrain_Snow";
            SetColor(mat, new Color(0.92f, 0.92f, 0.95f));
            SetFloat(mat, "_Smoothness", 0.3f);
            SetFloat(mat, "_Metallic", 0f);
            return mat;
        }

        static Material MakeTerrainSand()
        {
            var mat = BaseLit;
            mat.name = "M_Terrain_Sand";
            SetColor(mat, new Color(0.85f, 0.75f, 0.5f));
            SetFloat(mat, "_Smoothness", 0.1f);
            SetFloat(mat, "_Metallic", 0f);
            return mat;
        }

        static Material MakeTreeTrunk()
        {
            var mat = BaseLit;
            mat.name = "M_Tree_Trunk";
            SetColor(mat, new Color(0.5f, 0.35f, 0.2f));
            SetFloat(mat, "_Smoothness", 0.1f);
            return mat;
        }

        static Material MakeTreeLeaves()
        {
            var mat = BaseLit;
            mat.name = "M_Tree_Leaves";
            SetColor(mat, new Color(0.3f, 0.55f, 0.3f));
            SetFloat(mat, "_Smoothness", 0.15f);
            return mat;
        }

        static Material MakeCrystal()
        {
            var mat = BaseLit;
            mat.name = "M_Crystal";
            SetColor(mat, new Color(0.6f, 0.85f, 1f));
            SetFloat(mat, "_Smoothness", 0.6f);
            SetFloat(mat, "_Metallic", 0.3f);
            EnableEmission(mat, new Color(0.4f, 0.7f, 1f), 2f);
            return mat;
        }

        static Material MakeWindBell()
        {
            var mat = BaseLit;
            mat.name = "M_WindBell";
            SetColor(mat, new Color(0.95f, 0.85f, 0.4f));
            SetFloat(mat, "_Smoothness", 0.4f);
            SetFloat(mat, "_Metallic", 0.2f);
            EnableEmission(mat, new Color(0.9f, 0.8f, 0.3f), 3f);
            return mat;
        }

        static Material MakeBeacon()
        {
            var mat = BaseLit;
            mat.name = "M_Beacon";
            SetColor(mat, new Color(0.95f, 0.7f, 0.3f));
            SetFloat(mat, "_Smoothness", 0.3f);
            SetFloat(mat, "_Metallic", 0.1f);
            EnableEmission(mat, new Color(0.9f, 0.6f, 0.2f), 2f);
            return mat;
        }

        static Material MakeResident()
        {
            var mat = BaseLit;
            mat.name = "M_Resident";
            SetColor(mat, new Color(0.9f, 0.8f, 0.75f));
            SetFloat(mat, "_Smoothness", 0.25f);
            return mat;
        }

        static Material MakeCreature()
        {
            var mat = BaseLit;
            mat.name = "M_Creature";
            SetColor(mat, new Color(0.8f, 0.75f, 0.7f));
            SetFloat(mat, "_Smoothness", 0.3f);
            return mat;
        }

        static Material MakeWater()
        {
            var mat = BaseLit;
            mat.name = "M_Water";
            SetColor(mat, new Color(0.3f, 0.5f, 0.7f, 0.6f));
            SetFloat(mat, "_Smoothness", 0.8f);
            SetFloat(mat, "_Metallic", 0.1f);
            SetRenderQueue(mat, 3000); // Transparent
            return mat;
        }

        static Material MakeAtmosphere()
        {
            var mat = BaseUnlit;
            mat.name = "M_Atmosphere";
            SetColor(mat, new Color(0.4f, 0.6f, 0.9f, 0.3f));
            SetRenderQueue(mat, 3100);
            return mat;
        }

        static Material MakeCloud()
        {
            var mat = BaseLit;
            mat.name = "M_Cloud";
            SetColor(mat, new Color(0.95f, 0.95f, 0.98f, 0.8f));
            SetFloat(mat, "_Smoothness", 0.1f);
            SetRenderQueue(mat, 3000);
            return mat;
        }

        // Helper methods
        static void SetColor(Material mat, Color color)
        {
            if (mat.HasProperty("_BaseColor")) mat.SetColor("_BaseColor", color);
            if (mat.HasProperty("_Color")) mat.SetColor("_Color", color);
            mat.color = color;
        }

        static void SetFloat(Material mat, string name, float value)
        {
            if (mat.HasProperty(name)) mat.SetFloat(name, value);
        }

        static void EnableEmission(Material mat, Color color, float intensity)
        {
            if (mat.HasProperty("_EmissionColor"))
            {
                mat.EnableKeyword("_EMISSION");
                mat.SetColor("_EmissionColor", color * intensity);
            }
        }

        static void SetRenderQueue(Material mat, int queue)
        {
            mat.renderQueue = queue;
        }
    }
}
