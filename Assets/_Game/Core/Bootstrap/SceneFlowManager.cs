using Asteria.Interaction;
using Asteria.Persistence;
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
            // Ensure DiscoveryJournal exists and is synced from save
            SyncDiscoveryFromSave();

            // Wire the "return to expedition" trigger
            Debug.Log("[Asteria] Home scene ready. Walk to the expedition beacon to depart.");
        }

        void SetupExpeditionScene()
        {
            // Ensure DiscoveryJournal exists and is synced from save
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
        /// Called by UI/interaction to start expedition.
        /// </summary>
        public void StartExpedition()
        {
            _bootstrap?.SaveService?.Save();
            SceneManager.LoadScene(expeditionSceneName);
        }
    }
}
