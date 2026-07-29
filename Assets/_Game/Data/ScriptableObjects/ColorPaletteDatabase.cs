using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Color palette for Asteria's visual style.
    /// All colors follow the low-saturation NPR style from ART_STYLE_GUIDE.md.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Color Palette Database")]
    public sealed class ColorPaletteDatabase : ScriptableObject
    {
        [Header("Terrain Colors")]
        public Color grassGreen = new(0.45f, 0.62f, 0.48f);
        public Color warmGray = new(0.7f, 0.65f, 0.6f);
        public Color skyBlue = new(0.55f, 0.7f, 0.9f);
        public Color sunsetOrange = new(0.95f, 0.7f, 0.4f);
        public Color nightBlue = new(0.15f, 0.15f, 0.3f);
        public Color windBellGold = new(0.95f, 0.85f, 0.4f);
        public Color residentWarm = new(0.9f, 0.8f, 0.75f);
        public Color residentCool = new(0.7f, 0.8f, 0.9f);
        public Color crystalBlue = new(0.6f, 0.85f, 1f);
        public Color flowerPink = new(0.95f, 0.7f, 0.8f);
        public Color treeBrown = new(0.5f, 0.35f, 0.2f);
        public Color treeGreen = new(0.3f, 0.55f, 0.3f);
        public Color rockGray = new(0.55f, 0.5f, 0.45f);
        public Color snowWhite = new(0.92f, 0.92f, 0.95f);
        public Color sandYellow = new(0.85f, 0.75f, 0.5f);

        [Header("Biome Colors")]
        public Color biomeWind = new(0.5f, 0.7f, 0.5f);
        public Color biomeMist = new(0.3f, 0.5f, 0.3f);
        public Color biomeNight = new(0.2f, 0.2f, 0.4f);
        public Color biomeIce = new(0.7f, 0.8f, 0.9f);
        public Color biomeBloom = new(0.9f, 0.7f, 0.8f);
        public Color biomeRuin = new(0.5f, 0.5f, 0.5f);

        [Header("UI Colors")]
        public Color uiBackground = new(0.05f, 0.08f, 0.15f, 0.95f);
        public Color uiPanel = new(0.1f, 0.12f, 0.18f, 0.95f);
        public Color uiButton = new(0.3f, 0.5f, 0.7f);
        public Color uiText = Color.white;
        public Color uiAccent = new(0.95f, 0.85f, 0.4f);
        public Color uiSuccess = new(0.3f, 0.8f, 0.4f);
        public Color uiWarning = new(0.9f, 0.7f, 0.3f);
        public Color uiError = new(0.8f, 0.3f, 0.3f);

        [Header("Atmosphere Colors")]
        public Color atmosDay = new(0.4f, 0.6f, 0.9f, 0.3f);
        public Color atmosSunset = new(0.9f, 0.5f, 0.3f, 0.5f);
        public Color atmosNight = new(0.1f, 0.1f, 0.3f, 0.2f);
        public Color fogDay = new(0.55f, 0.68f, 0.82f);
        public Color fogNight = new(0.05f, 0.05f, 0.1f);
    }
}
