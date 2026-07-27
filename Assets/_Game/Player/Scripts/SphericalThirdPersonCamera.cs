using Asteria.Planet;
using UnityEngine;

namespace Asteria.Player
{
    /// <summary>
    /// Third-person orbit camera for a spherical world.
    /// Orbit yaw is independent of the player facing — critical to avoid
    /// the classic "WS spins in place" feedback loop with camera-relative movement.
    /// </summary>
    public sealed class SphericalThirdPersonCamera : MonoBehaviour
    {
        [SerializeField] Transform target;
        [SerializeField] PlanetBody planet;
        [SerializeField] float distance = 7f;
        [SerializeField] float minDistance = 2.5f;
        [SerializeField] float heightOffset = 1.4f;
        [SerializeField] float mouseSensitivity = 2.4f;
        [SerializeField] float minPitch = -30f;
        [SerializeField] float maxPitch = 70f;
        [SerializeField] float positionSharpness = 18f;
        [SerializeField] float collisionRadius = 0.25f;
        [SerializeField] LayerMask collisionMask = ~0;

        float _pitch = 12f;
        Vector3 _planarForward;
        bool _initialized;

        public Transform Target
        {
            get => target;
            set => target = value;
        }

        public PlanetBody Planet
        {
            get => planet;
            set => planet = value;
        }

        /// <summary>Camera look direction projected onto the local tangent plane.</summary>
        public Vector3 PlanarForward => _planarForward;

        /// <summary>Right axis on the local tangent plane.</summary>
        public Vector3 PlanarRight
        {
            get
            {
                Vector3 up = GetUp(target != null ? target.position : transform.position);
                Vector3 right = Vector3.Cross(up, _planarForward);
                return right.sqrMagnitude > 0.0001f ? right.normalized : Vector3.right;
            }
        }

        void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        void LateUpdate()
        {
            if (target == null)
            {
                return;
            }

            HandleCursor();

            Vector3 up = GetUp(target.position);
            EnsurePlanarForward(up);

            if (Cursor.lockState == CursorLockMode.Locked)
            {
                float yawDelta = Input.GetAxisRaw("Mouse X") * mouseSensitivity;
                float pitchDelta = Input.GetAxisRaw("Mouse Y") * mouseSensitivity;

                if (Mathf.Abs(yawDelta) > 0.0001f)
                {
                    _planarForward = Quaternion.AngleAxis(yawDelta, up) * _planarForward;
                    _planarForward = Vector3.ProjectOnPlane(_planarForward, up).normalized;
                }

                _pitch -= pitchDelta;
                _pitch = Mathf.Clamp(_pitch, minPitch, maxPitch);
            }

            // Transport the orbit frame as the player moves across the sphere.
            _planarForward = Vector3.ProjectOnPlane(_planarForward, up);
            if (_planarForward.sqrMagnitude < 0.0001f)
            {
                EnsurePlanarForward(up, force: true);
            }
            else
            {
                _planarForward.Normalize();
            }

            Vector3 pivot = target.position + up * heightOffset;
            Vector3 orbitRight = Vector3.Cross(up, _planarForward).normalized;
            Vector3 offsetDir = (Quaternion.AngleAxis(_pitch, orbitRight) * -_planarForward).normalized;

            float finalDistance = distance;
            if (Physics.SphereCast(
                    pivot,
                    collisionRadius,
                    offsetDir,
                    out RaycastHit hit,
                    distance,
                    collisionMask,
                    QueryTriggerInteraction.Ignore))
            {
                finalDistance = Mathf.Max(minDistance, hit.distance - collisionRadius);
            }

            Vector3 desiredPosition = pivot + offsetDir * finalDistance;
            float t = 1f - Mathf.Exp(-positionSharpness * Time.deltaTime);
            transform.position = Vector3.Lerp(transform.position, desiredPosition, t);

            Vector3 look = pivot - transform.position;
            if (look.sqrMagnitude > 0.0001f)
            {
                transform.rotation = Quaternion.LookRotation(look.normalized, up);
            }
        }

        void HandleCursor()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }

            if (Input.GetMouseButtonDown(0) && Cursor.lockState != CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        Vector3 GetUp(Vector3 worldPosition)
        {
            return planet != null ? planet.GetSurfaceUp(worldPosition) : Vector3.up;
        }

        void EnsurePlanarForward(Vector3 up, bool force = false)
        {
            if (_initialized && !force && _planarForward.sqrMagnitude > 0.0001f)
            {
                _planarForward = Vector3.ProjectOnPlane(_planarForward, up).normalized;
                if (_planarForward.sqrMagnitude > 0.0001f)
                {
                    return;
                }
            }

            Vector3 candidate = Vector3.ProjectOnPlane(transform.forward, up);
            if (candidate.sqrMagnitude < 0.0001f && target != null)
            {
                candidate = Vector3.ProjectOnPlane(target.forward, up);
            }

            if (candidate.sqrMagnitude < 0.0001f)
            {
                candidate = Vector3.ProjectOnPlane(Vector3.forward, up);
            }

            if (candidate.sqrMagnitude < 0.0001f)
            {
                candidate = Vector3.Cross(up, Vector3.right);
            }

            _planarForward = candidate.normalized;
            _initialized = true;
        }
    }
}
