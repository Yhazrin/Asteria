using Asteria.Planet;
using UnityEngine;

namespace Asteria.Player
{
    /// <summary>
    /// Disables world gravity and applies planet-centered acceleration.
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public sealed class SphericalGravityBody : MonoBehaviour
    {
        [SerializeField] PlanetBody planet;

        Rigidbody _body;

        public PlanetBody Planet
        {
            get => planet;
            set => planet = value;
        }

        public Vector3 SurfaceUp =>
            planet != null ? planet.GetSurfaceUp(_body.position) : Vector3.up;

        void Awake()
        {
            _body = GetComponent<Rigidbody>();
            _body.useGravity = false;
            _body.constraints = RigidbodyConstraints.FreezeRotation;
            _body.interpolation = RigidbodyInterpolation.Interpolate;
            _body.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        }

        void FixedUpdate()
        {
            if (planet == null)
            {
                return;
            }

            _body.AddForce(planet.GetGravityAcceleration(_body.position), ForceMode.Acceleration);
        }
    }
}
