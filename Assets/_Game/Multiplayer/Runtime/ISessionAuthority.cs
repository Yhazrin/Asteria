using UnityEngine;

namespace Asteria.Multiplayer
{
    /// <summary>
    /// Abstracts session authority. Single-player uses local authority;
    /// multiplayer uses host authority. Gameplay code never touches NGO directly.
    /// </summary>
    public interface ISessionAuthority
    {
        bool IsHost { get; }
        bool IsConnected { get; }
        string LocalPlayerId { get; }

        /// <summary>
        /// Request an interaction authority check. Returns true if approved.
        /// </summary>
        bool RequestInteraction(string interactionId, string playerId);

        /// <summary>
        /// Broadcast an event to all connected players.
        /// </summary>
        void BroadcastEvent(string eventId, string data);

        /// <summary>
        /// Get the current authoritative state snapshot for reconnection.
        /// </summary>
        SessionSnapshot GetSnapshot();

        /// <summary>
        /// Restore from a snapshot (used on reconnection).
        /// </summary>
        void RestoreFromSnapshot(SessionSnapshot snapshot);
    }

    public class SessionSnapshot
    {
        public string expeditionId;
        public string phase;
        public float elapsedTime;
        public string[] playerIds;
        public Vector3[] playerPositions;
        public string[] discoveredIds;
    }
}
