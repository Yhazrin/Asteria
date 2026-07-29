using Asteria.Data;
using Asteria.Interaction;
using Asteria.Persistence;
using Asteria.Planet.Creatures;
using Asteria.Residents;
using Asteria.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Asteria.Core
{
    /// <summary>
    /// Composition root that persists across scenes. Initializes all services
    /// and manages the save/load lifecycle.
    /// Attach to a GameObject in the Bootstrap scene.
    /// </summary>
    public sealed class GameBootstrap : MonoBehaviour
    {
        static GameBootstrap _instance;

        [SerializeField] string homeSceneName = AsteriaConstants.HomeSceneName;
        [SerializeField] string expeditionSceneName = AsteriaConstants.ExpeditionSceneName;

        SaveService _saveService;
        DiscoveryRepository _discoveryRepository;
        DiscoveryJournal _subscribedJournal;
        GameClock _gameClock;
        WorldStateService _worldState;
        RelationshipService _relationships;
        bool _initialized;

        public static GameBootstrap Instance => _instance;

        public ISaveService SaveService => _saveService;
        public IDiscoveryRepository DiscoveryRepository => _discoveryRepository;
        public IGameClock GameClock => _gameClock;
        public IWorldStateService WorldState => _worldState;
        public IRelationshipService Relationships => _relationships;

        public string HomeSceneName => homeSceneName;
        public string ExpeditionSceneName => expeditionSceneName;

        void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);
            Initialize();
        }

        void Initialize()
        {
            if (_initialized)
            {
                return;
            }

            // Initialize core services
            _gameClock = gameObject.AddComponent<GameClock>();
            _worldState = gameObject.AddComponent<WorldStateService>();
            _relationships = gameObject.AddComponent<RelationshipService>();

            // Initialize save service
            _saveService = new SaveService();
            _saveService.LoadOrCreate();

            // Initialize discovery repository from save data
            _discoveryRepository = new DiscoveryRepository(_saveService);

            // Ensure default content registry exists
            EnsureDefaultContent();

            // Ensure UI system exists
            EnsureUISystem();

            // Wire DiscoveryJournal to use the repository
            WireDiscoveryJournal();

            _initialized = true;
            Debug.Log("[Asteria] GameBootstrap initialized with all systems.");
        }

        void EnsureDefaultContent()
        {
            if (DefaultContentRegistry.Instance == null)
            {
                var go = new GameObject("DefaultContentRegistry");
                go.AddComponent<DefaultContentRegistry>();
            }

            // Planet codex persists across scenes
            if (PlanetCodex.Instance == null)
            {
                var codexGo = new GameObject("PlanetCodex");
                codexGo.AddComponent<PlanetCodex>();
            }
        }

        void EnsureUISystem()
        {
            // GameUIRoot will auto-create if needed
            var uiRoot = GameUIRoot.Instance;
        }

        void WireDiscoveryJournal()
        {
            _subscribedJournal = DiscoveryJournal.Instance;
            if (_subscribedJournal != null)
            {
                _subscribedJournal.DiscoveryUnlocked += OnDiscoveryUnlocked;
            }
        }

        void OnDiscoveryUnlocked(Data.ObserveEntry entry)
        {
            if (_discoveryRepository != null && entry != null)
            {
                _discoveryRepository.Record(entry.id, entry.displayName);
                _saveService.Save();
            }
        }

        /// <summary>
        /// Loads the home planet scene.
        /// </summary>
        public void GoHome()
        {
            Debug.Log("[Asteria] Returning home...");
            SceneManager.LoadScene(homeSceneName);
        }

        /// <summary>
        /// Loads the expedition scene.
        /// </summary>
        public void StartExpedition()
        {
            Debug.Log("[Asteria] Starting expedition...");
            SceneManager.LoadScene(expeditionSceneName);
        }

        void OnApplicationQuit()
        {
            _saveService?.Save();
        }

        void OnDestroy()
        {
            if (_instance == this)
            {
                if (_subscribedJournal != null)
                {
                    _subscribedJournal.DiscoveryUnlocked -= OnDiscoveryUnlocked;
                    _subscribedJournal = null;
                }

                _instance = null;
            }
        }
    }
}
