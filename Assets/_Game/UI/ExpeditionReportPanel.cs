using UnityEngine;
using UnityEngine.UI;

namespace Asteria.UI
{
    /// <summary>
    /// UI panel displaying expedition report after returning home.
    /// Shows statistics, narrative, and recommendations.
    /// </summary>
    public sealed class ExpeditionReportPanel : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float displayDuration = 15f;

        GameObject _panelRoot;
        Text _titleText;
        Text _outcomeText;
        Text _statsText;
        Text _narrativeText;
        Text _momentsText;
        Text _recommendationsText;
        CanvasGroup _canvasGroup;

        void Start()
        {
            BuildUI();
            Hide();
        }

        void BuildUI()
        {
            var canvas = FindFirstObjectByType<Canvas>();
            if (canvas == null) return;

            _panelRoot = new GameObject("ExpeditionReportPanel");
            _panelRoot.transform.SetParent(canvas.transform, false);

            var rect = _panelRoot.AddComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.sizeDelta = new Vector2(700, 800);

            _canvasGroup = _panelRoot.AddComponent<CanvasGroup>();
            _canvasGroup.alpha = 0f;

            var bg = _panelRoot.AddComponent<Image>();
            bg.color = new Color(0.05f, 0.08f, 0.15f, 0.95f);

            // Title
            _titleText = CreateText(_panelRoot.transform, "Title", "远征报告",
                new Vector2(0, 350), new Vector2(600, 50), 32, TextAnchor.MiddleCenter,
                new Color(0.95f, 0.85f, 0.4f));

            // Outcome
            _outcomeText = CreateText(_panelRoot.transform, "Outcome", "",
                new Vector2(0, 290), new Vector2(600, 40), 24, TextAnchor.MiddleCenter,
                new Color(0.4f, 0.8f, 0.4f));

            // Stats
            _statsText = CreateText(_panelRoot.transform, "Stats", "",
                new Vector2(0, 200), new Vector2(600, 100), 18, TextAnchor.UpperLeft,
                Color.white);

            // Narrative
            _narrativeText = CreateText(_panelRoot.transform, "Narrative", "",
                new Vector2(0, 80), new Vector2(600, 80), 16, TextAnchor.UpperLeft,
                new Color(0.8f, 0.8f, 0.85f));

            // Key moments
            _momentsText = CreateText(_panelRoot.transform, "Moments", "",
                new Vector2(0, -30), new Vector2(600, 100), 16, TextAnchor.UpperLeft,
                new Color(0.7f, 0.8f, 0.9f));

            // Recommendations
            _recommendationsText = CreateText(_panelRoot.transform, "Recommendations", "",
                new Vector2(0, -160), new Vector2(600, 100), 16, TextAnchor.UpperLeft,
                new Color(0.9f, 0.8f, 0.6f));

            // Close button
            var closeBtn = CreateButton(_panelRoot.transform, "CloseButton", "关闭",
                new Vector2(0, -330), new Vector2(200, 40), Hide,
                new Color(0.4f, 0.4f, 0.5f));
        }

        /// <summary>
        /// Show the expedition report.
        /// </summary>
        public void ShowReport(Expedition.ExpeditionReport.ExpeditionReportData report)
        {
            if (report == null) return;

            // Outcome
            string outcomeText = report.outcome switch
            {
                "perfect" => "🌟 完美远征！",
                "success" => "✅ 远征成功",
                "partial" => "⚠️ 部分完成",
                "minimal" => "📝 收获有限",
                _ => "远征结束"
            };
            _outcomeText.text = outcomeText;

            // Stats
            _statsText.text = $"📊 统计\n" +
                             $"发现: {report.discoveries}\n" +
                             $"修复: {report.restores}\n" +
                             $"合作: {report.cooperates}\n" +
                             $"救援: {report.rescues}\n" +
                             $"总分: {report.totalScore}";

            // Narrative
            _narrativeText.text = $"📖 {report.narrative}";

            // Key moments
            if (report.keyMoments != null && report.keyMoments.Count > 0)
            {
                _momentsText.text = "✨ 关键时刻\n" + string.Join("\n", report.keyMoments);
            }

            // Recommendations
            if (report.recommendations != null && report.recommendations != 0)
            {
                _recommendationsText.text = "💡 建议\n" + string.Join("\n", report.recommendations);
            }

            Show();
        }

        void Show()
        {
            _panelRoot.SetActive(true);
            StartCoroutine(FadeIn());
        }

        public void Hide()
        {
            StartCoroutine(FadeOut());
        }

        System.Collections.IEnumerator FadeIn()
        {
            while (_canvasGroup.alpha < 1f)
            {
                _canvasGroup.alpha += Time.deltaTime * 2f;
                yield return null;
            }
            _canvasGroup.alpha = 1f;
        }

        System.Collections.IEnumerator FadeOut()
        {
            while (_canvasGroup.alpha > 0f)
            {
                _canvasGroup.alpha -= Time.deltaTime * 2f;
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
