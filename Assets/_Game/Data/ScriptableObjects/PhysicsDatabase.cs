using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Physics configuration database.
    /// Contains all physics parameters for the game.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Physics Database")]
    public sealed class PhysicsDatabase : ScriptableObject
    {
        [Header("Gravity")]
        public float surfaceGravity = 9.81f;
        public float gravityFalloff = 2f;
        public float maxGravity = 20f;

        [Header("Player Physics")]
        public float playerMass = 80f;
        public float groundCheckDistance = 0.35f;
        public float groundedSkin = 0.08f;
        public float stepHeight = 0.3f;

        [Header("Wind")]
        public float windBaseStrength = 2f;
        public float windMaxStrength = 10f;
        public float windGustFrequency = 0.3f;

        [Header("Water")]
        public float waterLevel = 0.4f;
        public float buoyancyFactor = 0.5f;
        public float waterDrag = 0.1f;

        [Header("Collision")]
        public float collisionRadius = 0.5f;
        public float bounceForce = 5f;
        public float friction = 0.8f;
    }
}
