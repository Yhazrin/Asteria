using UnityEngine;

namespace Asteria.Building
{
    /// <summary>
    /// Static definition of a facility that can be built on the home planet.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Facility Definition")]
    public sealed class FacilityDefinition : ScriptableObject
    {
        public string facilityId = "facility_default";
        public string displayName = "设施";
        [TextArea(2, 4)] public string description = "";
        public string facilityType = "general";
        public AnchorSize requiredAnchorSize = AnchorSize.Medium;

        [Header("Behavioral Impact")]
        public string[] unlockedScheduleSlots = { };
        public string[] unlockedEventIds = { };
        public string[] unlockedWishIds = { };

        [Header("Visual")]
        public Color previewColor = new(0.8f, 0.8f, 0.7f);
        public Vector3 previewScale = Vector3.one;
    }
}
