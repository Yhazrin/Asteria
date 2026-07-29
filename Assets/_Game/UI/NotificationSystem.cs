using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Asteria.UI
{
    /// <summary>
    /// In-game notification system for achievements, discoveries, and events.
    /// Shows toast messages and popup notifications.
    /// </summary>
    public sealed class NotificationSystem : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float toastDuration = 3f;
        [SerializeField] float popupDuration = 5f;
        [SerializeField] int maxToasts = 5;

        GameObject _toastContainer;
        GameObject _popupContainer;
        readonly Queue<ToastData> _toastQueue = new();
        readonly List<ToastInstance> _activeToasts = new();

        void Start()
        {
            BuildUI();
        }

        void BuildUI()
        {
            var canvas = FindFirstObjectByType<Canvas>();
            if (canvas == null) return;

            // Toast container (top right)
            _toastContainer = new GameObject("ToastContainer");
            _toastContainer.transform.SetParent(canvas.transform, false);
            var toastRect = _toastContainer.AddComponent<RectTransform>();
            toastRect.anchorMin = new Vector2(1, 1);
            toastRect.anchorMax = new Vector2(1, 1);
            toastRect.pivot = new Vector2(1, 1);
            toastRect.anchoredPosition = new Vector2(-10, -10);
            toastRect.sizeDelta = new Vector2(300, 400);

            // Popup container (center)
            _popupContainer = new GameObject("PopupContainer");
            _popupContainer.transform.SetParent(canvas.transform, false);
            var popupRect = _popupContainer.AddComponent<RectTransform>();
            popupRect.anchorMin = new Vector2(0.5f, 0.5f);
            popupRect.anchorMax = new Vector2(0.5f, 0.5f);
            popupRect.pivot = new Vector2(0.5f, 0.5f);
            popupRect.anchoredPosition = Vector2.zero;
            popupRect.sizeDelta = new Vector2(400, 200);
        }

        /// <summary>
        /// Show a toast notification.
        /// </summary>
        public void ShowToast(string message, ToastType type = ToastType.Info)
        {
            var data = new ToastData
            {
                message = message,
                type = type,
                duration = toastDuration
            };

            _toastQueue.Enqueue(data);
            ProcessQueue();
        }

        /// <summary>
        /// Show an achievement notification.
        /// </summary>
        public void ShowAchievement(string title, string description)
        {
            ShowToast($"🏆 {title}: {description}", ToastType.Achievement);
        }

        /// <summary>
        /// Show a discovery notification.
        /// </summary>
        public void ShowDiscovery(string name, string description)
        {
            ShowToast($"🔍 发现: {name}", ToastType.Discovery);
        }

        /// <summary>
        /// Show a warning notification.
        /// </summary>
        public void ShowWarning(string message)
        {
            ShowToast($"⚠️ {message}", ToastType.Warning);
        }

        /// <summary>
        /// Show an error notification.
        /// </summary>
        public void ShowError(string message)
        {
            ShowToast($"❌ {message}", ToastType.Error);
        }

        void ProcessQueue()
        {
            if (_toastQueue.Count == 0) return;
            if (_activeToasts.Count >= maxToasts) return;

            var data = _toastQueue.Dequeue();
            CreateToast(data);
        }

        void CreateToast(ToastData data)
        {
            var toastGo = new GameObject("Toast");
            toastGo.transform.SetParent(_toastContainer.transform, false);

            var rect = toastGo.AddComponent<RectTransform>();
            rect.anchorMin = new Vector2(0, 1);
            rect.anchorMax = new Vector2(1, 1);
            rect.pivot = new Vector2(0.5f, 1);
            rect.anchoredPosition = new Vector2(0, -_activeToasts.Count * 60);
            rect.sizeDelta = new Vector2(0, 50);

            var bg = toastGo.AddComponent<Image>();
            bg.color = GetColorForType(data.type);

            var textGo = new GameObject("Text");
            textGo.transform.SetParent(toastGo.transform, false);
            var textRect = textGo.AddComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.offsetMin = new Vector2(10, 5);
            textRect.offsetMax = new Vector2(-10, -5);

            var text = textGo.AddComponent<Text>();
            text.text = data.message;
            text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            text.fontSize = 16;
            text.color = Color.white;
            text.alignment = TextAnchor.MiddleLeft;

            var instance = new ToastInstance
            {
                root = toastGo,
                data = data,
                createdAt = Time.time
            };

            _activeToasts.Add(instance);
            StartCoroutine(FadeOutToast(instance));
        }

        IEnumerator FadeOutToast(ToastInstance instance)
        {
            yield return new WaitForSeconds(instance.data.duration);

            var canvasGroup = instance.root.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = instance.root.AddComponent<CanvasGroup>();
            }

            float elapsed = 0f;
            float fadeDuration = 0.5f;

            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                canvasGroup.alpha = 1f - (elapsed / fadeDuration);
                yield return null;
            }

            _activeToasts.Remove(instance);
            Destroy(instance.root);
            ProcessQueue();
        }

        Color GetColorForType(ToastType type)
        {
            return type switch
            {
                ToastType.Info => new Color(0.2f, 0.3f, 0.5f, 0.9f),
                ToastType.Success => new Color(0.2f, 0.5f, 0.3f, 0.9f),
                ToastType.Warning => new Color(0.6f, 0.5f, 0.2f, 0.9f),
                ToastType.Error => new Color(0.6f, 0.2f, 0.2f, 0.9f),
                ToastType.Achievement => new Color(0.5f, 0.3f, 0.6f, 0.9f),
                ToastType.Discovery => new Color(0.3f, 0.5f, 0.6f, 0.9f),
                _ => new Color(0.2f, 0.3f, 0.5f, 0.9f)
            };
        }

        public enum ToastType
        {
            Info,
            Success,
            Warning,
            Error,
            Achievement,
            Discovery
        }

        struct ToastData
        {
            public string message;
            public ToastType type;
            public float duration;
        }

        class ToastInstance
        {
            public GameObject root;
            public ToastData data;
            public float createdAt;
        }
    }
}
