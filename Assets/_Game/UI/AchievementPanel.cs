using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Asteria.UI
{
    /// <summary>
    /// Achievement panel UI showing unlocked and locked achievements.
    /// </summary>
    public sealed class AchievementPanel : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] KeyCode toggleKey = KeyCode.J;

        GameObject _panelRoot;
        Transform _achievementContainer;
        Text _titleText;
        Text _progressText;
        CanvasGroup _canvasGroup;

        readonly List<AchievementSlot> _slots = new();
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

            _panelRoot = new GameObject("AchievementPanel");
            _panelRoot.transform.SetParent(canvas.transform, false);

            var rect = _panelRoot.AddComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.sizeDelta = new Vector2(700, 600);

            _canvasGroup = _panelRoot.AddComponent<CanvasGroup>();
            _canvasGroup.alpha = 0f;

            var bg = _panelRoot.AddComponent<Image>();
            bg.color = new Color(0.08f, 0.1f, 0.15f, 0.95f);

            // Title
            _titleText = CreateText(_panelRoot.transform, "Title", "成就",
                new Vector2(0, 260), new Vector2(600, 50), 32, TextAnchor.MiddleCenter,
                new Color(0.95f, 0.85f, 0.4f));

            // Progress
            _progressText = CreateText(_panelRoot.transform, "Progress", "0/15",
                new Vector2(250, 260), new Vector2(100, 30), 16, TextAnchor.MiddleRight,
                new Color(0.6f, 0.6f, 0.7f));

            // Scroll container
            var scrollGo = new GameObject("ScrollContainer");
            scrollGo.transform.SetParent(_panelRoot.transform, false);
            var scrollRect = scrollGo.AddComponent<RectTransform>();
            scrollRect.anchorMin = new Vector2(0, 0);
            scrollRect.anchorMax = new Vector2(1, 0.85f);
            scrollRect.offsetMin = new Vector2(20, 20);
            scrollRect.offsetMax = new Vector2(-20, -20);
            _achievementContainer = scrollGo.transform;

            // Close button
            var closeBtn = CreateButton(_panelRoot.transform, "CloseButton", "关闭",
                new Vector2(0, -260), new Vector2(200, 40), Hide,
                new Color(0.4f, 0.4f, 0.5f));
        }

        /// <summary>
        /// Refresh the achievement display.
        /// </summary>
        public void Refresh()
        {
            var achievementSystem = Data.AchievementSystem.Instance;
            if (achievementSystem == null) return;

            var allAchievements = achievementSystem.GetAllAchievements();
            var unlocked = achievementSystem.GetUnlocked();
            var unlockedIds = new HashSet<string>();
            foreach (var a in unlocked) unlockedIds.Add(a.achievementId);

            _progressText.text = $"{unlocked.Count}/{allAchievements.Count}";

            // Clear existing slots
            foreach (var slot in _slots)
            {
                if (slot.root != null) Destroy(slot.root);
            }
            _slots.Clear();

            // Create new slots
            for (int i = 0; i < allAchievements.Count; i++)
            {
                var achievement = allAchievements[i];
                bool isUnlocked = unlockedIds.Contains(achievement.achievementId);
                float progress = achievementSystem.GetProgress(achievement.achievementId);

                CreateAchievementSlot(achievement, isUnlocked, progress, i);
            }
        }

        void CreateAchievementSlot(Data.AchievementDefinition achievement, bool isUnlocked, float progress, int index)
        {
            float y = -index * 70;

            var slotGo = new GameObject($"Achievement_{achievement.achievementId}");
            slotGo.transform.SetParent(_achievementContainer, false);

            var slotRect = slotGo.AddComponent<RectTransform>();
            slotRect.anchorMin = new Vector2(0, 1);
            slotRect.anchorMax = new Vector2(1, 1);
            slotRect.pivot = new Vector2(0.5f, 1);
            slotRect.anchoredPosition = new Vector2(0, y);
            slotRect.sizeDelta = new Vector2(0, 60);

            var bg = slotGo.AddComponent<Image>();
            bg.color = isUnlocked ?
                new Color(0.2f, 0.3f, 0.2f, 0.8f) :
                new Color(0.15f, 0.15f, 0.2f, 0.6f);

            // Icon placeholder
            var iconGo = new GameObject("Icon");
            iconGo.transform.SetParent(slotGo.transform, false);
            var iconRect = iconGo.AddComponent<RectTransform>();
            iconRect.anchorMin = new Vector2(0, 0);
            iconRect.anchorMax = new Vector2(0, 1);
            iconRect.pivot = new Vector2(0, 0.5f);
            iconRect.anchoredPosition = new Vector2(10, 0);
            iconRect.sizeDelta = new Vector2(50, 0);
            var icon = iconGo.AddComponent<Image>();
            icon.color = isUnlocked ? new Color(0.95f, 0.85f, 0.4f) : new Color(0.3f, 0.3f, 0.35f);

            // Name
            var nameGo = new GameObject("Name");
            nameGo.transform.SetParent(slotGo.transform, false);
            var nameRect = nameGo.AddComponent<RectTransform>();
            nameRect.anchorMin = new Vector2(0.1f, 0.5f);
            nameRect.anchorMax = new Vector2(0.7f, 1);
            nameRect.offsetMin = new Vector2(5, 0);
            nameRect.offsetMax = new Vector2(-5, -5);
            var nameText = nameGo.AddComponent<Text>();
            nameText.text = achievement.displayName;
            nameText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            nameText.fontSize = 18;
            nameText.color = isUnlocked ? Color.white : new Color(0.5f, 0.5f, 0.55f);

            // Description
            var descGo = new GameObject("Description");
            descGo.transform.SetParent(slotGo.transform, false);
            var descRect = descGo.AddComponent<RectTransform>();
            descRect.anchorMin = new Vector2(0.1f, 0);
            descRect.anchorMax = new Vector2(0.7f, 0.5f);
            descRect.offsetMin = new Vector2(5, 5);
            descRect.offsetMax = new Vector2(-5, 0);
            var descText = descGo.AddComponent<Text>();
            descText.text = achievement.description;
            descText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            descText.fontSize = 14;
            descText.color = new Color(0.6f, 0.6f, 0.65f);

            // Progress bar
            var progressGo = new GameObject("ProgressBar");
            progressGo.transform.SetParent(slotGo.transform, false);
            var progressRect = progressGo.AddComponent<RectTransform>();
            progressRect.anchorMin = new Vector2(0.75f, 0.3f);
            progressRect.anchorMax = new Vector2(0.95f, 0.7f);
            progressRect.offsetMin = Vector2.zero;
            progressRect.offsetMax = Vector2.zero;
            var progressBg = progressGo.AddComponent<Image>();
            progressBg.color = new Color(0.1f, 0.1f, 0.15f);

            var fillGo = new GameObject("Fill");
            fillGo.transform.SetParent(progressGo.transform, false);
            var fillRect = fillGo.AddComponent<RectTransform>();
            fillRect.anchorMin = Vector2.zero;
            fillRect.anchorMax = new Vector2(progress, 1);
            fillRect.offsetMin = Vector2.zero;
            fillRect.offsetMax = Vector2.zero;
            var fill = fillGo.AddComponent<Image>();
            fill.color = isUnlocked ? new Color(0.3f, 0.8f, 0.3f) : new Color(0.3f, 0.5f, 0.7f);

            var slot = new AchievementSlot
            {
                root = slotGo,
                achievement = achievement,
                isUnlocked = isUnlocked
            };

            _slots.Add(slot);
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
            text.fontSize = 18;
            text.color = Color.white;
            text.alignment = TextAnchor.MiddleCenter;
            return button;
        }

        class AchievementSlot
        {
            public GameObject root;
            public Data.AchievementDefinition achievement;
            public bool isUnlocked;
        }
    }
}
