using UnityEngine;
using UnityEngine.UI;

namespace Asteria.UI
{
    /// <summary>
    /// Shows discovery popup when player observes something new.
    /// Fades in, displays for a few seconds, then fades out.
    /// </summary>
    public sealed class DiscoveryPopup
    {
        readonly GameObject _root;
        readonly Text _title;
        readonly Text _description;
        readonly CanvasGroup _canvasGroup;
        float _showUntil;
        float _fadeSpeed = 2f;

        public DiscoveryPopup(Transform parent)
        {
            _root = new GameObject("DiscoveryPopup");
            _root.transform.SetParent(parent, false);

            var rect = _root.AddComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.5f, 0.7f);
            rect.anchorMax = new Vector2(0.5f, 0.7f);
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = Vector2.zero;
            rect.sizeDelta = new Vector2(500, 120);

            var bg = _root.AddComponent<Image>();
            bg.color = new Color(0.1f, 0.15f, 0.2f, 0.9f);

            _canvasGroup = _root.AddComponent<CanvasGroup>();
            _canvasGroup.alpha = 0;

            // Title
            var titleGo = new GameObject("Title");
            titleGo.transform.SetParent(_root.transform, false);
            var titleRect = titleGo.AddComponent<RectTransform>();
            titleRect.anchorMin = new Vector2(0, 0.6f);
            titleRect.anchorMax = new Vector2(1, 1);
            titleRect.offsetMin = new Vector2(15, 0);
            titleRect.offsetMax = new Vector2(-15, -5);
            _title = titleGo.AddComponent<Text>();
            _title.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            _title.fontSize = 24;
            _title.color = new Color(0.95f, 0.85f, 0.4f);
            _title.alignment = TextAnchor.MiddleCenter;
            _title.text = "";

            // Description
            var descGo = new GameObject("Description");
            descGo.transform.SetParent(_root.transform, false);
            var descRect = descGo.AddComponent<RectTransform>();
            descRect.anchorMin = new Vector2(0, 0);
            descRect.anchorMax = new Vector2(1, 0.6f);
            descRect.offsetMin = new Vector2(15, 5);
            descRect.offsetMax = new Vector2(-15, 0);
            _description = descGo.AddComponent<Text>();
            _description.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            _description.fontSize = 16;
            _description.color = new Color(0.8f, 0.8f, 0.8f);
            _description.alignment = TextAnchor.UpperCenter;
            _description.text = "";
        }

        public void Show(string title, string description, float duration = 4f)
        {
            if (_title != null) _title.text = $"发现 · {title}";
            if (_description != null) _description.text = description;
            _showUntil = Time.unscaledTime + duration;
            if (_canvasGroup != null) _canvasGroup.alpha = 1;
        }

        public void Tick()
        {
            if (_canvasGroup == null) return;

            if (Time.unscaledTime > _showUntil)
            {
                _canvasGroup.alpha = Mathf.MoveTowards(_canvasGroup.alpha, 0, _fadeSpeed * Time.unscaledDeltaTime);
            }
        }
    }
}
