using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Gravity configuration database for the game.
    /// Contains all gravity parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Gravity Config Database")]
    public sealed class GravityConfigDatabase : ScriptableObject
    {
        [Header("Gravity")]
        public float surfaceGravity = 9.81f;
        public float gravityFalloff = 2f;
        public float maxGravity = 20f;

        [Header("Falloff Curves")]
        public AnimationCurve gravityCurve = AnimationCurve.Linear(0, 1, 1, 0.1f);

        [Header("Special")]
        public bool enableAntiGravityZones = false;
        public float antiGravityStrength = 5f;
        public float antiGravityRadius = 30f;
    }
}
