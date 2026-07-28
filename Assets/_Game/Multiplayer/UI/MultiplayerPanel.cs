using UnityEngine;
using UnityEngine.UI;

namespace Asteria.Multiplayer.UI
{
    /// <summary>
    /// Multiplayer panel accessible from the home planet.
    /// Allows players to host or join friend rooms.
    /// </summary>
    public sealed class MultiplayerPanel : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] KeyCode toggleKey = KeyCode.M;

        GameObject _panelRoot;
        Text _statusText;
        Text _playerListText;
        InputField _roomCodeInput;
        Button _hostButton;
        Button _joinButton;
        Button _closeButton;
        bool _isVisible;

        void Start()
        {
            BuildUI();
            Hide();
        }

        void Update()
        {
            if (Input.GetKeyDown(toggleKey))
            {
                Toggle();
            }
        }

        void BuildUI()
        {
            var canvas = FindFirstObjectByType<Canvas>();
            if (canvas == null) return;

            // Main panel
            _panelRoot = new GameObject("MultiplayerPanel");
            _panelRoot.transform.SetParent(canvas.transform, false);

            var rect = _panelRoot.AddComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.sizeDelta = new Vector2(400, 350);

            var bg = _panelRoot.AddComponent<Image>();
            bg.color = new Color(0.1f, 0.15f, 0.2f, 0.95f);

            // Title
            CreateText(_panelRoot.transform, "Title", "多人游戏",
                new Vector2(0, 140), new Vector2(380, 40), 24, TextAnchor.MiddleCenter);

            // Status
            _statusText = CreateText(_panelRoot.transform, "Status", "按 M 打开此面板",
                new Vector2(0, 100), new Vector2(380, 30), 16, TextAnchor.MiddleCenter);

            // Room code input
            var inputGo = new GameObject("RoomCodeInput");
            inputGo.transform.SetParent(_panelRoot.transform, false);
            var inputRect = inputGo.AddComponent<RectTransform>();
            inputRect.anchoredPosition = new Vector2(0, 55);
            inputRect.sizeDelta = new Vector2(300, 35);
            _roomCodeInput = inputGo.AddComponent<InputField>();

            var inputBg = inputGo.AddComponent<Image>();
            inputBg.color = new Color(0.2f, 0.25f, 0.3f);

            // Input text
            var inputTextGo = new GameObject("Text");
            inputTextGo.transform.SetParent(inputGo.transform, false);
            var inputTextRect = inputTextGo.AddComponent<RectTransform>();
            inputTextRect.anchorMin = Vector2.zero;
            inputTextRect.anchorMax = Vector2.one;
            inputTextRect.offsetMin = new Vector2(10, 0);
            inputTextRect.offsetMax = new Vector2(-10, 0);
            var inputText = inputTextGo.AddComponent<Text>();
            inputText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            inputText.fontSize = 18;
            inputText.color = Color.white;
            _roomCodeInput.textComponent = inputText;

            // Player list
            _playerListText = CreateText(_panelRoot.transform, "PlayerList", "玩家: 0/4",
                new Vector2(0, 15), new Vector2(380, 30), 16, TextAnchor.MiddleCenter);

            // Host button
            _hostButton = CreateButton(_panelRoot.transform, "HostButton", "创建房间",
                new Vector2(-80, -30), new Vector2(140, 40), OnHostClicked);

            // Join button
            _joinButton = CreateButton(_panelRoot.transform, "JoinButton", "加入房间",
                new Vector2(80, -30), new Vector2(140, 40), OnJoinClicked);

            // Close button
            _closeButton = CreateButton(_panelRoot.transform, "CloseButton", "关闭",
                new Vector2(0, -80), new Vector2(140, 40), OnCloseClicked);

            // Instructions
            CreateText(_panelRoot.transform, "Instructions",
                "创建房间: 你是房主，好友可加入\n加入房间: 输入房间代码加入\n按 M 关闭此面板",
                new Vector2(0, -130), new Vector2(380, 60), 14, TextAnchor.MiddleCenter);
        }

        void OnHostClicked()
        {
            var session = NetworkSessionManager.Instance;
            if (session == null)
            {
                var go = new GameObject("NetworkSessionManager");
                session = go.AddComponent<NetworkSessionManager>();
            }

            bool success = session.StartHost();
            if (success)
            {
                _statusText.text = $"房间已创建! 代码: {session.LocalPlayerId}";
                _playerListText.text = $"玩家: {session.PlayerCount}/{session.MaxPlayers}";
            }
        }

        void OnJoinClicked()
        {
            if (_roomCodeInput == null) return;

            string code = _roomCodeInput.text.Trim();
            if (string.IsNullOrEmpty(code))
            {
                _statusText.text = "请输入房间代码";
                return;
            }

            var session = NetworkSessionManager.Instance;
            if (session == null)
            {
                var go = new GameObject("NetworkSessionManager");
                session = go.AddComponent<NetworkSessionManager>();
            }

            bool success = session.JoinAsClient(code);
            if (success)
            {
                _statusText.text = "连接中...";
            }
        }

        void OnCloseClicked()
        {
            Hide();
        }

        public void Toggle()
        {
            if (_isVisible) Hide();
            else Show();
        }

        public void Show()
        {
            if (_panelRoot != null) _panelRoot.SetActive(true);
            _isVisible = true;
        }

        public void Hide()
        {
            if (_panelRoot != null) _panelRoot.SetActive(false);
            _isVisible = false;
        }

        static Text CreateText(Transform parent, string name, string content,
            Vector2 offset, Vector2 size, int fontSize, TextAnchor alignment)
        {
            var go = new GameObject(name);
            go.transform.SetParent(parent, false);

            var rect = go.AddComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = offset;
            rect.sizeDelta = size;

            var text = go.AddComponent<Text>();
            text.text = content;
            text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            text.fontSize = fontSize;
            text.color = Color.white;
            text.alignment = alignment;

            return text;
        }

        static Button CreateButton(Transform parent, string name, string label,
            Vector2 offset, Vector2 size, UnityEngine.Events.UnityAction onClick)
        {
            var go = new GameObject(name);
            go.transform.SetParent(parent, false);

            var rect = go.AddComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = offset;
            rect.sizeDelta = size;

            var img = go.AddComponent<Image>();
            img.color = new Color(0.3f, 0.5f, 0.7f);

            var button = go.AddComponent<Button>();
            button.targetGraphic = img;
            button.onClick.AddListener(onClick);

            var textGo = new GameObject("Text");
            textGo.transform.SetParent(go.transform, false);
            var textRect = textGo.AddComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.offsetMin = Vector2.zero;
            textRect.offsetMax = Vector2.zero;
            var text = textGo.AddComponent<Text>();
            text.text = label;
            text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            text.fontSize = 18;
            text.color = Color.white;
            text.alignment = TextAnchor.MiddleCenter;

            return button;
        }
    }
}
