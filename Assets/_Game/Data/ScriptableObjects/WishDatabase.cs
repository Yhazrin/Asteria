using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Database of all wishes for the home planet.
    /// Contains 6+ wishes as required by Milestone I.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Wish Database")]
    public sealed class WishDatabase : ScriptableObject
    {
        [Header("Wishes")]
        public WishData[] wishes = new WishData[]
        {
            new WishData
            {
                wishId = "wish_hear_wind_bell",
                title = "想听完整的风铃石声音",
                description = "居民对远征中发现的风铃石很感兴趣，想听完整的声音。",
                residentPersonality = "curiosity > 0.4",
                requiredExpedition = "wind_grassland",
                requiredDiscovery = "observe_wind_bell_01",
                fulfillmentText = "居民听完风铃石声音后很开心，关系改善。",
                rewardType = "affinity",
                rewardValue = 0.15f
            },
            new WishData
            {
                wishId = "wish_see_aurora",
                title = "想看一次极光",
                description = "居民听说远征星球上有极光现象，很想亲眼看看。",
                residentPersonality = "curiosity > 0.3",
                requiredExpedition = "wind_grassland",
                requiredDiscovery = "bipolar_resonance",
                fulfillmentText = "家园出现一夜极光，居民们聚在一起观赏。",
                rewardType = "affinity",
                rewardValue = 0.2f
            },
            new WishData
            {
                wishId = "wish_blue_plant",
                title = "想要一种蓝色植物",
                description = "居民在远征照片里看到一种蓝色植物，很想要一棵。",
                residentPersonality = "warmth > 0.3",
                requiredExpedition = "wind_grassland",
                requiredDiscovery = "seed_or_nest",
                fulfillmentText = "居民收到蓝色植物种子，在温室种植。",
                rewardType = "affinity",
                rewardValue = 0.1f
            },
            new WishData
            {
                wishId = "wish_make_wind_chime",
                title = "想制作风铃",
                description = "居民想用远征带回的材料制作风铃挂在窗边。",
                residentPersonality = "warmth > 0.4",
                requiredExpedition = "wind_grassland",
                requiredDiscovery = "silent_bell",
                fulfillmentText = "风铃广场出现新风铃装置，居民很高兴。",
                rewardType = "facility",
                rewardValue = 1f
            },
            new WishData
            {
                wishId = "wish_not_talking",
                title = "想找不尴尬的活动",
                description = "居民和室友最近说不上话，想找一个不尴尬的活动一起做。",
                residentPersonality = "sociability < -0.2",
                requiredExpedition = "wind_grassland",
                requiredDiscovery = "lost_traveler",
                fulfillmentText = "共同经历远征后关系改善，不再尴尬。",
                rewardType = "relationship",
                rewardValue = 0.15f
            },
            new WishData
            {
                wishId = "wish_reliable_friend",
                title = "想知道谁最可靠",
                description = "居民想在紧急情况下知道谁最可靠。",
                residentPersonality = "boldness > 0.3",
                requiredExpedition = "wind_grassland",
                requiredDiscovery = "global_wind",
                fulfillmentText = "居民讨论谁在风暴中最可靠，找到了答案。",
                rewardType = "trust",
                rewardValue = 0.2f
            },
        };
    }

    [System.Serializable]
    public class WishData
    {
        public string wishId;
        public string title;
        [TextArea(2, 4)] public string description;
        public string residentPersonality;
        public string requiredExpedition;
        public string requiredDiscovery;
        [TextArea(2, 4)] public string fulfillmentText;
        public string rewardType;
        public float rewardValue;
    }
}
