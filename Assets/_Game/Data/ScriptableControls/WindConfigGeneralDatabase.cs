using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// General wind configuration database for the game.
    /// Contains all wind parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Wind Config General Database")]
    public sealed class WindConfigGeneralDatabase : ScriptableObject
    {
        [Header("Wind")]
        public float windStrength = 5f;
        public float windVariation = 2f;
        public float gustFrequency = 0.3f;
        public float gustStrength = 3f;

        [Header("Direction")]
        public float windDirectionX = 1f;
        public float windDirectionZ = 0f;
        public float directionVariation = 30f;

        [Header("Effects")]
        public float grassBendAmount = 0.3f;
        public float particleDriftSpeed = 5f;
        public float playerPushbackForce = 2f;
    }
}
