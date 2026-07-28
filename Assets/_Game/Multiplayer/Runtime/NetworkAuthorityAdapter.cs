using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Multiplayer
{
    /// <summary>
    /// Network implementation of ISessionAuthority.
    /// Uses NetworkSessionManager, NetworkInteractionAuthority, and NetworkCheckpoint.
    /// </summary>
    public sealed class NetworkAuthorityAdapter : MonoBehaviour, ISessionAuthority
    {
        NetworkSessionManager _sessionManager;
        NetworkInteractionAuthority _interactionAuthority;
        NetworkCheckpoint _checkpoint;

        void Awake()
        {
            _sessionManager = NetworkSessionManager.Instance;
            _interactionAuthority = GetComponent<NetworkInteractionAuthority>();
            _checkpoint = GetComponent<NetworkCheckpoint>();

            // Auto-create if missing
            if (_interactionAuthority == null)
                _interactionAuthority = gameObject.AddComponent<NetworkInteractionAuthority>();
            if (_checkpoint == null)
                _checkpoint = gameObject.AddComponent<NetworkCheckpoint>();
        }

        public bool IsHost => _sessionManager?.IsHost ?? true;
        public bool IsConnected => _sessionManager?.IsConnected ?? false;
        public string LocalPlayerId => _sessionManager?.LocalPlayerId ?? "local";

        public bool RequestInteraction(string interactionId, string playerId)
        {
            if (_interactionAuthority != null)
            {
                return _interactionAuthority.RequestInteraction(interactionId, playerId);
            }
            return true; // Fallback: approve
        }

        public void BroadcastEvent(string eventId, string data)
        {
            if (_sessionManager != null && _sessionManager.IsConnected)
            {
                // In NGO: ClientRpc
                Debug.Log($"[Network] Broadcasting: {eventId}");
            }
        }

        public SessionSnapshot GetSnapshot()
        {
            if (_checkpoint != null)
            {
                var cp = _checkpoint.TakeCheckpoint();
                return new SessionSnapshot
                {
                    expeditionId = cp.expeditionId,
                    phase = "expedition",
                    elapsedTime = cp.timestamp,
                    playerIds = System.Array.ConvertAll(cp.playerStates, p => p.playerId),
                    playerPositions = System.Array.ConvertAll(cp.playerStates, p => p.position),
                    discoveredIds = cp.discoveryIds
                };
            }

            return _sessionManager?.TakeSnapshot() ?? new SessionSnapshot();
        }

        public void RestoreFromSnapshot(SessionSnapshot snapshot)
        {
            if (snapshot == null) return;

            _sessionManager?.RestoreFromSnapshot(snapshot);

            // Restore checkpoint
            if (_checkpoint != null)
            {
                var checkpointData = new NetworkCheckpoint.CheckpointData
                {
                    expeditionId = snapshot.expeditionId,
                    timestamp = snapshot.elapsedTime,
                    playerStates = new NetworkCheckpoint.PlayerStateData[snapshot.playerIds.Length],
                    discoveryIds = snapshot.discoveredIds
                };

                for (int i = 0; i < snapshot.playerIds.Length; i++)
                {
                    checkpointData.playerStates[i] = new NetworkCheckpoint.PlayerStateData
                    {
                        playerId = snapshot.playerIds[i],
                        position = snapshot.playerPositions[i],
                        rotation = Quaternion.identity
                    };
                }

                _checkpoint.RestoreCheckpoint(checkpointData);
            }

            Debug.Log($"[Network] Restored from snapshot: {snapshot.expeditionId}");
        }
    }
}
