using Asteria.Interaction;
using Asteria.Persistence;
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

        [SerializeField] string homeSceneName = "HomePlanet";
        [SerializeField] string expeditionSceneName = "SphereMoveDemo";

        SaveService _saveService;
        DiscoveryRepository _discoveryRepository;
        DiscoveryJournal _subscribedJournal;
        bool _initialized;

        public static GameBootstrap Instance => _instance;

        public ISaveService SaveService => _saveService;
        public IDiscoveryRepository DiscoveryRepository => _discoveryRepository;

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

            // Initialize save service
            _saveService = new SaveService();
            _saveService.LoadOrCreate();

            // Initialize discovery repository from save data
            _discoveryRepository = new DiscoveryRepository(_saveService);

            // Wire DiscoveryJournal to use the repository
            WireDiscoveryJournal();

            _initialized = true;
            Debug.Log("[Asteria] GameBootstrap initialized.");
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
