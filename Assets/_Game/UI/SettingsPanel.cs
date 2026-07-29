using UnityEngine;
using UnityEngine.UI;

namespace Asteria.UI
{
    /// <summary>
    /// Settings panel for game options (audio, graphics, controls).
    /// </summary>
    public sealed class SettingsPanel : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] KeyCode toggleKey = KeyCode.Escape;

        GameObject _panelRoot;
        Slider _musicSlider;
        Slider _sfxSlider;
        Slider _mouseSensitivitySlider;
        Toggle _invertYToggle;
        Dropdown _qualityDropdown;
        Dropdown _resolutionDropdown;
        Toggle _fullscreenToggle;
        Button _applyButton;
        Button _closeButton;
        bool _isVisible;

        // Settings values
        float _musicVolume = 0.7f;
        float _sfxVolume = 0.8f;
        float _mouseSensitivity = 2.4f;
        bool _invertY = false;
        int _qualityLevel = 2;
        bool _fullscreen = true;

        void Start()
        {
            BuildUI();
            LoadSettings();
            Hide();
        }

        void Update()
        {
            if (Input.GetKeyDown(toggleKey) && _isVisible)
            {
                Hide();
            }
        }

        void BuildUI()
        {
            var canvas = FindFirstObjectByType<Canvas>();
            if (canvas == null) return;

            // Panel background
            _panelRoot = new GameObject("SettingsPanel");
            _panelRoot.transform.SetParent(canvas.transform, false);

            var rect = _panelRoot.AddComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.sizeDelta = new Vector2(500, 600);

            var bg = _panelRoot.AddComponent<Image>();
            bg.color = new Color(0.1f, 0.12f, 0.18f, 0.95f);

            // Title
            CreateText(_panelRoot.transform, "Title", "设置",
                new Vector2(0, 260), new Vector2(400, 50), 32, TextAnchor.MiddleCenter);

            // Music volume
            CreateText(_panelRoot.transform, "MusicLabel", "音乐音量",
                new Vector2(-120, 190), new Vector2(150, 30), 18, TextAnchor.MiddleRight);
            _musicSlider = CreateSlider(_panelRoot.transform, "MusicSlider",
                new Vector2(60, 190), new Vector2(200, 30), _musicVolume);

            // SFX volume
            CreateText(_panelRoot.transform, "SFXLabel", "音效音量",
                new Vector2(-120, 145), new Vector2(150, 30), 18, TextAnchor.MiddleRight);
            _sfxSlider = CreateSlider(_panelRoot.transform, "SFXSlider",
                new Vector2(60, 145), new Vector2(200, 30), _sfxVolume);

            // Mouse sensitivity
            CreateText(_panelRoot.transform, "MouseLabel", "鼠标灵敏度",
                new Vector2(-120, 100), new Vector2(150, 30), 18, TextAnchor.MiddleRight);
            _mouseSensitivitySlider = CreateSlider(_panelRoot.transform, "MouseSlider",
                new Vector2(60, 100), new Vector2(200, 30), _mouseSensitivity / 5f);

            // Invert Y
            CreateText(_panelRoot.transform, "InvertLabel", "反转Y轴",
                new Vector2(-120, 55), new Vector2(150, 30), 18, TextAnchor.MiddleRight);
            _invertYToggle = CreateToggle(_panelRoot.transform, "InvertToggle",
                new Vector2(60, 55), _invertY);

            // Quality
            CreateText(_panelRoot.transform, "QualityLabel", "画质",
                new Vector2(-120, 10), new Vector2(150, 30), 18, TextAnchor.MiddleRight);
            _qualityDropdown = CreateDropdown(_panelRoot.transform, "QualityDropdown",
                new Vector2(60, 10), new Vector2(200, 30),
                new[] { "低", "中", "高", "极高" }, _qualityLevel);

            // Fullscreen
            CreateText(_panelRoot.transform, "FullscreenLabel", "全屏",
                new Vector2(-120, -35), new Vector2(150, 30), 18, TextAnchor.MiddleRight);
            _fullscreenToggle = CreateToggle(_panelRoot.transform, "FullscreenToggle",
                new Vector2(60, -35), _fullscreen);

            // Apply button
            _applyButton = CreateButton(_panelRoot.transform, "ApplyButton", "应用",
                new Vector2(-70, -100), new Vector2(120, 40), OnApplyClicked,
                new Color(0.3f, 0.6f, 0.3f));

            // Close button
            _closeButton = CreateButton(_panelRoot.transform, "CloseButton", "关闭",
                new Vector2(70, -100), new Vector2(120, 40), OnCloseClicked,
                new Color(0.5f, 0.5f, 0.5f));
        }

        void OnApplyClicked()
        {
            _musicVolume = _musicSlider.value;
            _sfxVolume = _sfxSlider.value;
            _mouseSensitivity = _mouseSensitivitySlider.value * 5f;
            _invertY = _invertYToggle.isOn;
            _qualityLevel = _qualityDropdown.value;
            _fullscreen = _fullscreenToggle.isOn;

            ApplySettings();
            SaveSettings();
        }

        void OnCloseClicked()
        {
            Hide();
        }

        void ApplySettings()
        {
            // Audio
            AudioListener.volume = _sfxVolume;
            // TODO: Apply music volume to music source

            // Graphics
            QualitySettings.SetQualityLevel(_qualityLevel);
            Screen.fullScreen = _fullscreen;

            // Input
            // TODO: Apply mouse sensitivity to camera
        }

        void SaveSettings()
        {
            PlayerPrefs.SetFloat("MusicVolume", _musicVolume);
            PlayerPrefs.SetFloat("SFXVolume", _sfxVolume);
            PlayerPrefs.SetFloat("MouseSensitivity", _mouseSensitivity);
            PlayerPrefs.SetInt("InvertY", _invertY ? 1 : 0);
            PlayerPrefs.SetInt("QualityLevel", _qualityLevel);
            PlayerPrefs.SetInt("Fullscreen", _fullscreen ? 1 : 0);
            PlayerPrefs.Save();
        }

        void LoadSettings()
        {
            _musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.7f);
            _sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 0.8f);
            _mouseSensitivity = PlayerPrefs.GetFloat("MouseSensitivity", 2.4f);
            _invertY = PlayerPrefs.GetInt("InvertY", 0) == 1;
            _qualityLevel = PlayerPrefs.GetInt("QualityLevel", 2);
            _fullscreen = PlayerPrefs.GetInt("Fullscreen", 1) == 1;

            ApplySettings();
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
            rect.anchorMin = rect.anchorMax = new Vector2(0.5f, 0.5f);
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

        static Slider CreateSlider(Transform parent, string name, Vector2 offset, Vector2 size, float value)
        {
            var go = new GameObject(name);
            go.transform.SetParent(parent, false);
            var rect = go.AddComponent<RectTransform>();
            rect.anchorMin = rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = offset;
            rect.sizeDelta = size;
            var img = go.AddComponent<Image>();
            img.color = new Color(0.2f, 0.25f, 0.3f);
            var slider = go.AddComponent<Slider>();
            slider.value = value;
            slider.minValue = 0f;
            slider.maxValue = 1f;
            return slider;
        }

        static Toggle CreateToggle(Transform parent, string name, Vector2 offset, bool value)
        {
            var go = new GameObject(name);
            go.transform.SetParent(parent, false);
            var rect = go.AddComponent<RectTransform>();
            rect.anchorMin = rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = offset;
            rect.sizeDelta = new Vector2(30, 30);
            var img = go.AddComponent<Image>();
            img.color = value ? new Color(0.3f, 0.7f, 0.3f) : new Color(0.3f, 0.3f, 0.3f);
            var toggle = go.AddComponent<Toggle>();
            toggle.isOn = value;
            toggle.targetGraphic = img;
            return toggle;
        }

        static Dropdown CreateDropdown(Transform parent, string name, Vector2 offset, Vector2 size,
            string[] options, int value)
        {
            var go = new GameObject(name);
            go.transform.SetParent(parent, false);
            var rect = go.AddComponent<RectTransform>();
            rect.anchorMin = rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = offset;
            rect.sizeDelta = size;
            var img = go.AddComponent<Image>();
            img.color = new Color(0.2f, 0.25f, 0.3f);
            var dropdown = go.AddComponent<Dropdown>();
            dropdown.AddOptions(new System.Collections.Generic.List<string>(options));
            dropdown.value = value;
            return dropdown;
        }

        static Button CreateButton(Transform parent, string name, string label,
            Vector2 offset, Vector2 size, UnityEngine.Events.UnityAction onClick, Color color)
        {
            var go = new GameObject(name);
            go.transform.SetParent(parent, false);
            var rect = go.AddComponent<RectTransform>();
            rect.anchorMin = rect.anchorMax = new Vector2(0.5f, 0.5f);
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
            textRect.offsetMin = textRect.offsetMax = Vector2.zero;
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
