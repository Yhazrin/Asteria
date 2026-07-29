using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// General cloud configuration database for the game.
    /// Contains all cloud parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Cloud Config General Database")]
    public sealed class CloudConfigGeneralDatabase : ScriptableObject
    {
        [Header("Clouds")]
        public int cloudCount = 20;
        public float cloudAltitude = 50f;
        public float cloudScale = 20f;
        public Color cloudColor = new(0.95f, 0.95f, 0.98f, 0.6f);

        [Header("Movement")]
        public float moveSpeed = 2f;
        public float rotationSpeed = 0.1f;

        [Header("Generation")]
        public int minBlobs = 3;
        public int maxBlobs = 6;
        public float blobMinRadius = 0.3f;
        public float blobMaxRadius = 0.6f;
    }
}
