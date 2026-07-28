using UnityEngine;
using UnityEngine.UI;

namespace Asteria.Multiplayer.UI
{
    /// <summary>
    /// Friend room UI for hosting/joining multiplayer sessions.
    /// Shows room code, connected players, and session controls.
    /// </summary>
    public sealed class FriendRoomUI : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] GameObject roomPanel;
        [SerializeField] Text roomCodeText;
        [SerializeField] Text playerListText;
        [SerializeField] Text statusText;
        [SerializeField] Button hostButton;
        [SerializeField] Button joinButton;
        [SerializeField] Button startButton;
        [SerializeField] Button disconnectButton;
        [SerializeField] InputField roomCodeInput;

        NetworkSessionManager _sessionManager;

        void Start()
        {
            _sessionManager = NetworkSessionManager.Instance;

            // Setup button handlers
            if (hostButton != null) hostButton.onClick.AddListener(OnHostClicked);
            if (joinButton != null) joinButton.onClick.AddListener(OnJoinClicked);
            if (startButton != null) startButton.onClick.AddListener(OnStartClicked);
            if (disconnectButton != null) disconnectButton.onClick.AddListener(OnDisconnectClicked);

            // Subscribe to session events
            if (_sessionManager != null)
            {
                _sessionManager.OnSessionStarted += OnSessionStarted;
                _sessionManager.OnSessionEnded += OnSessionEnded;
                _sessionManager.OnPlayerJoined += OnPlayerJoined;
                _sessionManager.OnPlayerLeft += OnPlayerLeft;
            }

            UpdateUI();
        }

        void OnDestroy()
        {
            if (_sessionManager != null)
            {
                _sessionManager.OnSessionStarted -= OnSessionStarted;
                _sessionManager.OnSessionEnded -= OnSessionEnded;
                _sessionManager.OnPlayerJoined -= OnPlayerJoined;
                _sessionManager.OnPlayerLeft -= OnPlayerLeft;
            }
        }

        void OnHostClicked()
        {
            if (_sessionManager == null) return;

            bool success = _sessionManager.StartHost();
            if (success)
            {
                UpdateUI();
            }
        }

        void OnJoinClicked()
        {
            if (_sessionManager == null || roomCodeInput == null) return;

            string roomCode = roomCodeInput.text.Trim();
            if (string.IsNullOrEmpty(roomCode))
            {
                SetStatus("请输入房间代码");
                return;
            }

            bool success = _sessionManager.JoinAsClient(roomCode);
            if (success)
            {
                SetStatus("连接中...");
            }
        }

        void OnStartClicked()
        {
            if (_sessionManager == null || !_sessionManager.IsHost) return;

            // Start the expedition
            Core.GameBootstrap.Instance?.StartExpedition();
        }

        void OnDisconnectClicked()
        {
            _sessionManager?.Disconnect();
        }

        void OnSessionStarted()
        {
            SetStatus("房间已创建");
            UpdateUI();
        }

        void OnSessionEnded()
        {
            SetStatus("已断开连接");
            UpdateUI();
        }

        void OnPlayerJoined(string playerId)
        {
            SetStatus($"玩家加入: {playerId}");
            UpdateUI();
        }

        void OnPlayerLeft(string playerId)
        {
            SetStatus($"玩家离开: {playerId}");
            UpdateUI();
        }

        void UpdateUI()
        {
            if (_sessionManager == null) return;

            bool isConnected = _sessionManager.IsConnected;
            bool isHost = _sessionManager.IsHost;

            // Show/hide panels
            if (roomPanel != null) roomPanel.SetActive(true);

            // Update room code
            if (roomCodeText != null)
            {
                roomCodeText.text = isConnected ? $"房间: {_sessionManager.LocalPlayerId}" : "未连接";
            }

            // Update player list
            if (playerListText != null)
            {
                playerListText.text = $"玩家: {_sessionManager.PlayerCount}/{_sessionManager.MaxPlayers}";
            }

            // Update buttons
            if (hostButton != null) hostButton.gameObject.SetActive(!isConnected);
            if (joinButton != null) joinButton.gameObject.SetActive(!isConnected);
            if (startButton != null) startButton.gameObject.SetActive(isHost);
            if (disconnectButton != null) disconnectButton.gameObject.SetActive(isConnected);
            if (roomCodeInput != null) roomCodeInput.gameObject.SetActive(!isConnected);
        }

        void SetStatus(string message)
        {
            if (statusText != null)
            {
                statusText.text = message;
            }
        }
    }
}
