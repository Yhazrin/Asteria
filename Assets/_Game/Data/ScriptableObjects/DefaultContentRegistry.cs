using Asteria.Expedition;
using Asteria.Interaction;
using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Holds all default content created by DefaultContentFactory.
    /// Attached to the Bootstrap scene so all systems can access it.
    /// </summary>
    public sealed class DefaultContentRegistry : MonoBehaviour
    {
        static DefaultContentRegistry _instance;

        PlanetArchetypeDefinition _windGrassland;
        BiomeDefinition _windGrasslandBiome;
        WorldEventDefinition[] _worldEvents;
        ToolDefinition[] _tools;
        SocialEventDefinition[] _socialEvents;

        public static DefaultContentRegistry Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindFirstObjectByType<DefaultContentRegistry>();
                }

                return _instance;
            }
        }

        public PlanetArchetypeDefinition WindGrassland => _windGrassland;
        public BiomeDefinition WindGrasslandBiome => _windGrasslandBiome;
        public WorldEventDefinition[] WorldEvents => _worldEvents;
        public ToolDefinition[] Tools => _tools;
        public SocialEventDefinition[] SocialEvents => _socialEvents;

        void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);
            CreateAllContent();
        }

        void CreateAllContent()
        {
            _windGrassland = DefaultContentFactory.CreateWindGrassland();
            _windGrasslandBiome = DefaultContentFactory.CreateWindGrasslandBiome();
            _worldEvents = DefaultContentFactory.CreateWindGrasslandEvents();
            _tools = DefaultContentFactory.CreateDefaultTools();
            _socialEvents = DefaultContentFactory.CreateDefaultSocialEvents();

            Debug.Log($"[Asteria] Default content loaded: " +
                $"{_worldEvents.Length} world events, " +
                $"{_tools.Length} tools, " +
                $"{_socialEvents.Length} social events");
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
