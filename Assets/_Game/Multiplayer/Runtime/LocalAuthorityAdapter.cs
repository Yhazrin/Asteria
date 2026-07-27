using UnityEngine;

namespace Asteria.Multiplayer
{
    /// <summary>
    /// Local (single-player) implementation of ISessionAuthority.
    /// All authority checks pass locally. Used when no network is active.
    /// </summary>
    public sealed class LocalAuthorityAdapter : ISessionAuthority
    {
        readonly string _playerId = "local_player";

        public bool IsHost => true;
        public bool IsConnected => false;
        public string LocalPlayerId => _playerId;

        public bool RequestInteraction(string interactionId, string playerId)
        {
            // Local mode: always approve
            return true;
        }

        public void BroadcastEvent(string eventId, string data)
        {
            // Local mode: no-op
            Debug.Log($"[Asteria] Local event: {eventId}");
        }

        public SessionSnapshot GetSnapshot()
        {
            return new SessionSnapshot
            {
                expeditionId = "local",
                phase = "local",
                elapsedTime = 0f,
                playerIds = new[] { _playerId },
                playerPositions = new[] { Vector3.zero },
                discoveredIds = System.Array.Empty<string>()
            };
        }

        public void RestoreFromSnapshot(SessionSnapshot snapshot)
        {
            // Local mode: no-op
        }
    }
}
