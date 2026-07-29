using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// System requirement database for the game.
    /// Contains all system requirements for different platforms.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/System Requirement Database")]
    public sealed class SystemRequirementDatabase : ScriptableObject
    {
        [Header("Minimum Requirements")]
        public string minOS = "Windows 10 64-bit";
        public string minCPU = "Intel Core i5-4590 / AMD FX 8350";
        public int minRAM = 8;
        public string minGPU = "NVIDIA GTX 970 / AMD R9 290";
        public int minVRAM = 4;
        public int minStorage = 10; // GB

        [Header("Recommended Requirements")]
        public string recOS = "Windows 11 64-bit";
        public string recCPU = "Intel Core i7-10700 / AMD Ryzen 5 3600";
        public int recRAM = 16;
        public string recGPU = "NVIDIA RTX 3060 / AMD RX 6600";
        public int recVRAM = 8;
        public int recStorage = 20; // GB
    }
}
