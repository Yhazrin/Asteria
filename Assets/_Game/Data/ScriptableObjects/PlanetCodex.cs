using System;
using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Planet codex (图鉴) — tracks all discovered planet types, biomes, and creatures.
    /// Provides collection motivation and unlocks new content.
    /// </summary>
    public sealed class PlanetCodex : MonoBehaviour
    {
        static PlanetCodex _instance;

        [Header("Codex Data")]
        [SerializeField] PlanetCodexEntry[] allEntries;

        readonly Dictionary<string, PlanetCodexEntry> _discovered = new();
        readonly Dictionary<string, PlanetCodexEntry> _allEntriesMap = new();

        public static PlanetCodex Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindFirstObjectByType<PlanetCodex>();
                    if (_instance == null)
                    {
                        var go = new GameObject("PlanetCodex");
                        _instance = go.AddComponent<PlanetCodex>();
                    }
                }
                return _instance;
            }
        }

        // Events
        public event Action<PlanetCodexEntry> OnEntryDiscovered;
        public event Action<int, int> OnProgressUpdated; // discovered, total

        public int DiscoveredCount => _discovered.Count;
        public int TotalCount => _allEntriesMap.Count;
        public float CompletionRatio => TotalCount > 0 ? (float)DiscoveredCount / TotalCount : 0f;

        void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }
            _instance = this;
            DontDestroyOnLoad(gameObject);

            InitializeCodex();
        }

        void InitializeCodex()
        {
            // Register all entries
            if (allEntries != null)
            {
                foreach (var entry in allEntries)
                {
                    if (entry != null)
                    {
                        _allEntriesMap[entry.entryId] = entry;
                    }
                }
            }

            // Add default entries if none configured
            if (_allEntriesMap.Count == 0)
            {
                CreateDefaultEntries();
            }
        }

        void CreateDefaultEntries()
        {
            var defaults = new[]
            {
                CreateEntry("planet.wind_grassland", "风之草原", PlanetType.Expedition,
                    "开阔的坡地，风带和峡谷。适合滑翔和探索。", BiomeType.Wind),
                CreateEntry("planet.mist_forest", "雾声森林", PlanetType.Expedition,
                    "密林中的低能见度区域。声音是唯一的导航方式。", BiomeType.Mist),
                CreateEntry("planet.night_valley", "星砂夜谷", PlanetType.Expedition,
                    "暗色沙丘和发光路径。夜晚带来变化。", BiomeType.Night),
                CreateEntry("planet.ice_tide", "浮冰潮汐星", PlanetType.Expedition,
                    "冰壳、裂隙和热泉。潮汐改变路线。", BiomeType.Ice),
                CreateEntry("planet.bloom_garden", "花粉云庭", PlanetType.Expedition,
                    "巨花和漂浮孢团。上下风半球差异。", BiomeType.Bloom),
                CreateEntry("planet.ruin_mech", "失落机械星", PlanetType.Expedition,
                    "遗迹和轨道装置。需要双人同步修复。", BiomeType.Ruin),
                CreateEntry("planet.home", "家园星球", PlanetType.Home,
                    "你的长期社区空间。居民在这里生活。", BiomeType.Wind),
            };

            foreach (var entry in defaults)
            {
                _allEntriesMap[entry.entryId] = entry;
            }
        }

        PlanetCodexEntry CreateEntry(string id, string name, PlanetType type, string desc, BiomeType biome)
        {
            var entry = ScriptableObject.CreateInstance<PlanetCodexEntry>();
            entry.entryId = id;
            entry.displayName = name;
            entry.planetType = type;
            entry.description = desc;
            entry.primaryBiome = biome;
            entry.isDiscovered = false;
            return entry;
        }

        /// <summary>
        /// Discover a planet entry. Returns true if newly discovered.
        /// </summary>
        public bool Discover(string entryId)
        {
            if (string.IsNullOrEmpty(entryId)) return false;
            if (_discovered.ContainsKey(entryId)) return false;

            if (!_allEntriesMap.TryGetValue(entryId, out var entry))
            {
                // Create entry on the fly for procedural planets
                entry = CreateEntry(entryId, entryId, PlanetType.Expedition, "新发现的星球", BiomeType.Wind);
                _allEntriesMap[entryId] = entry;
            }

            entry.isDiscovered = true;
            entry.discoveryTime = DateTime.UtcNow.ToString("o");
            _discovered[entryId] = entry;

            OnEntryDiscovered?.Invoke(entry);
            OnProgressUpdated?.Invoke(DiscoveredCount, TotalCount);

            Debug.Log($"[Asteria] Planet codex: Discovered {entry.displayName} ({DiscoveredCount}/{TotalCount})");
            return true;
        }

        /// <summary>
        /// Check if a planet is discovered.
        /// </summary>
        public bool IsDiscovered(string entryId)
        {
            return _discovered.ContainsKey(entryId);
        }

        /// <summary>
        /// Get all discovered entries.
        /// </summary>
        public IReadOnlyList<PlanetCodexEntry> GetDiscovered()
        {
            return new List<PlanetCodexEntry>(_discovered.Values);
        }

        /// <summary>
        /// Get all entries (discovered and undiscovered).
        /// </summary>
        public IReadOnlyList<PlanetCodexEntry> GetAllEntries()
        {
            return new List<PlanetCodexEntry>(_allEntriesMap.Values);
        }

        /// <summary>
        /// Get entries by biome type.
        /// </summary>
        public List<PlanetCodexEntry> GetByBiome(BiomeType biome)
        {
            var result = new List<PlanetCodexEntry>();
            foreach (var entry in _allEntriesMap.Values)
            {
                if (entry.primaryBiome == biome) result.Add(entry);
            }
            return result;
        }

        public enum BiomeType { Wind, Mist, Night, Ice, Bloom, Ruin }
        public enum PlanetType { Home, Expedition }
    }

    /// <summary>
    /// Single entry in the planet codex.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Planet Codex Entry")]
    public sealed class PlanetCodexEntry : ScriptableObject
    {
        public string entryId;
        public string displayName;
        [TextArea(2, 4)] public string description;
        public PlanetCodex.PlanetType planetType;
        public PlanetCodex.BiomeType primaryBiome;

        [Header("Discovery")]
        public bool isDiscovered;
        public string discoveryTime;

        [Header("Visual")]
        public Color previewColor = Color.white;
        public Sprite previewImage;
    }
}
