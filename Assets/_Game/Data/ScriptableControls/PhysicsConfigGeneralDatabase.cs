using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// General physics configuration database for the game.
    /// Contains all physics parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Physics Config General Database")]
    public sealed class PhysicsConfigGeneralDatabase : ScriptableObject
    {
        [Header("Gravity")]
        public float surfaceGravity = 9.81f;
        public float gravityFalloff = 2f;
        public float maxGravity = 20f;

        [Header("Simulation")]
        public int physicsTickRate = 50;
        public int solverIterations = 6;

        [Header("Collision")]
        public float collisionRadius = 0.5f;
        public float bounceForce = 5f;
        public float friction = 0.8f;

        [Header("Wind")]
        public float windResistance = 0.1f;
        public float windPushbackForce = 2f;
    }
}
