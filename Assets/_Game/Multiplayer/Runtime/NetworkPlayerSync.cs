using UnityEngine;

namespace Asteria.Multiplayer
{
    /// <summary>
    /// Synchronizes player position and state across the network.
    /// Handles spherical position correction at poles.
    ///
    /// In a real NGO implementation, this would use NetworkTransform
    /// with custom spherical correction.
    /// </summary>
    public sealed class NetworkPlayerSync : MonoBehaviour
    {
        [Header("Sync Settings")]
        [SerializeField] float positionSyncRate = 20f;
        [SerializeField] float interpolationSpeed = 15f;
        [SerializeField] float positionThreshold = 0.01f;
        [SerializeField] float rotationThreshold = 1f;

        [Header("Spherical Correction")]
        [SerializeField] float poleCorrectionSpeed = 10f;
        [SerializeField] float poleThreshold = 85f; // degrees from equator

        // Network state
        string _playerId;
        bool _isLocalPlayer;
        bool _isOwner;

        // Interpolation
        Vector3 _targetPosition;
        Quaternion _targetRotation;
        Vector3 _networkVelocity;
        float _lastSyncTime;

        // Spherical
        Planet.PlanetBody _planet;

        public string PlayerId => _playerId;
        public bool IsLocalPlayer => _isLocalPlayer;
        public bool IsOwner => _isOwner;

        /// <summary>
        /// Initialize this network player.
        /// </summary>
        public void Initialize(string playerId, bool isLocalPlayer, bool isOwner)
        {
            _playerId = playerId;
            _isLocalPlayer = isLocalPlayer;
            _isOwner = isOwner;
            _targetPosition = transform.position;
            _targetRotation = transform.rotation;
        }

        /// <summary>
        /// Set the planet reference for spherical correction.
        /// </summary>
        public void SetPlanet(Planet.PlanetBody planet)
        {
            _planet = planet;
        }

        void Update()
        {
            if (_isLocalPlayer)
            {
                // Local player: send position to network
                if (Time.time - _lastSyncTime > 1f / positionSyncRate)
                {
                    SendPositionUpdate();
                    _lastSyncTime = Time.time;
                }
            }
            else
            {
                // Remote player: interpolate to target
                InterpolateToTarget();
            }
        }

        void SendPositionUpdate()
        {
            // In NGO, this would be a ServerRpc or NetworkVariable
            // For now, store locally
            var session = NetworkSessionManager.Instance;
            if (session != null && session.IsConnected)
            {
                // TODO: Send via NGO
                // NetworkManager.Singleton.LocalClient.PlayerObject
                //     .GetComponent<NetworkPlayerSync>()
                //     .UpdatePositionServerRpc(transform.position, transform.rotation);
            }
        }

        void InterpolateToTarget()
        {
            // Smooth interpolation
            transform.position = Vector3.Lerp(transform.position, _targetPosition,
                interpolationSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, _targetRotation,
                interpolationSpeed * Time.deltaTime);

            // Spherical correction near poles
            if (_planet != null)
            {
                CorrectPoleOrientation();
            }
        }

        void CorrectPoleOrientation()
        {
            Vector3 up = _planet.GetSurfaceUp(transform.position);
            float angleFromEquator = 90f - Vector3.Angle(up, Vector3.up);

            // Near poles: correct orientation to prevent flipping
            if (Mathf.Abs(angleFromEquator) > poleThreshold)
            {
                Quaternion upright = Quaternion.FromToRotation(transform.up, up) * transform.rotation;
                transform.rotation = Quaternion.Slerp(transform.rotation, upright,
                    poleCorrectionSpeed * Time.deltaTime);
            }

            // Re-project onto sphere surface
            Vector3 surfacePos = _planet.GetPointOnSurface(up, 1.05f);
            transform.position = Vector3.Lerp(transform.position, surfacePos,
                poleCorrectionSpeed * Time.deltaTime);
        }

        /// <summary>
        /// Called by network system when position update received.
        /// </summary>
        public void OnNetworkPositionUpdate(Vector3 position, Quaternion rotation, Vector3 velocity)
        {
            _targetPosition = position;
            _targetRotation = rotation;
            _networkVelocity = velocity;
        }

        /// <summary>
        /// Teleport to a position (used on reconnection).
        /// </summary>
        public void TeleportTo(Vector3 position, Quaternion rotation)
        {
            transform.position = position;
            transform.rotation = rotation;
            _targetPosition = position;
            _targetRotation = rotation;
        }
    }
}
