using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Asteria.UI
{
    /// <summary>
    /// Tutorial system that guides new players through game mechanics.
    /// Shows step-by-step instructions with UI highlights.
    /// </summary>
    public sealed class TutorialSystem : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float messageDuration = 5f;
        [SerializeField] float fadeSpeed = 2f;

        GameObject _tutorialRoot;
        Text _messageText;
        Text _stepText;
        Image _highlightImage;
        CanvasGroup _canvasGroup;

        int _currentStep;
        bool _isActive;
        TutorialStep[] _steps;

        void Start()
        {
            BuildUI();
            InitializeTutorial();
        }

        void BuildUI()
        {
            var canvas = FindFirstObjectByType<Canvas>();
            if (canvas == null) return;

            _tutorialRoot = new GameObject("TutorialPanel");
            _tutorialRoot.transform.SetParent(canvas.transform, false);

            var rect = _tutorialRoot.AddComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.5f, 0);
            rect.anchorMax = new Vector2(0.5f, 0);
            rect.pivot = new Vector2(0.5f, 0);
            rect.anchoredPosition = new Vector2(0, 50);
            rect.sizeDelta = new Vector2(600, 120);

            _canvasGroup = _tutorialRoot.AddComponent<CanvasGroup>();
            _canvasGroup.alpha = 0f;

            var bg = _tutorialRoot.AddComponent<Image>();
            bg.color = new Color(0.05f, 0.08f, 0.15f, 0.9f);

            // Step indicator
            _stepText = CreateText(_tutorialRoot.transform, "StepText", "步骤 1/5",
                new Vector2(-250, 40), new Vector2(100, 30), 14, TextAnchor.MiddleCenter,
                new Color(0.6f, 0.7f, 0.8f));

            // Message
            _messageText = CreateText(_tutorialRoot.transform, "MessageText", "",
                new Vector2(0, -10), new Vector2(560, 60), 20, TextAnchor.MiddleCenter,
                Color.white);
        }

        void InitializeTutorial()
        {
            _steps = new[]
            {
                new TutorialStep
                {
                    message = "欢迎来到 Asteria！这是一颗属于你的小星球。",
                    trigger = "auto",
                    duration = 4f
                },
                new TutorialStep
                {
                    message = "使用 WASD 移动，鼠标控制视角。",
                    trigger = "move",
                    duration = 5f
                },
                new TutorialStep
                {
                    message = "看到发光的石头了吗？走过去按 E 观察它。",
                    trigger = "observe",
                    duration = 6f
                },
                new TutorialStep
                {
                    message = "太棒了！你的发现已记录在图鉴中。",
                    trigger = "auto",
                    duration = 3f
                },
                new TutorialStep
                {
                    message = "走向金色信标，按 E 出发前往远征星球。",
                    trigger = "expedition",
                    duration = 6f
                },
            };
        }

        /// <summary>
        /// Start the tutorial.
        /// </summary>
        public void StartTutorial()
        {
            _currentStep = 0;
            _isActive = true;
            ShowStep(_steps[0]);
        }

        /// <summary>
        /// Advance to the next tutorial step.
        /// </summary>
        public void AdvanceStep()
        {
            _currentStep++;
            if (_currentStep >= _steps.Length)
            {
                CompleteTutorial();
                return;
            }

            ShowStep(_steps[_currentStep]);
        }

        /// <summary>
        /// Skip the tutorial.
        /// </summary>
        public void SkipTutorial()
        {
            CompleteTutorial();
        }

        void ShowStep(TutorialStep step)
        {
            _stepText.text = $"步骤 {_currentStep + 1}/{_steps.Length}";
            _messageText.text = step.message;
            StartCoroutine(FadeIn());

            if (step.trigger == "auto")
            {
                StartCoroutine(AutoAdvance(step.duration));
            }
        }

        IEnumerator FadeIn()
        {
            while (_canvasGroup.alpha < 1f)
            {
                _canvasGroup.alpha += fadeSpeed * Time.deltaTime;
                yield return null;
            }
            _canvasGroup.alpha = 1f;
        }

        IEnumerator FadeOut()
        {
            while (_canvasGroup.alpha > 0f)
            {
                _canvasGroup.alpha -= fadeSpeed * Time.deltaTime;
                yield return null;
            }
            _canvasGroup.alpha = 0f;
        }

        IEnumerator AutoAdvance(float duration)
        {
            yield return new WaitForSeconds(duration);
            AdvanceStep();
        }

        void CompleteTutorial()
        {
            _isActive = false;
            StartCoroutine(FadeOut());
            Debug.Log("[Tutorial] Tutorial completed!");
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

        struct TutorialStep
        {
            public string message;
            public string trigger; // "auto", "move", "observe", "expedition"
            public float duration;
        }
    }
}
