using Asteria.Data;
using UnityEngine;

// GameHud lives in the Interaction namespace (not UI) because InteractionDetector
// and ObserveInteractable depend on it. Keeping it here avoids a circular asmdef
// dependency between Interaction ↔ UI.
namespace Asteria.Interaction
{
    /// <summary>
    /// Lightweight HUD bridge used by interaction systems (OnGUI for Phase 1).
    /// </summary>
    public sealed class GameHud : MonoBehaviour
    {
        static GameHud _instance;

        string _prompt = string.Empty;
        string _toast = string.Empty;
        float _toastUntil;
        string _lastDiscoveryTitle = string.Empty;
        string _lastDiscoveryBody = string.Empty;
        float _discoveryUntil;
        int _discoveryCount;

        public static GameHud Instance
        {
            get
            {
                if (_instance != null)
                {
                    return _instance;
                }

                _instance = FindFirstObjectByType<GameHud>();
                if (_instance != null)
                {
                    return _instance;
                }

                var go = new GameObject("GameHud");
                _instance = go.AddComponent<GameHud>();
                return _instance;
            }
        }

        public static void SetPrompt(string text)
        {
            Instance._prompt = text ?? string.Empty;
        }

        public static void ShowToast(string text, float seconds = 2.5f)
        {
            Instance._toast = text ?? string.Empty;
            Instance._toastUntil = Time.unscaledTime + seconds;
        }

        public static void ShowDiscovery(ObserveEntry entry, float seconds = 4.5f)
        {
            if (entry == null)
            {
                return;
            }

            Instance._lastDiscoveryTitle = entry.displayName;
            Instance._lastDiscoveryBody = entry.description;
            Instance._discoveryUntil = Time.unscaledTime + seconds;
            Instance._discoveryCount = DiscoveryJournal.Instance.Count;
            ShowToast($"图鉴 +1 · {entry.displayName}", seconds);
        }

        void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
        }

        void OnGUI()
        {
            const float pad = 16f;

            GUILayout.BeginArea(new Rect(pad, pad, 460f, 150f), GUI.skin.box);
            GUILayout.Label("Asteria — 球面探索切片");
            GUILayout.Label("WASD 移动 · Shift 跑 · Space 跳 · 鼠标视角 · E 观察");
            GUILayout.Label($"图鉴记录：{_discoveryCount}");
            GUILayout.EndArea();

            if (!string.IsNullOrEmpty(_prompt))
            {
                float w = 360f;
                float h = 40f;
                Rect r = new Rect((Screen.width - w) * 0.5f, Screen.height - 96f, w, h);
                GUI.Box(r, _prompt);
            }

            if (!string.IsNullOrEmpty(_toast) && Time.unscaledTime < _toastUntil)
            {
                float w = 420f;
                float h = 36f;
                Rect r = new Rect((Screen.width - w) * 0.5f, 96f, w, h);
                GUI.Box(r, _toast);
            }

            if (!string.IsNullOrEmpty(_lastDiscoveryTitle) && Time.unscaledTime < _discoveryUntil)
            {
                float w = 480f;
                float h = 88f;
                Rect r = new Rect((Screen.width - w) * 0.5f, 140f, w, h);
                GUILayout.BeginArea(r, GUI.skin.box);
                GUILayout.Label($"发现 · {_lastDiscoveryTitle}");
                GUILayout.Label(_lastDiscoveryBody);
                GUILayout.EndArea();
            }
        }
    }
}
