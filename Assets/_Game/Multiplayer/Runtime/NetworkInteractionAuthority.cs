using System;
using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Multiplayer
{
    /// <summary>
    /// Handles interaction authority in multiplayer.
    /// Ensures each interaction is only processed once by the host.
    ///
    /// In NGO: uses ServerRpc for requests, ClientRpc for confirmations.
    /// </summary>
    public sealed class NetworkInteractionAuthority : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float requestTimeout = 5f;

        readonly Dictionary<string, InteractionRequest> _pendingRequests = new();
        readonly HashSet<string> _processedInteractions = new();

        /// <summary>
        /// Request an interaction authority check. Returns true if approved.
        /// In single-player, always approves. In multiplayer, sends to host.
        /// </summary>
        public bool RequestInteraction(string interactionId, string playerId)
        {
            var session = NetworkSessionManager.Instance;

            // Single-player: always approve
            if (session == null || !session.IsConnected)
            {
                return true;
            }

            // Already processed: reject
            if (_processedInteractions.Contains(interactionId))
            {
                Debug.Log($"[Network] Interaction {interactionId} already processed.");
                return false;
            }

            if (session.IsHost)
            {
                // Host: approve immediately
                ApproveInteraction(interactionId, playerId);
                return true;
            }
            else
            {
                // Client: send request to host
                SendRequestToHost(interactionId, playerId);
                return false; // Will be approved via callback
            }
        }

        /// <summary>
        /// Approve an interaction (called by host).
        /// </summary>
        public void ApproveInteraction(string interactionId, string playerId)
        {
            _processedInteractions.Add(interactionId);

            // Broadcast approval to all clients
            BroadcastApproval(interactionId, playerId);

            Debug.Log($"[Network] Interaction approved: {interactionId} by {playerId}");
        }

        /// <summary>
        /// Check if an interaction has been processed.
        /// </summary>
        public bool IsProcessed(string interactionId)
        {
            return _processedInteractions.Contains(interactionId);
        }

        /// <summary>
        /// Clear processed interactions (used on new expedition).
        /// </summary>
        public void ClearProcessed()
        {
            _processedInteractions.Clear();
        }

        void SendRequestToHost(string interactionId, string playerId)
        {
            // In NGO: ServerRpc
            // For now, store locally
            _pendingRequests[interactionId] = new InteractionRequest
            {
                interactionId = interactionId,
                playerId = playerId,
                requestTime = Time.time
            };

            Debug.Log($"[Network] Requesting interaction: {interactionId}");
        }

        void BroadcastApproval(string interactionId, string playerId)
        {
            // In NGO: ClientRpc
            // For now, invoke locally
            OnInteractionApproved?.Invoke(interactionId, playerId);
        }

        /// <summary>
        /// Called when an interaction is approved by the host.
        /// </summary>
        public event Action<string, string> OnInteractionApproved;

        void Update()
        {
            // Clean up expired requests
            var expired = new List<string>();
            foreach (var kvp in _pendingRequests)
            {
                if (Time.time - kvp.Value.requestTime > requestTimeout)
                {
                    expired.Add(kvp.Key);
                }
            }

            foreach (var id in expired)
            {
                _pendingRequests.Remove(id);
                Debug.Log($"[Network] Interaction request expired: {id}");
            }
        }

        class InteractionRequest
        {
            public string interactionId;
            public string playerId;
            public float requestTime;
        }
    }
}
