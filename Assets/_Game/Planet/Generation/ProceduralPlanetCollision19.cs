using UnityEngine;

namespace Asteria.Planet.Generation
{
    /// <summary>
    /// Collision system with terrain adaptation.
    /// </summary>
    public sealed class ProceduralPlanetCollision19 : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float adaptationSpeed = 5f;

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
            AdaptToTerrain();
        }

        void AdaptToTerrain()
        {
            if (planet == null || target == null) return;

            Vector3 direction = (target.position - planet.Center).normalized;
            Vector3 surfacePoint = planet.GetPointOnSurface(direction, 1f);

            // Smoothly adapt position to terrain
            target.position = Vector3.Lerp(target.position, surfacePoint, Time.deltaTime * adaptationSpeed);

            // Adapt rotation to terrain normal
            Quaternion targetRot = Quaternion.FromToRotation(target.up, direction) * target.rotation;
            target.rotation = Quaternion.Slerp(target.rotation, targetRot, Time.deltaTime * adaptationSpeed);
        }
    }
}
