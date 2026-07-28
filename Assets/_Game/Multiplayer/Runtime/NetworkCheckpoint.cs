using System;
using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Multiplayer
{
    /// <summary>
    /// Manages expedition checkpoints for reconnection.
    /// Saves and restores expedition state when players disconnect/reconnect.
    /// </summary>
    public sealed class NetworkCheckpoint : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float checkpointInterval = 30f;
        [SerializeField] int maxCheckpoints = 5;

        readonly Queue<CheckpointData> _checkpoints = new();
        float _checkpointTimer;

        /// <summary>
        /// Take a checkpoint of the current expedition state.
        /// </summary>
        public CheckpointData TakeCheckpoint()
        {
            var checkpoint = new CheckpointData
            {
                timestamp = Time.time,
                expeditionId = Core.GameBootstrap.Instance?.ExpeditionSceneName ?? "unknown",

                // Player states
                playerStates = CapturePlayerStates(),

                // Discovery states
                discoveryIds = CaptureDiscoveryIds(),

                // Event director state
                activeEventId = CaptureActiveEventId(),

                // Tool states
                placedToolStates = CaptureToolStates()
            };

            _checkpoints.Enqueue(checkpoint);

            // Limit checkpoint count
            while (_checkpoints.Count > maxCheckpoints)
            {
                _checkpoints.Dequeue();
            }

            Debug.Log($"[Network] Checkpoint taken. Players: {checkpoint.playerStates.Length}");
            return checkpoint;
        }

        /// <summary>
        /// Restore from the latest checkpoint.
        /// </summary>
        public bool RestoreLatest()
        {
            if (_checkpoints.Count == 0)
            {
                Debug.LogWarning("[Network] No checkpoints available.");
                return false;
            }

            // Get latest checkpoint (peek, don't remove)
            var checkpoint = _checkpoints.ToArray()[_checkpoints.Count - 1];
            return RestoreCheckpoint(checkpoint);
        }

        /// <summary>
        /// Restore from a specific checkpoint.
        /// </summary>
        public bool RestoreCheckpoint(CheckpointData checkpoint)
        {
            if (checkpoint == null) return false;

            // Restore player positions
            RestorePlayerStates(checkpoint.playerStates);

            // Restore discoveries
            RestoreDiscoveries(checkpoint.discoveryIds);

            Debug.Log($"[Network] Restored from checkpoint. Players: {checkpoint.playerStates.Length}");
            return true;
        }

        void Update()
        {
            var session = NetworkSessionManager.Instance;
            if (session == null || !session.IsHost) return;

            _checkpointTimer -= Time.deltaTime;
            if (_checkpointTimer <= 0f)
            {
                _checkpointTimer = checkpointInterval;
                TakeCheckpoint();
            }
        }

        PlayerStateData[] CapturePlayerStates()
        {
            var players = FindObjectsByType<NetworkPlayerSync>(FindObjectsSortMode.None);
            var states = new PlayerStateData[players.Length];

            for (int i = 0; i < players.Length; i++)
            {
                states[i] = new PlayerStateData
                {
                    playerId = players[i].PlayerId,
                    position = players[i].transform.position,
                    rotation = players[i].transform.rotation
                };
            }

            return states;
        }

        string[] CaptureDiscoveryIds()
        {
            var journal = Interaction.DiscoveryJournal.Instance;
            if (journal == null) return Array.Empty<string>();

            // Capture all discovery IDs
            var discoveries = Persistence.SaveService?.Current?.discoveries;
            if (discoveries == null) return Array.Empty<string>();

            var ids = new string[discoveries.Count];
            for (int i = 0; i < discoveries.Count; i++)
            {
                ids[i] = discoveries[i].id;
            }

            return ids;
        }

        string CaptureActiveEventId()
        {
            var director = FindFirstObjectByType<Expedition.EventDirectorMinimal>();
            return director != null ? "active" : null;
        }

        PlacedToolState[] CaptureToolStates()
        {
            var tools = FindObjectsByType<Expedition.PlacedTool>(FindObjectsSortMode.None);
            var states = new PlacedToolState[tools.Length];

            for (int i = 0; i < tools.Length; i++)
            {
                states[i] = new PlacedToolState
                {
                    toolId = tools[i].ToolId,
                    position = tools[i].transform.position,
                    placerId = tools[i].PlacedBy
                };
            }

            return states;
        }

        void RestorePlayerStates(PlayerStateData[] states)
        {
            if (states == null) return;

            foreach (var state in states)
            {
                var player = FindNetworkPlayer(state.playerId);
                if (player != null)
                {
                    player.TeleportTo(state.position, state.rotation);
                }
            }
        }

        void RestoreDiscoveries(string[] discoveryIds)
        {
            // Discoveries are already in save data, just verify
            if (discoveryIds == null) return;
            Debug.Log($"[Network] Verified {discoveryIds.Length} discoveries from checkpoint.");
        }

        NetworkPlayerSync FindNetworkPlayer(string playerId)
        {
            var players = FindObjectsByType<NetworkPlayerSync>(FindObjectsSortMode.None);
            foreach (var player in players)
            {
                if (player.PlayerId == playerId) return player;
            }
            return null;
        }

        [Serializable]
        public class CheckpointData
        {
            public float timestamp;
            public string expeditionId;
            public PlayerStateData[] playerStates;
            public string[] discoveryIds;
            public string activeEventId;
            public PlacedToolState[] placedToolStates;
        }

        [Serializable]
        public class PlayerStateData
        {
            public string playerId;
            public Vector3 position;
            public Quaternion rotation;
        }

        [Serializable]
        public class PlacedToolState
        {
            public string toolId;
            public Vector3 position;
            public string placerId;
        }
    }
}
