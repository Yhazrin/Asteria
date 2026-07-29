using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Movement configuration file database for the game.
    /// Contains all movement parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Movement Config File Database")]
    public sealed class MovementConfigFileDatabase : ScriptableObject
    {
        [Header("Walk")]
        public float walkSpeed = 8f;
        public float runSpeed = 14f;
        public float acceleration = 40f;
        public float airControl = 0.35f;

        [Header("Jump")]
        public float jumpSpeed = 7.5f;
        public float jumpCooldown = 0.2f;

        [Header("Rotation")]
        public float rotationSharpness = 12f;

        [Header("Grounding")]
        public float groundCheckDistance = 0.35f;
        public float groundedSkin = 0.08f;
        public LayerMask groundMask = -1;
    }
}
