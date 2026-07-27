using UnityEngine;

namespace Asteria.Planet
{
    /// <summary>
    /// Defines a spherical world body. Gravity pulls toward the center;
    /// surface "up" points outward from the center.
    /// </summary>
    public sealed class PlanetBody : MonoBehaviour
    {
        [SerializeField] float radius = 300f;
        [SerializeField] float gravityStrength = 9.81f;

        public float Radius => radius;
        public float GravityStrength => gravityStrength;
        public Vector3 Center => transform.position;

        public void Configure(float surfaceRadius, float gravity)
        {
            radius = Mathf.Max(1f, surfaceRadius);
            gravityStrength = Mathf.Max(0.01f, gravity);
        }

        public Vector3 GetSurfaceUp(Vector3 worldPosition)
        {
            Vector3 fromCenter = worldPosition - Center;
            return fromCenter.sqrMagnitude > 0.0001f
                ? fromCenter.normalized
                : transform.up;
        }

        public Vector3 GetGravityAcceleration(Vector3 worldPosition)
        {
            return -GetSurfaceUp(worldPosition) * gravityStrength;
        }

        public Vector3 GetPointOnSurface(Vector3 directionFromCenter, float heightOffset = 0f)
        {
            return Center + directionFromCenter.normalized * (radius + heightOffset);
        }

        public void AlignTransformToSurface(Transform target, Vector3 preferredForward)
        {
            Vector3 up = GetSurfaceUp(target.position);
            Vector3 forward = Vector3.ProjectOnPlane(preferredForward, up);
            if (forward.sqrMagnitude < 0.0001f)
            {
                forward = Vector3.ProjectOnPlane(target.forward, up);
            }

            if (forward.sqrMagnitude < 0.0001f)
            {
                forward = Vector3.Cross(up, Vector3.right);
                if (forward.sqrMagnitude < 0.0001f)
                {
                    forward = Vector3.Cross(up, Vector3.forward);
                }
            }

            target.rotation = Quaternion.LookRotation(forward.normalized, up);
        }

#if UNITY_EDITOR
        void OnValidate()
        {
            radius = Mathf.Max(1f, radius);
            gravityStrength = Mathf.Max(0.01f, gravityStrength);
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(0.4f, 0.75f, 1f, 0.35f);
            Gizmos.DrawWireSphere(Center, radius);
        }
#endif
    }
}
