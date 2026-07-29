using UnityEngine;
using UnityEngine.UI;

namespace Asteria.UI
{
    /// <summary>
    /// Main game UI root. Creates all UI panels dynamically at runtime.
    /// No Prefab required — built entirely in code.
    /// </summary>
    public sealed class GameUIRoot : MonoBehaviour
    {
        static GameUIRoot _instance;

        Canvas _canvas;
        HUDPanel _hud;
        DiscoveryPopup _discoveryPopup;
        InteractionPrompt _interactionPrompt;
        ExpeditorPanel _expeditionPanel;
        SaveLoadPanel _saveLoadPanel;
        ResidentInteractionPanel _residentPanel;
        CodexPanel _codexPanel;

        public static GameUIRoot Instance
        {
            get
            {
                if (_instance == null)
                {
                    var go = new GameObject("GameUIRoot");
                    _instance = go.AddComponent<GameUIRoot>();
                }

                return _instance;
            }
        }

        public HUDPanel HUD => _hud;
        public DiscoveryPopup DiscoveryPopup => _discoveryPopup;
        public InteractionPrompt InteractionPrompt => _interactionPrompt;
        public ExpeditorPanel ExpeditionPanel => _expeditionPanel;
        public SaveLoadPanel SaveLoadPanel => _saveLoadPanel;
        public ResidentInteractionPanel ResidentPanel => _residentPanel;
        public CodexPanel CodexPanel => _codexPanel;

        void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);
            BuildUI();
        }

        void BuildUI()
        {
            // Create Canvas
            var canvasGo = new GameObject("UICanvas");
            canvasGo.transform.SetParent(transform, false);
            _canvas = canvasGo.AddComponent<Canvas>();
            _canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            _canvas.sortingOrder = 100;

            var scaler = canvasGo.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            scaler.matchWidthOrHeight = 0.5f;

            canvasGo.AddComponent<GraphicRaycaster>();

            // Build panels
            _hud = new HUDPanel(canvasGo.transform);
            _discoveryPopup = new DiscoveryPopup(canvasGo.transform);
            _interactionPrompt = new InteractionPrompt(canvasGo.transform);
            _expeditionPanel = new ExpeditorPanel(canvasGo.transform);

            // Add panel components
            var saveLoadGo = new GameObject("SaveLoadPanel");
            saveLoadGo.transform.SetParent(transform, false);
            _saveLoadPanel = saveLoadGo.AddComponent<SaveLoadPanel>();

            var residentGo = new GameObject("ResidentPanel");
            residentGo.transform.SetParent(transform, false);
            _residentPanel = residentGo.AddComponent<ResidentInteractionPanel>();

            var codexGo = new GameObject("CodexPanel");
            codexGo.transform.SetParent(transform, false);
            _codexPanel = codexGo.AddComponent<CodexPanel>();
        }

        void OnDestroy()
        {
            if (_instance == this)
            {
                _instance = null;
            }
        }
    }
}
