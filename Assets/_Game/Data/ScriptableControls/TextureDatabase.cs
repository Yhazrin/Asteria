using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Texture configuration database for the game.
    /// Contains all texture references.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Texture Database")]
    public sealed class TextureDatabase : ScriptableObject
    {
        [Header("Terrain")]
        public Texture2D grassTexture;
        public Texture2D rockTexture;
        public Texture2D snowTexture;
        public Texture2D sandTexture;

        [Header("Noise")]
        public Texture2D noiseTexture;
        public Texture2D perlinNoise;
        public Texture2D simplexNoise;

        [Header("UI")]
        public Texture2D buttonNormal;
        public Texture2D buttonHover;
        public Texture2D buttonPressed;

        [Header("Icons")]
        public Texture2D[] toolIcons;
        public Texture2D[] statusIcons;
        public Texture2D[] biomeIcons;
    }
}
