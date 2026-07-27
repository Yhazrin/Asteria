using Asteria.Data;
using Asteria.Planet;
using UnityEngine;

namespace Asteria.Player
{
    /// <summary>
    /// WASD / run / jump movement on a spherical surface.
    /// Movement axes come from the camera's independent planar frame
    /// (not from the player's facing) to avoid orbit feedback spin.
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(SphericalGravityBody))]
    public sealed class SphericalMotor : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField] PlayerMotorConfig config;

        [Header("Movement (fallback if no config)")]
        [SerializeField] float walkSpeed = 8f;
        [SerializeField] float runSpeed = 14f;
        [SerializeField] float acceleration = 40f;
        [SerializeField] float airControl = 0.35f;
        [SerializeField] float jumpSpeed = 7.5f;
        [SerializeField] float rotationSharpness = 12f;

        [Header("Grounding")]
        [SerializeField] float groundCheckDistance = 0.35f;
        [SerializeField] float groundedSkin = 0.08f;
        [SerializeField] LayerMask groundMask = ~0;

        [Header("References")]
        [SerializeField] Transform cameraTransform;
        [SerializeField] SphericalGravityBody gravityBody;

        Rigidbody _body;
        SphericalThirdPersonCamera _orbitCamera;
        bool _jumpRequested;
        bool _isGrounded;

        public bool IsGrounded => _isGrounded;

        public void SetCamera(Transform cam)
        {
            cameraTransform = cam;
            _orbitCamera = cam != null ? cam.GetComponent<SphericalThirdPersonCamera>() : null;
        }

        public void SetConfig(PlayerMotorConfig motorConfig)
        {
            config = motorConfig;
        }

        float WalkSpeed => config != null ? config.walkSpeed : walkSpeed;
        float RunSpeed => config != null ? config.runSpeed : runSpeed;
        float Acceleration => config != null ? config.acceleration : acceleration;
        float AirControl => config != null ? config.airControl : airControl;
        float JumpSpeed => config != null ? config.jumpSpeed : jumpSpeed;
        float RotationSharpness => config != null ? config.rotationSharpness : rotationSharpness;
        float GroundCheckDistance => config != null ? config.groundCheckDistance : groundCheckDistance;
        float GroundedSkin => config != null ? config.groundedSkin : groundedSkin;

        void Awake()
        {
            _body = GetComponent<Rigidbody>();
            if (gravityBody == null)
            {
                gravityBody = GetComponent<SphericalGravityBody>();
            }

            if (cameraTransform != null)
            {
                _orbitCamera = cameraTransform.GetComponent<SphericalThirdPersonCamera>();
            }
        }

        void Update()
        {
            if (Input.GetButtonDown("Jump"))
            {
                _jumpRequested = true;
            }
        }

        void FixedUpdate()
        {
            PlanetBody planet = gravityBody != null ? gravityBody.Planet : null;
            if (planet == null)
            {
                return;
            }

            Vector3 up = planet.GetSurfaceUp(_body.position);
            UpdateGrounded(up);

            Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            moveInput = Vector2.ClampMagnitude(moveInput, 1f);
            bool running = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
            float targetSpeed = running ? RunSpeed : WalkSpeed;

            Vector3 moveDir = GetTangentMoveDirection(up, moveInput);
            Vector3 currentVelocity = _body.linearVelocity;
            Vector3 verticalVelocity = Vector3.Project(currentVelocity, up);
            Vector3 tangentialVelocity = currentVelocity - verticalVelocity;

            float control = _isGrounded ? 1f : AirControl;
            Vector3 desiredTangential = moveDir * (targetSpeed * moveInput.magnitude);
            Vector3 newTangential = Vector3.MoveTowards(
                tangentialVelocity,
                desiredTangential,
                Acceleration * control * Time.fixedDeltaTime);

            if (_jumpRequested)
            {
                if (_isGrounded)
                {
                    verticalVelocity = up * JumpSpeed;
                }

                _jumpRequested = false;
            }

            _body.linearVelocity = newTangential + verticalVelocity;

            // Face movement direction while moving; otherwise stay upright on the sphere.
            Vector3 faceDir = moveDir.sqrMagnitude > 0.001f
                ? moveDir
                : Vector3.ProjectOnPlane(transform.forward, up);

            if (faceDir.sqrMagnitude > 0.001f)
            {
                Quaternion targetRot = Quaternion.LookRotation(faceDir.normalized, up);
                _body.MoveRotation(Quaternion.Slerp(
                    _body.rotation,
                    targetRot,
                    1f - Mathf.Exp(-RotationSharpness * Time.fixedDeltaTime)));
            }
            else
            {
                Quaternion upright = Quaternion.FromToRotation(transform.up, up) * _body.rotation;
                _body.MoveRotation(Quaternion.Slerp(
                    _body.rotation,
                    upright,
                    1f - Mathf.Exp(-RotationSharpness * Time.fixedDeltaTime)));
            }
        }

        Vector3 GetTangentMoveDirection(Vector3 up, Vector2 moveInput)
        {
            if (moveInput.sqrMagnitude < 0.0001f)
            {
                return Vector3.zero;
            }

            Vector3 camForward;
            Vector3 camRight;

            if (_orbitCamera == null && cameraTransform != null)
            {
                _orbitCamera = cameraTransform.GetComponent<SphericalThirdPersonCamera>();
            }

            if (_orbitCamera != null)
            {
                // Use the camera's independent orbit frame — not transform.forward.
                camForward = Vector3.ProjectOnPlane(_orbitCamera.PlanarForward, up);
                camRight = Vector3.ProjectOnPlane(_orbitCamera.PlanarRight, up);
            }
            else
            {
                Transform reference = cameraTransform != null ? cameraTransform : transform;
                camForward = Vector3.ProjectOnPlane(reference.forward, up);
                camRight = Vector3.ProjectOnPlane(reference.right, up);
            }

            if (camForward.sqrMagnitude < 0.0001f)
            {
                camForward = Vector3.ProjectOnPlane(Vector3.forward, up);
            }

            if (camForward.sqrMagnitude < 0.0001f)
            {
                camForward = Vector3.Cross(up, Vector3.right);
            }

            camForward.Normalize();

            if (camRight.sqrMagnitude < 0.0001f)
            {
                camRight = Vector3.Cross(up, camForward);
            }

            camRight.Normalize();

            return (camRight * moveInput.x + camForward * moveInput.y).normalized;
        }

        void UpdateGrounded(Vector3 up)
        {
            Vector3 origin = _body.position + up * GroundedSkin;
            _isGrounded = Physics.SphereCast(
                origin,
                0.35f,
                -up,
                out _,
                GroundCheckDistance + 0.15f,
                groundMask,
                QueryTriggerInteraction.Ignore);
        }
    }
}
