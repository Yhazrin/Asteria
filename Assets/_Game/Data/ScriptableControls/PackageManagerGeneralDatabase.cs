using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// General package manager database for the game.
    /// Contains all package references.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Package Manager General Database")]
    public sealed class PackageManagerGeneralDatabase : ScriptableObject
    {
        [Header("Required Packages")]
        public string urpPackage = "com.unity.render-pipelines.universal@17.5.0";
        public string textMeshPro = "com.unity.textmeshpro@3.2.0";

        [Header("Optional Packages")]
        public string inputSystem = "com.unity.inputsystem@1.7.0";
        public string proBuilder = "com.unity.probuilder@5.2.0";
        public string shaderGraph = "com.unity.shadergraph@17.5.0";
        public string vfxGraph = "com.unity.visualeffectgraph@17.5.0";

        [Header("Network Packages")]
        public string netcode = "com.unity.netcode.gameobjects@2.0.0";
        public string relay = "com.unity.services.relay@2.0.0";
        public string lobby = "com.unity.services.lobby@2.0.0";
    }
}
