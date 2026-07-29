using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Input configuration database.
    /// Contains input sensitivity and dead zone settings.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Input Database")]
    public sealed class InputDatabase : ScriptableObject
    {
        [Header("Mouse")]
        public float mouseSensitivity = 2.4f;
        public bool invertY = false;
        public float mouseSmoothing = 0.1f;
        public float mouseDeadZone = 0.01f;

        [Header("Gamepad")]
        public float gamepadSensitivity = 1f;
        public bool gamepadInvertY = false;
        public float gamepadDeadZone = 0.15f;
        public float gamepadTriggerThreshold = 0.5f;

        [Header("Movement")]
        public float walkSpeed = 8f;
        public float runSpeed = 14f;
        public float jumpForce = 7.5f;
        public float airControl = 0.35f;
        public float rotationSharpness = 12f;

        [Header("Camera")]
        public float cameraDistance = 7f;
        public float cameraMinDistance = 2.5f;
        public float cameraHeightOffset = 1.4f;
        public float cameraMinPitch = -30f;
        public float cameraMaxPitch = 70f;
        public float cameraPositionSharpness = 18f;
    }
}
