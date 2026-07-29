using Asteria.Audio;
using UnityEngine;
using UnityEngine.UI;

namespace Asteria.UI
{
    /// <summary>
    /// Main menu screen. Shows title, options, and start/join buttons.
    /// </summary>
    public sealed class MainMenu : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] string gameTitle = "Asteria";
        [SerializeField] string subtitle = "在小星球上和朋友一起冒险";

        GameObject _menuRoot;
        Button _startButton;
        Button _joinButton;
        Button _settingsButton;
        Button _quitButton;
        Text _titleText;
        Text _subtitleText;
        Text _versionText;

        void Start()
        {
            BuildUI();
        }

        void BuildUI()
        {
            // Create canvas
            var canvasGo = new GameObject("MainMenuCanvas");
            canvasGo.transform.SetParent(transform, false);
            var canvas = canvasGo.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 200;

            var scaler = canvasGo.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            scaler.matchWidthOrHeight = 0.5f;

            canvasGo.AddComponent<GraphicRaycaster>();

            // Background
            var bgGo = new GameObject("Background");
            bgGo.transform.SetParent(canvasGo.transform, false);
            var bgRect = bgGo.AddComponent<RectTransform>();
            bgRect.anchorMin = Vector2.zero;
            bgRect.anchorMax = Vector2.one;
            bgRect.offsetMin = Vector2.zero;
            bgRect.offsetMax = Vector2.zero;
            var bg = bgGo.AddComponent<Image>();
            bg.color = new Color(0.05f, 0.08f, 0.15f, 0.95f);

            // Title
            _titleText = CreateText(canvasGo.transform, "Title", gameTitle,
                new Vector2(0, 200), new Vector2(800, 100), 64, TextAnchor.MiddleCenter,
                new Color(0.95f, 0.85f, 0.4f));

            // Subtitle
            _subtitleText = CreateText(canvasGo.transform, "Subtitle", subtitle,
                new Vector2(0, 130), new Vector2(600, 40), 24, TextAnchor.MiddleCenter,
                new Color(0.7f, 0.7f, 0.8f));

            // Button panel
            var buttonPanel = new GameObject("ButtonPanel");
            buttonPanel.transform.SetParent(canvasGo.transform, false);
            var panelRect = buttonPanel.AddComponent<RectTransform>();
            panelRect.anchoredPosition = new Vector2(0, -50);
            panelRect.sizeDelta = new Vector2(300, 250);

            // Start button
            _startButton = CreateButton(buttonPanel.transform, "StartButton", "开始冒险",
                new Vector2(0, 80), new Vector2(250, 50), OnStartClicked,
                new Color(0.3f, 0.6f, 0.3f));

            // Join button
            _joinButton = CreateButton(buttonPanel.transform, "JoinButton", "加入好友",
                new Vector2(0, 15), new Vector2(250, 50), OnJoinClicked,
                new Color(0.3f, 0.5f, 0.7f));

            // Settings button
            _settingsButton = CreateButton(buttonPanel.transform, "SettingsButton", "设置",
                new Vector2(0, -50), new Vector2(250, 50), OnSettingsClicked,
                new Color(0.5f, 0.5f, 0.5f));

            // Quit button
            _quitButton = CreateButton(buttonPanel.transform, "QuitButton", "退出",
                new Vector2(0, -115), new Vector2(250, 50), OnQuitClicked,
                new Color(0.6f, 0.3f, 0.3f));

            // Version
            _versionText = CreateText(canvasGo.transform, "Version", $"v{Application.version}",
                new Vector2(0, -350), new Vector2(200, 30), 14, TextAnchor.MiddleCenter,
                new Color(0.4f, 0.4f, 0.5f));

            // Decorative stars
            CreateStarField(canvasGo.transform);
        }

        void CreateStarField(Transform parent)
        {
            for (int i = 0; i < 50; i++)
            {
                var star = new GameObject($"Star_{i}");
                star.transform.SetParent(parent, false);

                var rect = star.AddComponent<RectTransform>();
                rect.anchoredPosition = new Vector2(
                    Random.Range(-960f, 960f),
                    Random.Range(-540f, 540f));
                rect.sizeDelta = Vector2.one * Random.Range(1f, 3f);

                var img = star.AddComponent<Image>();
                img.color = new Color(1f, 1f, 1f, Random.Range(0.1f, 0.4f));
            }
        }

        void OnStartClicked()
        {
            AudioManager.Instance?.PlayUIClick();
            Debug.Log("[MainMenu] Starting new game...");
            Core.GameBootstrap.Instance?.GoHome();
        }

        void OnJoinClicked()
        {
            Debug.Log("[MainMenu] Opening join screen...");
            // TODO: Show join dialog
        }

        void OnSettingsClicked()
        {
            Debug.Log("[MainMenu] Opening settings...");
            // TODO: Show settings
        }

        void OnQuitClicked()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        static Text CreateText(Transform parent, string name, string content,
            Vector2 offset, Vector2 size, int fontSize, TextAnchor alignment, Color color)
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
            text.color = color;
            text.alignment = alignment;

            return text;
        }

        static Button CreateButton(Transform parent, string name, string label,
            Vector2 offset, Vector2 size, UnityEngine.Events.UnityAction onClick, Color color)
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
            img.color = color;

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
            text.fontSize = 22;
            text.color = Color.white;
            text.alignment = TextAnchor.MiddleCenter;

            return button;
        }
    }
}
