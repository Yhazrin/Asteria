using System;
using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Multiplayer
{
    /// <summary>
    /// Manages network session lifecycle: start host, join as client, disconnect.
    /// Wraps Netcode for GameObjects (NGO) and provides a clean API for gameplay code.
    ///
    /// Requires: com.unity.netcode.gameobjects
    /// </summary>
    public sealed class NetworkSessionManager : MonoBehaviour
    {
        static NetworkSessionManager _instance;

        [Header("Settings")]
        [SerializeField] int maxPlayers = 4;
        [SerializeField] float reconnectTimeout = 30f;
        [SerializeField] float snapshotInterval = 5f;

        // NGO references (resolved at runtime)
        // NetworkManager _networkManager;

        SessionState _state = SessionState.Disconnected;
        string _localPlayerId;
        string _hostPlayerId;
        readonly Dictionary<string, PlayerConnection> _connectedPlayers = new();
        SessionSnapshot _lastSnapshot;
        float _snapshotTimer;

        public static NetworkSessionManager Instance => _instance;

        public SessionState State => _state;
        public bool IsHost => _state == SessionState.Hosting;
        public bool IsClient => _state == SessionState.Connected;
        public bool IsConnected => _state != SessionState.Disconnected;
        public string LocalPlayerId => _localPlayerId;
        public int PlayerCount => _connectedPlayers.Count;
        public int MaxPlayers => maxPlayers;

        // Events
        public event Action<string> OnPlayerJoined;
        public event Action<string> OnPlayerLeft;
        public event Action<SessionSnapshot> OnSnapshotTaken;
        public event Action OnSessionStarted;
        public event Action OnSessionEnded;

        void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);
        }

        /// <summary>
        /// Start hosting a session. Returns true if successful.
        /// </summary>
        public bool StartHost()
        {
            if (_state != SessionState.Disconnected)
            {
                Debug.LogWarning("[Network] Already in a session.");
                return false;
            }

            // TODO: Initialize NGO NetworkManager as Host
            // _networkManager = NetworkManager.Singleton;
            // _networkManager.StartHost();

            _state = SessionState.Hosting;
            _localPlayerId = Guid.NewGuid().ToString("N")[..8];
            _hostPlayerId = _localPlayerId;

            _connectedPlayers.Clear();
            _connectedPlayers[_localPlayerId] = new PlayerConnection
            {
                playerId = _localPlayerId,
                isHost = true,
                isConnected = true,
                joinTime = Time.time
            };

            OnSessionStarted?.Invoke();
            Debug.Log($"[Network] Started hosting. Player ID: {_localPlayerId}");
            return true;
        }

        /// <summary>
        /// Join an existing session as client. Returns true if successful.
        /// </summary>
        public bool JoinAsClient(string hostAddress)
        {
            if (_state != SessionState.Disconnected)
            {
                Debug.LogWarning("[Network] Already in a session.");
                return false;
            }

            // TODO: Initialize NGO NetworkManager as Client
            // _networkManager = NetworkManager.Singleton;
            // _networkManager.StartClient();

            _state = SessionState.Connecting;
            _localPlayerId = Guid.NewGuid().ToString("N")[..8];

            Debug.Log($"[Network] Connecting to {hostAddress}...");
            return true;
        }

        /// <summary>
        /// Disconnect from the current session.
        /// </summary>
        public void Disconnect()
        {
            if (_state == SessionState.Disconnected) return;

            // TODO: Shutdown NGO
            // _networkManager?.Shutdown();

            _state = SessionState.Disconnected;
            _connectedPlayers.Clear();

            OnSessionEnded?.Invoke();
            Debug.Log("[Network] Disconnected.");
        }

        /// <summary>
        /// Get the current session snapshot for reconnection.
        /// </summary>
        public SessionSnapshot TakeSnapshot()
        {
            var snapshot = new SessionSnapshot
            {
                expeditionId = Core.GameBootstrap.Instance?.ExpeditionSceneName ?? "unknown",
                phase = "expedition",
                elapsedTime = Time.time,
                playerIds = new string[_connectedPlayers.Count],
                playerPositions = new Vector3[_connectedPlayers.Count],
                discoveredIds = System.Array.Empty<string>()
            };

            int i = 0;
            foreach (var kvp in _connectedPlayers)
            {
                snapshot.playerIds[i] = kvp.Key;
                snapshot.playerPositions[i] = kvp.Value.lastKnownPosition;
                i++;
            }

            _lastSnapshot = snapshot;
            OnSnapshotTaken?.Invoke(snapshot);
            return snapshot;
        }

        /// <summary>
        /// Restore from a snapshot (used on reconnection).
        /// </summary>
        public void RestoreFromSnapshot(SessionSnapshot snapshot)
        {
            if (snapshot == null) return;

            _lastSnapshot = snapshot;
            Debug.Log($"[Network] Restored from snapshot: {snapshot.expeditionId}");
        }

        void Update()
        {
            if (_state == SessionState.Hosting)
            {
                _snapshotTimer -= Time.deltaTime;
                if (_snapshotTimer <= 0f)
                {
                    _snapshotTimer = snapshotInterval;
                    TakeSnapshot();
                }
            }
        }

        void OnDestroy()
        {
            if (_instance == this)
            {
                Disconnect();
                _instance = null;
            }
        }

        public enum SessionState
        {
            Disconnected,
            Connecting,
            Connected,
            Hosting,
            Reconnecting
        }

        public class PlayerConnection
        {
            public string playerId;
            public bool isHost;
            public bool isConnected;
            public float joinTime;
            public float lastSeenTime;
            public Vector3 lastKnownPosition;
        }
    }
}
