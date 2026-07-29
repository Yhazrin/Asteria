using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Asteria.UI
{
    /// <summary>
    /// Dialogue box for NPC conversations and story moments.
    /// Supports typewriter effect and multiple pages.
    /// </summary>
    public sealed class DialogueBox : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float typewriterSpeed = 0.03f;
        [SerializeField] float pagePauseTime = 0.5f;

        GameObject _dialogueRoot;
        Text _speakerText;
        Text _dialogueText;
        Text _continueHint;
        Image _portraitFrame;
        CanvasGroup _canvasGroup;

        // State
        bool _isActive;
        bool _isTyping;
        string _fullText;
        int _currentPage;
        DialoguePage[] _pages;
        System.Action _onComplete;

        void Start()
        {
            BuildUI();
            Hide();
        }

        void Update()
        {
            if (!_isActive) return;

            // Skip typewriter or advance page
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) ||
                Input.GetMouseButtonDown(0))
            {
                if (_isTyping)
                {
                    // Skip to end of text
                    _isTyping = false;
                    _dialogueText.text = _fullText;
                }
                else
                {
                    // Next page
                    NextPage();
                }
            }
        }

        void BuildUI()
        {
            var canvas = FindFirstObjectByType<Canvas>();
            if (canvas == null) return;

            _dialogueRoot = new GameObject("DialogueBox");
            _dialogueRoot.transform.SetParent(canvas.transform, false);

            var rect = _dialogueRoot.AddComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.5f, 0);
            rect.anchorMax = new Vector2(0.5f, 0);
            rect.pivot = new Vector2(0.5f, 0);
            rect.anchoredPosition = new Vector2(0, 20);
            rect.sizeDelta = new Vector2(800, 200);

            _canvasGroup = _dialogueRoot.AddComponent<CanvasGroup>();
            _canvasGroup.alpha = 0f;

            // Background
            var bg = _dialogueRoot.AddComponent<Image>();
            bg.color = new Color(0.05f, 0.08f, 0.15f, 0.95f);

            // Portrait frame (left side)
            var portraitGo = new GameObject("PortraitFrame");
            portraitGo.transform.SetParent(_dialogueRoot.transform, false);
            var portraitRect = portraitGo.AddComponent<RectTransform>();
            portraitRect.anchorMin = new Vector2(0, 0);
            portraitRect.anchorMax = new Vector2(0, 1);
            portraitRect.pivot = new Vector2(0, 0.5f);
            portraitRect.anchoredPosition = new Vector2(10, 0);
            portraitRect.sizeDelta = new Vector2(100, 0);
            _portraitFrame = portraitGo.AddComponent<Image>();
            _portraitFrame.color = new Color(0.2f, 0.25f, 0.3f);

            // Speaker name
            var speakerGo = new GameObject("SpeakerText");
            speakerGo.transform.SetParent(_dialogueRoot.transform, false);
            var speakerRect = speakerGo.AddComponent<RectTransform>();
            speakerRect.anchorMin = new Vector2(0, 1);
            speakerRect.anchorMax = new Vector2(1, 1);
            speakerRect.pivot = new Vector2(0.5f, 1);
            speakerRect.anchoredPosition = new Vector2(60, -5);
            speakerRect.sizeDelta = new Vector2(0, 30);
            _speakerText = speakerGo.AddComponent<Text>();
            _speakerText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            _speakerText.fontSize = 20;
            _speakerText.color = new Color(0.95f, 0.85f, 0.4f);
            _speakerText.alignment = TextAnchor.MiddleLeft;

            // Dialogue text
            var dialogueGo = new GameObject("DialogueText");
            dialogueGo.transform.SetParent(_dialogueRoot.transform, false);
            var dialogueRect = dialogueGo.AddComponent<RectTransform>();
            dialogueRect.anchorMin = new Vector2(0, 0);
            dialogueRect.anchorMax = new Vector2(1, 1);
            dialogueRect.pivot = new Vector2(0.5f, 0.5f);
            dialogueRect.anchoredPosition = new Vector2(60, 10);
            dialogueRect.sizeDelta = new Vector2(-70, -50);
            _dialogueText = dialogueGo.AddComponent<Text>();
            _dialogueText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            _dialogueText.fontSize = 18;
            _dialogueText.color = Color.white;
            _dialogueText.alignment = TextAnchor.UpperLeft;
            _dialogueText.horizontalOverflow = HorizontalWrapMode.Wrap;

            // Continue hint
            var hintGo = new GameObject("ContinueHint");
            hintGo.transform.SetParent(_dialogueRoot.transform, false);
            var hintRect = hintGo.AddComponent<RectTransform>();
            hintRect.anchorMin = new Vector2(1, 0);
            hintRect.anchorMax = new Vector2(1, 0);
            hintRect.pivot = new Vector2(1, 0);
            hintRect.anchoredPosition = new Vector2(-10, 5);
            hintRect.sizeDelta = new Vector2(200, 20);
            _continueHint = hintGo.AddComponent<Text>();
            _continueHint.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            _continueHint.fontSize = 14;
            _continueHint.color = new Color(0.6f, 0.6f, 0.7f);
            _continueHint.alignment = TextAnchor.MiddleRight;
            _continueHint.text = "按空格继续...";
        }

        /// <summary>
        /// Show a dialogue with a single page.
        /// </summary>
        public void Show(string speaker, string text, System.Action onComplete = null)
        {
            Show(new[] { new DialoguePage { speaker = speaker, text = text } }, onComplete);
        }

        /// <summary>
        /// Show a dialogue with multiple pages.
        /// </summary>
        public void Show(DialoguePage[] pages, System.Action onComplete = null)
        {
            if (pages == null || pages.Length == 0) return;

            _pages = pages;
            _currentPage = 0;
            _onComplete = onComplete;
            _isActive = true;

            StartCoroutine(FadeIn());
            ShowPage(0);
        }

        /// <summary>
        /// Hide the dialogue box.
        /// </summary>
        public void Hide()
        {
            _isActive = false;
            StartCoroutine(FadeOut());
        }

        void ShowPage(int index)
        {
            if (index >= _pages.Length)
            {
                Hide();
                _onComplete?.Invoke();
                return;
            }

            var page = _pages[index];
            _speakerText.text = page.speaker;
            _fullText = page.text;
            _dialogueText.text = "";
            _currentPage = index;

            // Update continue hint
            _continueHint.text = index < _pages.Length - 1 ? "按空格继续..." : "按空格结束";

            StartCoroutine(TypewriterEffect());
        }

        void NextPage()
        {
            _currentPage++;
            if (_currentPage >= _pages.Length)
            {
                Hide();
                _onComplete?.Invoke();
            }
            else
            {
                ShowPage(_currentPage);
            }
        }

        IEnumerator TypewriterEffect()
        {
            _isTyping = true;

            for (int i = 0; i <= _fullText.Length; i++)
            {
                if (!_isTyping) break;

                _dialogueText.text = _fullText[..i];
                yield return new WaitForSeconds(typewriterSpeed);
            }

            _isTyping = false;
            _dialogueText.text = _fullText;
        }

        IEnumerator FadeIn()
        {
            _dialogueRoot.SetActive(true);

            while (_canvasGroup.alpha < 1f)
            {
                _canvasGroup.alpha += Time.deltaTime * 3f;
                yield return null;
            }

            _canvasGroup.alpha = 1f;
        }

        IEnumerator FadeOut()
        {
            while (_canvasGroup.alpha > 0f)
            {
                _canvasGroup.alpha -= Time.deltaTime * 3f;
                yield return null;
            }

            _canvasGroup.alpha = 0f;
            _dialogueRoot.SetActive(false);
        }

        [System.Serializable]
        public class DialoguePage
        {
            public string speaker;
            [TextArea(2, 4)] public string text;
        }
    }
}
