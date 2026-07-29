using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// General LOD configuration database for the game.
    /// Contains all LOD parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/LOD Config General Database")]
    public sealed class LODConfigGeneralDatabase : ScriptableObject
    {
        [Header("LOD Settings")]
        public int maxLODLevels = 4;
        public float[] lodDistances = { 100f, 200f, 400f, 800f };
        public float lodBias = 1f;

        [Header("LOD Resolutions")]
        public int[] lodResolutions = { 128, 64, 32, 16 };

        [Header("Transitions")]
        public float lodTransitionSpeed = 2f;
        public bool enableLODTransitions = true;
    }
}
