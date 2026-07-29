using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// General camera database for the game.
    /// Contains all camera parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Camera General Database")]
    public sealed class CameraGeneralDatabase : ScriptableObject
    {
        [Header("Main Camera")]
        public float fieldOfView = 60f;
        public float nearClipPlane = 0.1f;
        public float farClipPlane = 2500f;
        public CameraClearFlags clearFlags = CameraClearFlags.Skybox;

        [Header("Third Person")]
        public float cameraDistance = 7f;
        public float cameraMinDistance = 2.5f;
        public float cameraHeightOffset = 1.4f;
        public float mouseSensitivity = 2.4f;
        public float minPitch = -30f;
        public float maxPitch = 70f;
        public float positionSharpness = 18f;

        [Header("Collision")]
        public float collisionRadius = 0.25f;
        public LayerMask collisionMask = -1;
    }
}
