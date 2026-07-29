using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Shader file database for the game.
    /// Contains all shader references.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Shader File Database")]
    public sealed class ShaderFileDatabase : ScriptableObject
    {
        [Header("URP Shaders")]
        public string urpLitShader = "Universal Render Pipeline/Lit";
        public string urpSimpleLitShader = "Universal Render Pipeline/Simple Lit";
        public string urpUnlitShader = "Universal Render Pipeline/Unlit";

        [Header("Fallback")]
        public string fallbackShader = "Sprites/Default";
        public string standardShader = "Standard";

        [Header("Custom")]
        public string atmosphereShader = "";
        public string waterShader = "";
        public string terrainShader = "";
    }
}
