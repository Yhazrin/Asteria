using UnityEngine;
using UnityEngine.UI;

namespace Asteria.UI
{
    /// <summary>
    /// Expedition status panel showing tools, pressure, and objectives.
    /// </summary>
    public sealed class ExpeditorPanel
    {
        readonly GameObject _root;
        readonly Text _toolInfo;
        readonly Text _pressureInfo;
        readonly Text _objectiveInfo;

        public ExpeditorPanel(Transform parent)
        {
            _root = new GameObject("ExpeditionPanel");
            _root.transform.SetParent(parent, false);

            var rect = _root.AddComponent<RectTransform>();
            rect.anchorMin = new Vector2(1, 1);
            rect.anchorMax = new Vector2(1, 1);
            rect.pivot = new Vector2(1, 1);
            rect.anchoredPosition = new Vector2(-10, -10);
            rect.sizeDelta = new Vector2(250, 150);

            var bg = _root.AddComponent<Image>();
            bg.color = new Color(0, 0, 0, 0.4f);

            _toolInfo = CreateText(_root.transform, "ToolInfo",
                "工具: 2/2", new Vector2(10, -10), new Vector2(230, 25));

            _pressureInfo = CreateText(_root.transform, "PressureInfo",
                "", new Vector2(10, -40), new Vector2(230, 25));

            _objectiveInfo = CreateText(_root.transform, "ObjectiveInfo",
                "", new Vector2(10, -70), new Vector2(230, 50));

            _root.SetActive(false);
        }

        public void Show()
        {
            if (_root != null) _root.SetActive(true);
        }

        public void Hide()
        {
            if (_root != null) _root.SetActive(false);
        }

        public void UpdateToolInfo(int used, int max)
        {
            if (_toolInfo != null) _toolInfo.text = $"工具: {used}/{max}";
        }

        public void UpdatePressureInfo(string text)
        {
            if (_pressureInfo != null)
            {
                _pressureInfo.text = text;
                _pressureInfo.color = string.IsNullOrEmpty(text) ? Color.white : new Color(1f, 0.5f, 0.3f);
            }
        }

        public void UpdateObjectiveInfo(string text)
        {
            if (_objectiveInfo != null) _objectiveInfo.text = text;
        }

        static Text CreateText(Transform parent, string name, string content,
            Vector2 offset, Vector2 size)
        {
            var go = new GameObject(name);
            go.transform.SetParent(parent, false);

            var rect = go.AddComponent<RectTransform>();
            rect.anchorMin = new Vector2(0, 1);
            rect.anchorMax = new Vector2(1, 1);
            rect.pivot = new Vector2(0, 1);
            rect.anchoredPosition = offset;
            rect.sizeDelta = size;

            var text = go.AddComponent<Text>();
            text.text = content;
            text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            text.fontSize = 16;
            text.color = Color.white;
            text.alignment = TextAnchor.UpperRight;

            return text;
        }
    }
}
