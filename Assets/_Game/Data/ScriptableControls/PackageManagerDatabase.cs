using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Package manager configuration database for the game.
    /// Contains all package references.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Package Manager Database")]
    public sealed class PackageManagerDatabase : ScriptableObject
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

        [Header("Editor Packages")]
        public string aiAssistant = "com.unity.ai.assistant@2.16.0-pre.1";
        public string aiInference = "com.unity.ai.inference@2.6.1";
    }
}
