using UnityEngine;

namespace Asteria.Residents
{
    /// <summary>
    /// Creates the default wishes for the Alpha build.
    /// Implements the wish loop from ROADMAP_V2.md Milestone D.
    /// </summary>
    public static class DefaultWishes
    {
        /// <summary>
        /// Create the default wishes that connect home to expedition.
        /// </summary>
        public static WishDefinition[] CreateDefaultWishes()
        {
            return new[]
            {
                CreateWish("hear_wind_bell", "想听完整的风铃石声音",
                    "居民对远征中发现的风铃石很感兴趣，想听完整的声音。",
                    "wind_grassland", "observe_wind_bell_01",
                    "居民听完风铃石声音后很开心，关系改善。"),

                CreateWish("see_aurora", "想看一次极光",
                    "居民听说远征星球上有极光现象。",
                    "wind_grassland", "bipolar_resonance",
                    "家园出现一夜极光，居民们聚在一起观赏。"),

                CreateWish("blue_plant", "想要一种蓝色植物",
                    "居民在远征照片里看到一种蓝色植物。",
                    "wind_grassland", "seed_or_nest",
                    "居民收到蓝色植物种子，在温室种植。"),

                CreateWish("make_wind_chime", "想制作风铃",
                    "居民想用远征带回的材料制作风铃。",
                    "wind_grassland", "silent_bell",
                    "风铃广场出现新风铃装置。"),

                CreateWish("not_talking", "想找不尴尬的活动",
                    "居民和室友最近说不上话，想找不尴尬的活动。",
                    "wind_grassland", "lost_traveler",
                    "共同经历远征后关系改善。"),

                CreateWish("reliable_friend", "想知道谁最可靠",
                    "居民想在紧急情况下知道谁最可靠。",
                    "wind_grassland", "global_wind",
                    "居民讨论谁在风暴中最可靠。"),
            };
        }

        static WishDefinition CreateWish(string id, string title, string desc,
            string expeditionId, string discoveryId, string fulfillment)
        {
            var wish = ScriptableObject.CreateInstance<WishDefinition>();
            wish.wishId = id;
            wish.displayName = title;
            wish.description = desc;
            wish.requiredExpeditionId = expeditionId;
            wish.requiredDiscoveryId = discoveryId;
            wish.fulfillmentText = fulfillment;
            return wish;
        }
    }
}
