using UnityEngine;
using UnityEngine.UI;

namespace Asteria.UI
{
    /// <summary>
    /// Shows interaction prompt at screen center (e.g., "按 E 观察 · 风铃石").
    /// </summary>
    public sealed class InteractionPrompt
    {
        readonly GameObject _root;
        readonly Text _promptText;

        public InteractionPrompt(Transform parent)
        {
            _root = new GameObject("InteractionPrompt");
            _root.transform.SetParent(parent, false);

            var rect = _root.AddComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.5f, 0);
            rect.anchorMax = new Vector2(0.5f, 0);
            rect.pivot = new Vector2(0.5f, 0);
            rect.anchoredPosition = new Vector2(0, 100);
            rect.sizeDelta = new Vector2(400, 50);

            var bg = _root.AddComponent<Image>();
            bg.color = new Color(0, 0, 0, 0.6f);

            var textGo = new GameObject("Text");
            textGo.transform.SetParent(_root.transform, false);

            var textRect = textGo.AddComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.offsetMin = new Vector2(10, 5);
            textRect.offsetMax = new Vector2(-10, -5);

            _promptText = textGo.AddComponent<Text>();
            _promptText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            _promptText.fontSize = 20;
            _promptText.color = Color.white;
            _promptText.alignment = TextAnchor.MiddleCenter;

            _root.SetActive(false);
        }

        public void Show(string text)
        {
            if (_promptText != null)
            {
                _promptText.text = text;
            }

            if (_root != null)
            {
                _root.SetActive(!string.IsNullOrEmpty(text));
            }
        }

        public void Hide()
        {
            if (_root != null)
            {
                _root.SetActive(false);
            }
        }
    }
}
