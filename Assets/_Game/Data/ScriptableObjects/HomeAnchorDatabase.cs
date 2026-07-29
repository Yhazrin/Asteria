using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Database of build anchors for the home planet.
    /// Contains 6 Large/Medium anchors + 20 Small anchors as required.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Home Anchor Database")]
    public sealed class HomeAnchorDatabase : ScriptableObject
    {
        [Header("Anchors")]
        public AnchorData[] anchors = new AnchorData[]
        {
            // Large anchors (4-6)
            new AnchorData
            {
                anchorId = "anchor_observatory",
                displayName = "观测台锚点",
                size = "Large",
                localDirection = new Vector3(0, 1, 0),
                allowedFacilityTypes = new[] { "observation", "social" },
                description = "星球最高点，适合建造观测台或社交设施。"
            },
            new AnchorData
            {
                anchorId = "anchor_plaza",
                displayName = "广场锚点",
                size = "Large",
                localDirection = new Vector3(0.6f, 0.7f, 0).normalized,
                allowedFacilityTypes = new[] { "social", "production" },
                description = "社区中心位置，适合建造广场或工坊。"
            },
            new AnchorData
            {
                anchorId = "anchor_departure",
                displayName = "出发锚点",
                size = "Large",
                localDirection = new Vector3(1, 0, 0.5f).normalized,
                allowedFacilityTypes = new[] { "transport" },
                description = "远征出发点，适合建造交通设施。"
            },
            new AnchorData
            {
                anchorId = "anchor_greenhouse",
                displayName = "温室锚点",
                size = "Large",
                localDirection = new Vector3(-0.5f, 0.8f, 0).normalized,
                allowedFacilityTypes = new[] { "ecology" },
                description = "阳光充足的区域，适合建造温室。"
            },
            // Medium anchors (8-12)
            new AnchorData
            {
                anchorId = "anchor_residence_01",
                displayName = "住宅锚点1",
                size = "Medium",
                localDirection = new Vector3(0.3f, 0.9f, 0.2f).normalized,
                allowedFacilityTypes = new[] { "residential" },
                description = "安静的住宅区位置。"
            },
            new AnchorData
            {
                anchorId = "anchor_residence_02",
                displayName = "住宅锚点2",
                size = "Medium",
                localDirection = new Vector3(-0.2f, 0.9f, 0.3f).normalized,
                allowedFacilityTypes = new[] { "residential" },
                description = "另一个住宅区位置。"
            },
            new AnchorData
            {
                anchorId = "anchor_workshop",
                displayName = "工坊锚点",
                size = "Medium",
                localDirection = new Vector3(0.5f, 0.7f, -0.3f).normalized,
                allowedFacilityTypes = new[] { "production" },
                description = "适合建造工坊的位置。"
            },
            new AnchorData
            {
                anchorId = "anchor_memorial",
                displayName = "纪念馆锚点",
                size = "Medium",
                localDirection = new Vector3(-0.3f, 0.8f, -0.4f).normalized,
                allowedFacilityTypes = new[] { "memory" },
                description = "安静的位置，适合建造纪念馆。"
            },
            new AnchorData
            {
                anchorId = "anchor_kitchen",
                displayName = "厨房锚点",
                size = "Medium",
                localDirection = new Vector3(0.4f, 0.8f, 0.3f).normalized,
                allowedFacilityTypes = new[] { "social" },
                description = "适合建造共享厨房的位置。"
            },
            new AnchorData
            {
                anchorId = "anchor_garden_01",
                displayName = "花园锚点1",
                size = "Medium",
                localDirection = new Vector3(-0.4f, 0.7f, 0.5f).normalized,
                allowedFacilityTypes = new[] { "ecology" },
                description = "适合建造花园的位置。"
            },
            new AnchorData
            {
                anchorId = "anchor_garden_02",
                displayName = "花园锚点2",
                size = "Medium",
                localDirection = new Vector3(0.2f, 0.8f, -0.5f).normalized,
                allowedFacilityTypes = new[] { "ecology" },
                description = "另一个花园位置。"
            },
            new AnchorData
            {
                anchorId = "anchor_transport",
                displayName = "交通锚点",
                size = "Medium",
                localDirection = new Vector3(0.7f, 0.5f, 0.3f).normalized,
                allowedFacilityTypes = new[] { "transport" },
                description = "适合建造交通设施的位置。"
            },
            new AnchorData
            {
                anchorId = "anchor_residence_03",
                displayName = "住宅锚点3",
                size = "Medium",
                localDirection = new Vector3(-0.5f, 0.7f, -0.2f).normalized,
                allowedFacilityTypes = new[] { "residential" },
                description = "第三个住宅区位置。"
            },
            new AnchorData
            {
                anchorId = "anchor_residence_04",
                displayName = "住宅锚点4",
                size = "Medium",
                localDirection = new Vector3(0.1f, 0.9f, -0.4f).normalized,
                allowedFacilityTypes = new[] { "residential" },
                description = "第四个住宅区位置。"
            },
            new AnchorData
            {
                anchorId = "anchor_residence_05",
                displayName = "住宅锚点5",
                size = "Medium",
                localDirection = new Vector3(-0.6f, 0.6f, 0.3f).normalized,
                allowedFacilityTypes = new[] { "residential" },
                description = "第五个住宅区位置。"
            },
            new AnchorData
            {
                anchorId = "anchor_residence_06",
                displayName = "住宅锚点6",
                size = "Medium",
                localDirection = new Vector3(0.3f, 0.6f, -0.6f).normalized,
                allowedFacilityTypes = new[] { "residential" },
                description = "第六个住宅区位置。"
            },
        };
    }

    [System.Serializable]
    public class AnchorData
    {
        public string anchorId;
        public string displayName;
        public string size;
        public Vector3 localDirection;
        public string[] allowedFacilityTypes;
        [TextArea(1, 3)] public string description;
    }
}
