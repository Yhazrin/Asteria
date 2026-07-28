using UnityEngine;
using UnityEngine.UI;

namespace Asteria.Residents
{
    /// <summary>
    /// Shows floating dialogue/thought bubbles above residents.
    /// Displays resident thoughts, conversations, and reactions.
    /// </summary>
    public sealed class ResidentDialogueBubble : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float bubbleDuration = 4f;
        [SerializeField] float fadeSpeed = 2f;
        [SerializeField] float bobSpeed = 1.5f;
        [SerializeField] float bobHeight = 0.2f;
        [SerializeField] Vector3 offset = new(0, 2.5f, 0);

        [Header("References")]
        [SerializeField] Canvas worldCanvas;
        [SerializeField] Text dialogueText;
        [SerializeField] Image bubbleBackground;

        float _showTimer;
        float _baseY;
        Camera _mainCamera;

        void Awake()
        {
            _baseY = transform.localPosition.y + offset.y;
            _mainCamera = Camera.main;
            CreateWorldBubble();
        }

        void Update()
        {
            // Billboard: face camera
            if (_mainCamera != null && worldCanvas != null)
            {
                worldCanvas.transform.rotation = _mainCamera.transform.rotation;
            }

            if (_showTimer > 0)
            {
                _showTimer -= Time.deltaTime;

                // Bob animation
                float bob = Mathf.Sin(Time.time * bobSpeed) * bobHeight;
                var pos = worldCanvas.transform.localPosition;
                pos.y = _baseY + bob;
                worldCanvas.transform.localPosition = pos;

                // Fade out near end
                if (_showTimer < 1f)
                {
                    SetAlpha(_showTimer);
                }
            }
            else
            {
                SetAlpha(0);
            }
        }

        /// <summary>
        /// Show a dialogue message.
        /// </summary>
        public void ShowDialogue(string message)
        {
            if (dialogueText != null)
            {
                dialogueText.text = message;
            }

            _showTimer = bubbleDuration;
            SetAlpha(1);
        }

        /// <summary>
        /// Show a thought (different style).
        /// </summary>
        public void ShowThought(string thought)
        {
            if (dialogueText != null)
            {
                dialogueText.text = $"...{thought}...";
            }

            if (bubbleBackground != null)
            {
                bubbleBackground.color = new Color(0.9f, 0.9f, 1f, 0.9f);
            }

            _showTimer = bubbleDuration;
            SetAlpha(1);
        }

        /// <summary>
        /// Show a reaction (short, with different style).
        /// </summary>
        public void ShowReaction(string reaction)
        {
            if (dialogueText != null)
            {
                dialogueText.text = reaction;
            }

            if (bubbleBackground != null)
            {
                bubbleBackground.color = new Color(1f, 0.9f, 0.7f, 0.9f);
            }

            _showTimer = bubbleDuration * 0.5f; // Shorter duration
            SetAlpha(1);
        }

        void SetAlpha(float alpha)
        {
            if (bubbleBackground != null)
            {
                var color = bubbleBackground.color;
                color.a = alpha * 0.9f;
                bubbleBackground.color = color;
            }

            if (dialogueText != null)
            {
                var color = dialogueText.color;
                color.a = alpha;
                dialogueText.color = color;
            }
        }

        void CreateWorldBubble()
        {
            // Create world-space canvas if not assigned
            if (worldCanvas == null)
            {
                var canvasGo = new GameObject("DialogueCanvas");
                canvasGo.transform.SetParent(transform, false);
                canvasGo.transform.localPosition = offset;

                worldCanvas = canvasGo.AddComponent<Canvas>();
                worldCanvas.renderMode = RenderMode.WorldSpace;
                worldCanvas.worldCamera = _mainCamera;

                var rect = canvasGo.GetComponent<RectTransform>();
                rect.sizeDelta = new Vector2(3, 1);
                rect.localScale = Vector3.one * 0.02f;

                canvasGo.AddComponent<CanvasScaler>();
                canvasGo.AddComponent<GraphicRaycaster>();
            }

            // Create bubble background
            if (bubbleBackground == null)
            {
                var bgGo = new GameObject("BubbleBG");
                bgGo.transform.SetParent(worldCanvas.transform, false);

                var bgRect = bgGo.AddComponent<RectTransform>();
                bgRect.anchorMin = Vector2.zero;
                bgRect.anchorMax = Vector2.one;
                bgRect.offsetMin = new Vector2(5, 5);
                bgRect.offsetMax = new Vector2(-5, -5);

                bubbleBackground = bgGo.AddComponent<Image>();
                bubbleBackground.color = new Color(1, 1, 1, 0.9f);
                bubbleBackground.type = Image.Type.Sliced;
            }

            // Create text
            if (dialogueText == null)
            {
                var textGo = new GameObject("DialogueText");
                textGo.transform.SetParent(worldCanvas.transform, false);

                var textRect = textGo.AddComponent<RectTransform>();
                textRect.anchorMin = Vector2.zero;
                textRect.anchorMax = Vector2.one;
                textRect.offsetMin = new Vector2(10, 5);
                textRect.offsetMax = new Vector2(-10, -5);

                dialogueText = textGo.AddComponent<Text>();
                dialogueText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
                dialogueText.fontSize = 24;
                dialogueText.color = Color.black;
                dialogueText.alignment = TextAnchor.MiddleCenter;
                dialogueText.horizontalOverflow = HorizontalWrapMode.WrapByCharacter;
                dialogueText.verticalOverflow = VerticalWrapMode.Truncate;
            }

            // Start hidden
            SetAlpha(0);
        }
    }
}
