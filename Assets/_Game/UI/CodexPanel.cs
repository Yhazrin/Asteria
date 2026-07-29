using System.Collections.Generic;
using Asteria.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Asteria.UI
{
    /// <summary>
    /// Planet codex UI panel showing discovered planets, biomes, and creatures.
    /// </summary>
    public sealed class CodexPanel : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] KeyCode toggleKey = KeyCode.C;

        GameObject _panelRoot;
        Text _titleText;
        Text _progressText;
        Transform _entryContainer;
        Text _detailText;
        Image _previewImage;
        CanvasGroup _canvasGroup;
        bool _isVisible;
        readonly List<CodexSlot> _slots = new();

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

            _panelRoot = new GameObject("CodexPanel");
            _panelRoot.transform.SetParent(canvas.transform, false);

            var rect = _panelRoot.AddComponent<RectTransform>();
            rect.anchorMin = rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.sizeDelta = new Vector2(700, 500);

            _canvasGroup = _panelRoot.AddComponent<CanvasGroup>();
            _canvasGroup.alpha = 0f;

            var bg = _panelRoot.AddComponent<Image>();
            bg.color = new Color(0.08f, 0.1f, 0.15f, 0.95f);

            // Title
            _titleText = CreateText(_panelRoot.transform, "Title", "星球图鉴",
                new Vector2(0, 220), new Vector2(400, 40), 28, TextAnchor.MiddleCenter,
                new Color(0.95f, 0.85f, 0.4f));

            // Progress
            _progressText = CreateText(_panelRoot.transform, "Progress", "0/7",
                new Vector2(250, 220), new Vector2(100, 30), 16, TextAnchor.MiddleRight,
                new Color(0.6f, 0.6f, 0.7f));

            // Entry list
            var listGo = new GameObject("EntryList");
            listGo.transform.SetParent(_panelRoot.transform, false);
            var listRect = listGo.AddComponent<RectTransform>();
            listRect.anchorMin = new Vector2(0, 0.2f);
            listRect.anchorMax = new Vector2(0.45f, 0.9f);
            listRect.offsetMin = new Vector2(15, 0);
            listRect.offsetMax = new Vector2(-5, -10);
            _entryContainer = listGo.transform;

            // Detail panel
            var detailGo = new GameObject("DetailPanel");
            detailGo.transform.SetParent(_panelRoot.transform, false);
            var detailRect = detailGo.AddComponent<RectTransform>();
            detailRect.anchorMin = new Vector2(0.5f, 0.2f);
            detailRect.anchorMax = new Vector2(1, 0.9f);
            detailRect.offsetMin = new Vector2(5, 0);
            detailRect.offsetMax = new Vector2(-15, -10);
            var detailBg = detailGo.AddComponent<Image>();
            detailBg.color = new Color(0.12f, 0.15f, 0.2f);

            // Detail text
            _detailText = CreateText(detailGo.transform, "Detail", "选择一个条目查看详情",
                new Vector2(0, 0), new Vector2(300, 200), 16, TextAnchor.UpperLeft,
                Color.white);

            // Close button
            CreateButton(_panelRoot.transform, "CloseButton", "关闭",
                new Vector2(0, -220), new Vector2(120, 35), OnCloseClicked,
                new Color(0.5f, 0.5f, 0.5f));
        }

        /// <summary>
        /// Refresh the codex display.
        /// </summary>
        public void Refresh()
        {
            var codex = PlanetCodex.Instance;
            if (codex == null) return;

            // Clear existing
            foreach (Transform child in _entryContainer)
                Destroy(child.gameObject);
            _slots.Clear();

            var allEntries = codex.GetAllEntries();
            _progressText.text = $"{codex.DiscoveredCount}/{allEntries.Count}";

            for (int i = 0; i < allEntries.Count; i++)
            {
                var entry = allEntries[i];
                CreateEntrySlot(entry, i);
            }
        }

        void CreateEntrySlot(PlanetCodexEntry entry, int index)
        {
            float y = -index * 50;

            var slotGo = new GameObject($"Entry_{entry.entryId}");
            slotGo.transform.SetParent(_entryContainer, false);

            var slotRect = slotGo.AddComponent<RectTransform>();
            slotRect.anchorMin = new Vector2(0, 1);
            slotRect.anchorMax = new Vector2(1, 1);
            slotRect.pivot = new Vector2(0.5f, 1);
            slotRect.anchoredPosition = new Vector2(0, y);
            slotRect.sizeDelta = new Vector2(0, 40);

            var bg = slotGo.AddComponent<Image>();
            bg.color = entry.isDiscovered
                ? new Color(0.15f, 0.2f, 0.25f)
                : new Color(0.1f, 0.1f, 0.15f);

            var button = slotGo.AddComponent<Button>();
            button.targetGraphic = bg;
            button.onClick.AddListener(() => OnEntryClicked(entry));

            // Icon
            var iconGo = new GameObject("Icon");
            iconGo.transform.SetParent(slotGo.transform, false);
            var iconRect = iconGo.AddComponent<RectTransform>();
            iconRect.anchorMin = new Vector2(0, 0);
            iconRect.anchorMax = new Vector2(0, 1);
            iconRect.pivot = new Vector2(0, 0.5f);
            iconRect.anchoredPosition = new Vector2(5, 0);
            iconRect.sizeDelta = new Vector2(30, 0);
            var icon = iconGo.AddComponent<Image>();
            icon.color = entry.isDiscovered ? entry.previewColor : new Color(0.3f, 0.3f, 0.35f);

            // Name
            var nameGo = new GameObject("Name");
            nameGo.transform.SetParent(slotGo.transform, false);
            var nameRect = nameGo.AddComponent<RectTransform>();
            nameRect.anchorMin = new Vector2(0.1f, 0);
            nameRect.anchorMax = new Vector2(1, 1);
            nameRect.offsetMin = new Vector2(5, 0);
            nameRect.offsetMax = new Vector2(-5, 0);
            var nameText = nameGo.AddComponent<Text>();
            nameText.text = entry.isDiscovered ? entry.displayName : "???";
            nameText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            nameText.fontSize = 16;
            nameText.color = entry.isDiscovered ? Color.white : new Color(0.4f, 0.4f, 0.45f);
            nameText.alignment = TextAnchor.MiddleLeft;

            _slots.Add(new CodexSlot { root = slotGo, entry = entry });
        }

        void OnEntryClicked(PlanetCodexEntry entry)
        {
            Audio.AudioManager.Instance?.PlayUIClick();

            if (entry.isDiscovered)
            {
                _detailText.text = $"<b>{entry.displayName}</b>\n\n{entry.description}\n\n" +
                    $"类型: {entry.planetType}\n" +
                    $"生态区: {entry.primaryBiome}\n" +
                    $"发现时间: {entry.discoveryTime}";
            }
            else
            {
                _detailText.text = "尚未发现此星球。\n\n继续探索以解锁更多内容。";
            }
        }

        void OnCloseClicked()
        {
            Audio.AudioManager.Instance?.PlayUIClick();
            Hide();
        }

        public void Toggle()
        {
            if (_isVisible) Hide();
            else Show();
        }

        public void Show()
        {
            _panelRoot.SetActive(true);
            Refresh();
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
            text.fontSize = 16;
            text.color = Color.white;
            text.alignment = TextAnchor.MiddleCenter;
            return button;
        }

        class CodexSlot
        {
            public GameObject root;
            public PlanetCodexEntry entry;
        }
    }
}
