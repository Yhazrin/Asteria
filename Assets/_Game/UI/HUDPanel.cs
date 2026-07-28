using UnityEngine;
using UnityEngine.UI;

namespace Asteria.UI
{
    /// <summary>
    /// Persistent HUD showing discovery count, current activity, and quick info.
    /// </summary>
    public sealed class HUDPanel
    {
        readonly GameObject _root;
        readonly Text _discoveryCount;
        readonly Text _dayTime;
        readonly Text _residentInfo;

        public HUDPanel(Transform parent)
        {
            _root = CreatePanel(parent, "HUD", new Vector2(0, 1), new Vector2(0, 1),
                new Vector2(10, -10), new Vector2(300, 120));

            _discoveryCount = CreateText(_root.transform, "DiscoveryCount",
                "图鉴: 0", new Vector2(10, -10), new Vector2(280, 30), TextAnchor.UpperLeft);

            _dayTime = CreateText(_root.transform, "DayTime",
                "第 1 天", new Vector2(10, -45), new Vector2(280, 30), TextAnchor.UpperLeft);

            _residentInfo = CreateText(_root.transform, "ResidentInfo",
                "", new Vector2(10, -80), new Vector2(280, 30), TextAnchor.UpperLeft);
        }

        public void UpdateDiscoveryCount(int count)
        {
            if (_discoveryCount != null)
            {
                _discoveryCount.text = $"图鉴: {count}";
            }
        }

        public void UpdateDayTime(int day, float timeOfDay)
        {
            if (_dayTime != null)
            {
                string timeStr = timeOfDay < 0.25f ? "凌晨" :
                    timeOfDay < 0.5f ? "上午" :
                    timeOfDay < 0.75f ? "下午" : "夜晚";
                _dayTime.text = $"第 {day} 天 · {timeStr}";
            }
        }

        public void UpdateResidentInfo(string text)
        {
            if (_residentInfo != null)
            {
                _residentInfo.text = text;
            }
        }

        static GameObject CreatePanel(Transform parent, string name,
            Vector2 anchorMin, Vector2 anchorMax, Vector2 offset, Vector2 size)
        {
            var go = new GameObject(name);
            go.transform.SetParent(parent, false);

            var rect = go.AddComponent<RectTransform>();
            rect.anchorMin = anchorMin;
            rect.anchorMax = anchorMax;
            rect.pivot = anchorMin;
            rect.anchoredPosition = offset;
            rect.sizeDelta = size;

            var bg = go.AddComponent<Image>();
            bg.color = new Color(0, 0, 0, 0.4f);

            return go;
        }

        static Text CreateText(Transform parent, string name, string content,
            Vector2 offset, Vector2 size, TextAnchor alignment)
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
            text.fontSize = 18;
            text.color = Color.white;
            text.alignment = alignment;

            return text;
        }
    }
}
