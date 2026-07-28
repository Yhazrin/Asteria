using System.Collections;
using UnityEngine;

namespace Asteria.Multiplayer
{
    /// <summary>
    /// Handles player disconnection and reconnection.
    /// Manages the 30-second reconnect window and state restoration.
    /// </summary>
    public sealed class ReconnectionHandler : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float reconnectWindow = 30f;
        [SerializeField] float reconnectCheckInterval = 1f;

        readonly System.Collections.Generic.Dictionary<string, ReconnectState> _disconnectedPlayers = new();

        /// <summary>
        /// Called when a player disconnects.
        /// </summary>
        public void OnPlayerDisconnected(string playerId)
        {
            var session = NetworkSessionManager.Instance;
            if (session == null || !session.IsHost) return;

            _disconnectedPlayers[playerId] = new ReconnectState
            {
                playerId = playerId,
                disconnectTime = Time.time,
                lastCheckpoint = FindFirstObjectByType<NetworkCheckpoint>()?.TakeCheckpoint()
            };

            Debug.Log($"[Network] Player disconnected: {playerId}. Reconnect window: {reconnectWindow}s");

            // Start countdown
            StartCoroutine(ReconnectCountdown(playerId));
        }

        /// <summary>
        /// Called when a player reconnects.
        /// </summary>
        public bool OnPlayerReconnected(string playerId)
        {
            if (!_disconnectedPlayers.TryGetValue(playerId, out var state))
            {
                Debug.LogWarning($"[Network] No reconnect state for player: {playerId}");
                return false;
            }

            // Restore from checkpoint
            if (state.lastCheckpoint != null)
            {
                var checkpoint = FindFirstObjectByType<NetworkCheckpoint>();
                if (checkpoint != null)
                {
                    checkpoint.RestoreCheckpoint(state.lastCheckpoint);
                }
            }

            _disconnectedPlayers.Remove(playerId);
            Debug.Log($"[Network] Player reconnected: {playerId}");
            return true;
        }

        /// <summary>
        /// Check if a player is in the reconnect window.
        /// </summary>
        public bool IsInReconnectWindow(string playerId)
        {
            return _disconnectedPlayers.ContainsKey(playerId);
        }

        /// <summary>
        /// Get remaining reconnect time for a player.
        /// </summary>
        public float GetRemainingReconnectTime(string playerId)
        {
            if (!_disconnectedPlayers.TryGetValue(playerId, out var state)) return 0f;
            float elapsed = Time.time - state.disconnectTime;
            return Mathf.Max(0f, reconnectWindow - elapsed);
        }

        IEnumerator ReconnectCountdown(string playerId)
        {
            float elapsed = 0f;

            while (elapsed < reconnectWindow)
            {
                yield return new WaitForSeconds(reconnectCheckInterval);
                elapsed += reconnectCheckInterval;

                // Check if player reconnected
                if (!_disconnectedPlayers.ContainsKey(playerId))
                {
                    yield break; // Player reconnected
                }
            }

            // Timeout: remove player
            if (_disconnectedPlayers.ContainsKey(playerId))
            {
                _disconnectedPlayers.Remove(playerId);
                OnReconnectTimeout(playerId);
            }
        }

        void OnReconnectTimeout(string playerId)
        {
            Debug.Log($"[Network] Reconnect timeout for player: {playerId}");

            // In multiplayer: remove player's tools from world
            var tools = FindObjectsByType<Expedition.PlacedTool>(FindObjectsSortMode.None);
            foreach (var tool in tools)
            {
                if (tool.PlacedBy == playerId)
                {
                    Destroy(tool.gameObject);
                }
            }

            // Notify other players
            var session = NetworkSessionManager.Instance;
            if (session != null)
            {
                // TODO: Broadcast player removal
            }
        }

        class ReconnectState
        {
            public string playerId;
            public float disconnectTime;
            public NetworkCheckpoint.CheckpointData lastCheckpoint;
        }
    }
}
