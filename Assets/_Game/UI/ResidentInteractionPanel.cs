using Asteria.Residents;
using UnityEngine;
using UnityEngine.UI;

namespace Asteria.UI
{
    /// <summary>
    /// Resident interaction panel showing resident details, relationships, and options.
    /// Opens when player interacts with a resident.
    /// </summary>
    public sealed class ResidentInteractionPanel : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float interactionRange = 4f;

        GameObject _panelRoot;
        Text _nameText;
        Text _personalityText;
        Text _moodText;
        Text _relationshipText;
        Text _memoryText;
        Button _giftButton;
        Button _talkButton;
        Button _inviteButton;
        Button _closeButton;
        CanvasGroup _canvasGroup;
        ResidentAgent _currentResident;
        bool _isVisible;

        void Start()
        {
            BuildUI();
            Hide();
        }

        void BuildUI()
        {
            var canvas = FindFirstObjectByType<Canvas>();
            if (canvas == null) return;

            _panelRoot = new GameObject("ResidentPanel");
            _panelRoot.transform.SetParent(canvas.transform, false);

            var rect = _panelRoot.AddComponent<RectTransform>();
            rect.anchorMin = rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.sizeDelta = new Vector2(450, 500);

            _canvasGroup = _panelRoot.AddComponent<CanvasGroup>();
            _canvasGroup.alpha = 0f;

            var bg = _panelRoot.AddComponent<Image>();
            bg.color = new Color(0.08f, 0.1f, 0.15f, 0.95f);

            // Portrait placeholder
            var portraitGo = new GameObject("Portrait");
            portraitGo.transform.SetParent(_panelRoot.transform, false);
            var portraitRect = portraitGo.AddComponent<RectTransform>();
            portraitRect.anchorMin = new Vector2(0, 0.65f);
            portraitRect.anchorMax = new Vector2(0.3f, 1f);
            portraitRect.offsetMin = new Vector2(15, 0);
            portraitRect.offsetMax = new Vector2(-5, -15);
            var portrait = portraitGo.AddComponent<Image>();
            portrait.color = new Color(0.2f, 0.25f, 0.3f);

            // Name
            _nameText = CreateText(_panelRoot.transform, "Name", "居民",
                new Vector2(80, 200), new Vector2(300, 35), 24, TextAnchor.MiddleLeft,
                new Color(0.95f, 0.85f, 0.4f));

            // Personality
            _personalityText = CreateText(_panelRoot.transform, "Personality", "",
                new Vector2(0, 140), new Vector2(420, 50), 14, TextAnchor.UpperLeft,
                new Color(0.7f, 0.7f, 0.75f));

            // Mood
            _moodText = CreateText(_panelRoot.transform, "Mood", "",
                new Vector2(0, 80), new Vector2(420, 25), 16, TextAnchor.UpperLeft,
                new Color(0.8f, 0.8f, 0.85f));

            // Relationship
            _relationshipText = CreateText(_panelRoot.transform, "Relationship", "",
                new Vector2(0, 40), new Vector2(420, 25), 16, TextAnchor.UpperLeft,
                new Color(0.8f, 0.8f, 0.85f));

            // Recent memory
            _memoryText = CreateText(_panelRoot.transform, "Memory", "",
                new Vector2(0, -10), new Vector2(420, 40), 14, TextAnchor.UpperLeft,
                new Color(0.6f, 0.6f, 0.65f));

            // Buttons
            _talkButton = CreateButton(_panelRoot.transform, "TalkButton", "对话",
                new Vector2(-120, -80), new Vector2(100, 35), OnTalkClicked,
                new Color(0.3f, 0.6f, 0.3f));

            _giftButton = CreateButton(_panelRoot.transform, "GiftButton", "赠礼",
                new Vector2(0, -80), new Vector2(100, 35), OnGiftClicked,
                new Color(0.6f, 0.5f, 0.3f));

            _inviteButton = CreateButton(_panelRoot.transform, "InviteButton", "邀请远征",
                new Vector2(120, -80), new Vector2(100, 35), OnInviteClicked,
                new Color(0.3f, 0.5f, 0.7f));

            _closeButton = CreateButton(_panelRoot.transform, "CloseButton", "关闭",
                new Vector2(0, -130), new Vector2(120, 35), OnCloseClicked,
                new Color(0.5f, 0.5f, 0.5f));
        }

        /// <summary>
        /// Show panel for a specific resident.
        /// </summary>
        public void ShowForResident(ResidentAgent resident)
        {
            if (resident == null || resident.Definition == null) return;

            _currentResident = resident;
            _nameText.text = resident.Definition.DisplayName;

            // Personality
            _personalityText.text = $"性格: 合群{FormatValue(resident.Definition.Sociability)} " +
                $"好奇{FormatValue(resident.Definition.Curiosity)} " +
                $"热情{FormatValue(resident.Definition.Warmth)}";

            // Mood
            if (resident.State != null)
            {
                string mood = resident.State.tension > 0.5f ? "紧张" :
                    resident.State.affinity > 0.5f ? "开心" : "平静";
                _moodText.text = $"心情: {mood}";

                // Relationship
                string rel = resident.State.affinity > 0.7f ? "亲密" :
                    resident.State.trust > 0.5f ? "信任" : "认识";
                _relationshipText.text = $"关系: {rel} (亲密度 {resident.State.affinity:F1})";

                // Memory
                if (resident.State.memories.Count > 0)
                {
                    var last = resident.State.memories[resident.State.memories.Count - 1];
                    _memoryText.text = $"最近: {last.eventId} ({last.emotionalTone})";
                }
                else
                {
                    _memoryText.text = "最近: 暂无记忆";
                }
            }

            Show();
        }

        string FormatValue(float value)
        {
            if (value > 0.3f) return "↑";
            if (value < -0.3f) return "↓";
            return "→";
        }

        void OnTalkClicked()
        {
            Audio.AudioManager.Instance?.PlayUIClick();
            if (_currentResident != null)
            {
                var bubble = _currentResident.GetComponentInChildren<ResidentDialogueBubble>();
                if (bubble != null)
                {
                    bubble.ShowDialogue("你好呀！今天天气真不错~");
                }
            }
        }

        void OnGiftClicked()
        {
            Audio.AudioManager.Instance?.PlayUIClick();
            if (_currentResident?.State != null)
            {
                _currentResident.State.affinity += 0.05f;
                _currentResident.State.affinity = Mathf.Clamp01(_currentResident.State.affinity);
                _relationshipText.text = $"关系: 亲密度 {_currentResident.State.affinity:F1}";

                var bubble = _currentResident.GetComponentInChildren<ResidentDialogueBubble>();
                if (bubble != null)
                {
                    bubble.ShowReaction("谢谢！");
                }
            }
        }

        void OnInviteClicked()
        {
            Audio.AudioManager.Instance?.PlayUIClick();
            // TODO: Add resident to expedition party
            Debug.Log("[ResidentPanel] Invite to expedition - not yet implemented");
        }

        void OnCloseClicked()
        {
            Audio.AudioManager.Instance?.PlayUIClick();
            Hide();
        }

        public void Show()
        {
            _panelRoot.SetActive(true);
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
    }
}
