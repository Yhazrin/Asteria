using Asteria.Expedition;
using Asteria.Interaction;
using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Creates all default content definitions for the Alpha build.
    /// Called by GameBootstrap to populate the content registry.
    /// Implements the content matrix from WORLD_CONTENT_MATRIX.md §7.
    /// </summary>
    public static class DefaultContentFactory
    {
        /// <summary>
        /// Create the Wind Grassland planet archetype with all POIs and events.
        /// </summary>
        public static PlanetArchetypeDefinition CreateWindGrassland()
        {
            var archetype = ScriptableObject.CreateInstance<PlanetArchetypeDefinition>();
            archetype.archetypeId = "wind_grassland";
            archetype.displayName = "风之草原";
            archetype.description = "开阔坡地、风带、峡谷。主要压力：强风、失衡。";
            archetype.planetRadius = 300f;

            archetype.poiSlots = new PoiSlotDefinition[]
            {
                CreatePoiSlot("wind_bell_01", "Observe", Vector3.forward),
                CreatePoiSlot("wind_bell_02", "Observe", Vector3.back),
                CreatePoiSlot("wind_tower_01", "Restore", Vector3.right),
                CreatePoiSlot("bipolar_gate", "Cooperate", Vector3.up),
                CreatePoiSlot("shelter_cave", "Shelter", (Vector3.forward + Vector3.right).normalized),
                CreatePoiSlot("vista_peak", "Vista", Vector3.up + Vector3.forward),
                CreatePoiSlot("lost_traveler", "Social", (Vector3.back + Vector3.left).normalized),
                CreatePoiSlot("seed_choice", "Choice", Vector3.down),
            };

            return archetype;
        }

        /// <summary>
        /// Create the Wind Grassland biome definition.
        /// </summary>
        public static BiomeDefinition CreateWindGrasslandBiome()
        {
            var biome = ScriptableObject.CreateInstance<BiomeDefinition>();
            biome.biomeId = "wind_grassland";
            biome.displayName = "风之草原";
            biome.biomeType = BiomeType.Wind;
            biome.moodTags = new[] { "Curious", "Wondrous", "Cozy" };
            biome.pressureTypes = new[] { "Wind" };
            biome.ambientColor = new Color(0.55f, 0.7f, 0.55f);
            return biome;
        }

        /// <summary>
        /// Create the 8 world events for the Wind Grassland expedition.
        /// </summary>
        public static WorldEventDefinition[] CreateWindGrasslandEvents()
        {
            return new[]
            {
                CreateEvent("wind_direction_test", "风向初测", ExpeditionPhase.Arrival,
                    new[] { "Observe" }, "多角度校准风向"),

                CreateEvent("silent_bell", "失声的风铃石", ExpeditionPhase.Invitation,
                    new[] { "Observe", "Care" }, "一人寻找，一人照明"),

                CreateEvent("wind_beast_migration", "风兽迁徙", ExpeditionPhase.Invitation,
                    new[] { "Traverse" }, "分头占据观测点"),

                CreateEvent("lost_traveler", "迷路的小旅人", ExpeditionPhase.Complication,
                    new[] { "Social", "Traverse" }, "包围式引导"),

                CreateEvent("tower_blades", "风塔叶片散落", ExpeditionPhase.Complication,
                    new[] { "Restore" }, "搬运与安装分工"),

                CreateEvent("global_wind", "全球强风", ExpeditionPhase.Pressure,
                    new[] { "Traverse" }, "信标链与牵引绳更重要"),

                CreateEvent("bipolar_resonance", "双极共鸣", ExpeditionPhase.Resolution,
                    new[] { "Cooperate" }, "两侧同时完成"),

                CreateEvent("seed_or_nest", "留下种子或修复巢穴", ExpeditionPhase.Resolution,
                    new[] { "Care", "Restore" }, "全队投票"),
            };
        }

        /// <summary>
        /// Create the default tool definitions.
        /// </summary>
        public static ToolDefinition[] CreateDefaultTools()
        {
            return new[]
            {
                CreateTool("resonance_mirror", "共鸣镜", ToolSlotType.Active1,
                    "扫描声音、生命和遗迹信号", new[] { "observe", "scan" }),

                CreateTool("warm_light", "暖光灯", ToolSlotType.Active2,
                    "建立短时安全区", new[] { "cold", "dark", "safe_zone" }),

                CreateTool("beacon", "信标", ToolSlotType.SharedBeacon,
                    "标记路线和安全点", new[] { "navigation", "rescue" }),
            };
        }

        /// <summary>
        /// Create the default social events for the home planet.
        /// </summary>
        public static SocialEventDefinition[] CreateDefaultSocialEvents()
        {
            return new[]
            {
                CreateSocialEvent("cooking_fail", "做饭失败", EventCategory.Daily,
                    "一位居民尝试做饭但失败了", 2, 0.5f),

                CreateSocialEvent("gift_mixup", "误拿礼物", EventCategory.Daily,
                    "两位居民交换了彼此的礼物", 2, 1f),

                CreateSocialEvent("window_seat", "争抢窗边位置", EventCategory.Conflict,
                    "两位居民都想坐在窗边", 2, 0.5f),

                CreateSocialEvent("expedition_story", "分享远征故事", EventCategory.Relationship,
                    "一位居民向另一位讲述远征经历", 2, 2f),

                CreateSocialEvent("conflict_late", "约定迟到", EventCategory.Conflict,
                    "一位居民迟到引起了不满", 2, 1f),

                CreateSocialEvent("community_festival", "社区庆典", EventCategory.Community,
                    "居民们举办小型庆祝活动", 3, 5f),
            };
        }

        // Helper methods
        static PoiSlotDefinition CreatePoiSlot(string id, string type, Vector3 dir)
        {
            return new PoiSlotDefinition
            {
                slotId = id,
                poiType = type,
                localDirection = SerializableVector3.From(dir.normalized),
                contentTags = new[] { type.ToLower() }
            };
        }

        static WorldEventDefinition CreateEvent(string id, string title, ExpeditionPhase phase,
            string[] actions, string description)
        {
            var evt = ScriptableObject.CreateInstance<WorldEventDefinition>();
            evt.eventId = id;
            evt.title = title;
            evt.description = description;
            evt.phase = phase;
            evt.biomeTags = new[] { "Wind" };
            evt.minPlayers = 1;
            evt.maxPlayers = 4;
            evt.durationMinSeconds = 60f;
            evt.durationMaxSeconds = 300f;
            return evt;
        }

        static ToolDefinition CreateTool(string id, string name, ToolSlotType slot,
            string desc, string[] tags)
        {
            var tool = ScriptableObject.CreateInstance<ToolDefinition>();
            tool.toolId = id;
            tool.displayName = name;
            tool.slotType = slot;
            tool.description = desc;
            tool.maxEnergy = 100f;
            tool.rechargeRate = 5f;
            tool.interactionTags = tags;
            return tool;
        }

        static SocialEventDefinition CreateSocialEvent(string id, string title,
            EventCategory category, string desc, int participants, float cooldown)
        {
            var evt = ScriptableObject.CreateInstance<SocialEventDefinition>();
            evt.eventId = id;
            evt.title = title;
            evt.category = category;
            evt.description = desc;
            evt.minParticipants = participants;
            evt.maxParticipants = participants;
            evt.cooldownDays = cooldown;
            return evt;
        }
    }
}
