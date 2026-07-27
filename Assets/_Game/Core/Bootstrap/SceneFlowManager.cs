using Asteria.Interaction;
using Asteria.Expedition;
using Asteria.Persistence;
using Asteria.Residents;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Asteria.Core
{
    /// <summary>
    /// Manages the Home ↔ Expedition scene flow.
    /// Lives in the Bootstrap scene and survives scene loads.
    /// </summary>
    public sealed class SceneFlowManager : MonoBehaviour
    {
        static SceneFlowManager _instance;

        [SerializeField] string homeSceneName = "HomePlanet";
        [SerializeField] string expeditionSceneName = "SphereMoveDemo";

        GameBootstrap _bootstrap;
        ExpeditionResult _pendingResult;

        public static SceneFlowManager Instance => _instance;

        void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);
            _bootstrap = GameBootstrap.Instance;
        }

        void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            Debug.Log($"[Asteria] Scene loaded: {scene.name}");

            if (scene.name == homeSceneName)
            {
                SetupHomeScene();
            }
            else if (scene.name == expeditionSceneName)
            {
                SetupExpeditionScene();
            }
        }

        void SetupHomeScene()
        {
            SyncDiscoveryFromSave();

            // Settle pending expedition result
            if (_pendingResult != null)
            {
                var manager = FindFirstObjectByType<ResidentManager>();
                ExpeditionSettlement.Settle(_pendingResult, _bootstrap?.SaveService, manager);
                _pendingResult = null;
            }

            Debug.Log("[Asteria] Home scene ready.");
        }

        void SetupExpeditionScene()
        {
            SyncDiscoveryFromSave();
            Debug.Log("[Asteria] Expedition scene ready.");
        }

        void SyncDiscoveryFromSave()
        {
            if (_bootstrap?.DiscoveryRepository == null)
            {
                return;
            }

            // The DiscoveryJournal is scene-scoped; recreate it from save data
            var journal = DiscoveryJournal.Instance;

            foreach (var record in _bootstrap.DiscoveryRepository.GetAll())
            {
                // Use reflection-free approach: create a temporary ObserveEntry
                var entry = ScriptableObject.CreateInstance<Data.ObserveEntry>();
                entry.id = record.id;
                entry.displayName = record.displayName;
                journal.TryUnlock(entry);
                Destroy(entry);
            }
        }

        /// <summary>
        /// Called by UI/interaction to go home.
        /// </summary>
        public void GoHome()
        {
            _bootstrap?.SaveService?.Save();
            SceneManager.LoadScene(homeSceneName);
        }

        /// <summary>
        /// Called by UI/interaction to go home with expedition results.
        /// </summary>
        public void GoHomeWithResult(ExpeditionResult result)
        {
            _pendingResult = result;
            _bootstrap?.SaveService?.Save();
            SceneManager.LoadScene(homeSceneName);
        }

        /// <summary>
        /// Called by UI/interaction to start expedition.
        /// </summary>
        public void StartExpedition()
        {
            _bootstrap?.SaveService?.Save();
            SceneManager.LoadScene(expeditionSceneName);
        }
    }
}
