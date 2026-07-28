using UnityEngine;

namespace Asteria.Residents
{
    /// <summary>
    /// Shows floating mood/emotion bubbles above residents.
    /// Visual feedback for resident state without UI panels.
    /// </summary>
    public sealed class ResidentMoodBubble : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float bobSpeed = 2f;
        [SerializeField] float bobHeight = 0.3f;
        [SerializeField] float fadeSpeed = 2f;
        [SerializeField] float showDuration = 3f;

        [Header("References")]
        [SerializeField] SpriteRenderer bubbleRenderer;

        // Mood sprites (created at runtime)
        Sprite _happySprite;
        Sprite _sadSprite;
        Sprite _angrySprite;
        Sprite _curiousSprite;
        Sprite _surprisedSprite;

        float _showTimer;
        float _baseY;
        Color _currentColor;

        void Awake()
        {
            _baseY = transform.localPosition.y;
            CreateMoodSprites();
        }

        void Update()
        {
            if (_showTimer > 0)
            {
                _showTimer -= Time.deltaTime;

                // Bob animation
                float bob = Mathf.Sin(Time.time * bobSpeed) * bobHeight;
                var pos = transform.localPosition;
                pos.y = _baseY + bob;
                transform.localPosition = pos;

                // Fade out near end
                if (_showTimer < 1f)
                {
                    var color = _currentColor;
                    color.a = _showTimer;
                    if (bubbleRenderer != null) bubbleRenderer.color = color;
                }
            }
            else
            {
                // Hide
                if (bubbleRenderer != null)
                {
                    var color = bubbleRenderer.color;
                    color.a = Mathf.MoveTowards(color.a, 0, fadeSpeed * Time.deltaTime);
                    bubbleRenderer.color = color;
                }
            }
        }

        /// <summary>
        /// Show a mood bubble with the given emotion.
        /// </summary>
        public void ShowMood(MoodType mood)
        {
            Sprite sprite = mood switch
            {
                MoodType.Happy => _happySprite,
                MoodType.Sad => _sadSprite,
                MoodType.Angry => _angrySprite,
                MoodType.Curious => _curiousSprite,
                MoodType.Surprised => _surprisedSprite,
                _ => _happySprite
            };

            _currentColor = mood switch
            {
                MoodType.Happy => new Color(1f, 0.9f, 0.3f),
                MoodType.Sad => new Color(0.5f, 0.5f, 0.8f),
                MoodType.Angry => new Color(0.9f, 0.3f, 0.3f),
                MoodType.Curious => new Color(0.3f, 0.8f, 1f),
                MoodType.Surprised => new Color(1f, 0.6f, 0.9f),
                _ => Color.white
            };

            if (bubbleRenderer != null)
            {
                bubbleRenderer.sprite = sprite;
                bubbleRenderer.color = _currentColor;
            }

            _showTimer = showDuration;
        }

        void CreateMoodSprites()
        {
            // Create simple circle sprites for moods
            _happySprite = CreateCircleSprite(new Color(1f, 0.9f, 0.3f));
            _sadSprite = CreateCircleSprite(new Color(0.5f, 0.5f, 0.8f));
            _angrySprite = CreateCircleSprite(new Color(0.9f, 0.3f, 0.3f));
            _curiousSprite = CreateCircleSprite(new Color(0.3f, 0.8f, 1f));
            _surprisedSprite = CreateCircleSprite(new Color(1f, 0.6f, 0.9f));
        }

        static Sprite CreateCircleSprite(Color color)
        {
            int size = 32;
            var texture = new Texture2D(size, size);
            var pixels = new Color[size * size];

            float center = size / 2f;
            float radius = size / 2f - 2f;

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    float dist = Vector2.Distance(new Vector2(x, y), new Vector2(center, center));
                    if (dist < radius)
                    {
                        pixels[y * size + x] = color;
                    }
                    else
                    {
                        pixels[y * size + x] = Color.clear;
                    }
                }
            }

            texture.SetPixels(pixels);
            texture.Apply();

            return Sprite.Create(texture, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f));
        }

        public enum MoodType
        {
            Happy,
            Sad,
            Angry,
            Curious,
            Surprised
        }
    }
}
