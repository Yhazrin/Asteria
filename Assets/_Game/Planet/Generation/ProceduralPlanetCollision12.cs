using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Collision system with terrain following.
    /// </summary>
    public sealed class ProceduralPlanetCollision12 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float followHeight = 1f;
        [SerializeField] float followSpeed = 5f;

        [Header("References")]
        [SerializeField] PlanetBody planet;
        [SerializeField] Transform target;

        void Start()
        {
            if (planet == null)
                planet = FindFirstObjectByType<PlanetBody>();
        }

        void LateUpdate()
        {
            FollowTerrain();
        }

        void FollowTerrain()
        {
            if (planet == null || target == null) return;

            Vector3 direction = (target.position - planet.Center).normalized;
            Vector3 surfacePoint = planet.GetPointOnSurface(direction, followHeight);

            target.position = Vector3.Lerp(target.position, surfacePoint, Time.deltaTime * followSpeed);
            target.up = direction;
        }
    }
}
