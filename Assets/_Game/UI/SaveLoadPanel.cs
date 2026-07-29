using System.IO;
using Asteria.Persistence;
using UnityEngine;
using UnityEngine.UI;

namespace Asteria.UI
{
    /// <summary>
    /// Save/Load UI panel for managing game saves.
    /// Shows save slots, allows saving and loading.
    /// </summary>
    public sealed class SaveLoadPanel : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] KeyCode toggleKey = KeyCode.F5;
        [SerializeField] int maxSlots = 3;

        GameObject _panelRoot;
        Text _titleText;
        Text _statusText;
        Transform _slotContainer;
        Button _saveButton;
        Button _loadButton;
        Button _closeButton;
        int _selectedSlot;
        CanvasGroup _canvasGroup;
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

            _panelRoot = new GameObject("SaveLoadPanel");
            _panelRoot.transform.SetParent(canvas.transform, false);

            var rect = _panelRoot.AddComponent<RectTransform>();
            rect.anchorMin = rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.sizeDelta = new Vector2(500, 400);

            _canvasGroup = _panelRoot.AddComponent<CanvasGroup>();
            _canvasGroup.alpha = 0f;

            var bg = _panelRoot.AddComponent<Image>();
            bg.color = new Color(0.08f, 0.1f, 0.15f, 0.95f);

            // Title
            _titleText = CreateText(_panelRoot.transform, "Title", "存档管理",
                new Vector2(0, 170), new Vector2(400, 40), 28, TextAnchor.MiddleCenter,
                new Color(0.95f, 0.85f, 0.4f));

            // Status
            _statusText = CreateText(_panelRoot.transform, "Status", "选择一个存档位",
                new Vector2(0, 135), new Vector2(400, 25), 16, TextAnchor.MiddleCenter,
                new Color(0.6f, 0.6f, 0.7f));

            // Slot container
            var containerGo = new GameObject("SlotContainer");
            containerGo.transform.SetParent(_panelRoot.transform, false);
            var containerRect = containerGo.AddComponent<RectTransform>();
            containerRect.anchorMin = new Vector2(0, 0.3f);
            containerRect.anchorMax = new Vector2(1, 0.85f);
            containerRect.offsetMin = new Vector2(20, 0);
            containerRect.offsetMax = new Vector2(-20, 0);
            _slotContainer = containerGo.transform;

            // Create slots
            CreateSlots();

            // Buttons
            _saveButton = CreateButton(_panelRoot.transform, "SaveButton", "保存",
                new Vector2(-80, -130), new Vector2(120, 40), OnSaveClicked,
                new Color(0.3f, 0.6f, 0.3f));

            _loadButton = CreateButton(_panelRoot.transform, "LoadButton", "读取",
                new Vector2(80, -130), new Vector2(120, 40), OnLoadClicked,
                new Color(0.3f, 0.5f, 0.7f));

            _closeButton = CreateButton(_panelRoot.transform, "CloseButton", "关闭",
                new Vector2(0, -175), new Vector2(120, 40), OnCloseClicked,
                new Color(0.5f, 0.5f, 0.5f));
        }

        void CreateSlots()
        {
            for (int i = 0; i < maxSlots; i++)
            {
                int slotIndex = i;
                float y = -i * 65;

                var slotGo = new GameObject($"Slot_{i}");
                slotGo.transform.SetParent(_slotContainer, false);

                var slotRect = slotGo.AddComponent<RectTransform>();
                slotRect.anchorMin = new Vector2(0, 1);
                slotRect.anchorMax = new Vector2(1, 1);
                slotRect.pivot = new Vector2(0.5f, 1);
                slotRect.anchoredPosition = new Vector2(0, y);
                slotRect.sizeDelta = new Vector2(0, 55);

                var bg = slotGo.AddComponent<Image>();
                bg.color = new Color(0.15f, 0.18f, 0.25f);

                var button = slotGo.AddComponent<Button>();
                button.targetGraphic = bg;
                button.onClick.AddListener(() => OnSlotClicked(slotIndex));

                // Slot label
                var labelGo = new GameObject("Label");
                labelGo.transform.SetParent(slotGo.transform, false);
                var labelRect = labelGo.AddComponent<RectTransform>();
                labelRect.anchorMin = new Vector2(0, 0);
                labelRect.anchorMax = new Vector2(1, 1);
                labelRect.offsetMin = new Vector2(15, 5);
                labelRect.offsetMax = new Vector2(-15, -5);
                var label = labelGo.AddComponent<Text>();
                label.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
                label.fontSize = 18;
                label.color = Color.white;
                label.alignment = TextAnchor.MiddleLeft;

                // Check if save exists
                string savePath = GetSavePath(i);
                if (File.Exists(savePath))
                {
                    var info = new FileInfo(savePath);
                    label.text = $"存档 {i + 1} — {info.LastWriteTime:yyyy-MM-dd HH:mm}";
                }
                else
                {
                    label.text = $"存档 {i + 1} — 空";
                    label.color = new Color(0.5f, 0.5f, 0.55f);
                }
            }
        }

        void OnSlotClicked(int index)
        {
            _selectedSlot = index;
            _statusText.text = $"已选择存档 {index + 1}";

            // Highlight selected slot
            for (int i = 0; i < _slotContainer.childCount; i++)
            {
                var child = _slotContainer.GetChild(i);
                var img = child.GetComponent<Image>();
                if (img != null)
                {
                    img.color = i == index
                        ? new Color(0.2f, 0.3f, 0.4f)
                        : new Color(0.15f, 0.18f, 0.25f);
                }
            }
        }

        void OnSaveClicked()
        {
            AudioManager.Instance?.PlayUIClick();
            var saveService = Core.GameBootstrap.Instance?.SaveService;
            if (saveService != null)
            {
                saveService.Save();
                _statusText.text = $"已保存到存档 {_selectedSlot + 1}";
                RefreshSlots();
            }
        }

        void OnLoadClicked()
        {
            AudioManager.Instance?.PlayUIClick();
            var saveService = Core.GameBootstrap.Instance?.SaveService;
            if (saveService != null)
            {
                saveService.LoadOrCreate();
                _statusText.text = $"已读取存档 {_selectedSlot + 1}";
            }
        }

        void OnCloseClicked()
        {
            AudioManager.Instance?.PlayUIClick();
            Hide();
        }

        void RefreshSlots()
        {
            // Recreate slots to show updated info
            foreach (Transform child in _slotContainer)
            {
                Destroy(child.gameObject);
            }
            CreateSlots();
        }

        string GetSavePath(int slot)
        {
            return Path.Combine(Application.persistentDataPath, "Saves", $"slot_{slot}", "save.json");
        }

        public void Toggle()
        {
            if (_isVisible) Hide();
            else Show();
        }

        public void Show()
        {
            _panelRoot.SetActive(true);
            RefreshSlots();
            StartCoroutine(FadeIn());
            _isVisible = true;
        }

        public void Hide()
        {
            StartCoroutine(FadeOut());
            _isVisible = false;
        }

        System.Collections.IEnumerator FadeIn()
        {
            while (_canvasGroup.alpha < 1f)
            {
                _canvasGroup.alpha += Time.deltaTime * 3f;
                yield return null;
            }
            _canvasGroup.alpha = 1f;
        }

        System.Collections.IEnumerator FadeOut()
        {
            while (_canvasGroup.alpha > 0f)
            {
                _canvasGroup.alpha -= Time.deltaTime * 3f;
                yield return null;
            }
            _canvasGroup.alpha = 0f;
            _panelRoot.SetActive(false);
        }

        static Text CreateText(Transform parent, string name, string content,
            Vector2 offset, Vector2 size, int fontSize, TextAnchor alignment, Color color)
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
