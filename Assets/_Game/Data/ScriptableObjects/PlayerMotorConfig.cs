using UnityEngine;

namespace Asteria.Data
{
    [CreateAssetMenu(fileName = "PlayerMotorConfig", menuName = "Asteria/Config/Player Motor")]
    public sealed class PlayerMotorConfig : ScriptableObject
    {
        [Header("Movement")]
        public float walkSpeed = 8f;
        public float runSpeed = 14f;
        public float acceleration = 40f;
        public float airControl = 0.35f;
        public float jumpSpeed = 7.5f;
        public float rotationSharpness = 12f;

        [Header("Grounding")]
        public float groundCheckDistance = 0.35f;
        public float groundedSkin = 0.08f;
    }
}
