using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Defines a Point of Interest (兴趣点) on a planet surface.
    /// POIs are interaction targets for Observe, Restore, Cooperate, etc.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/POI Definition")]
    public sealed class PoiDefinition : ScriptableObject
    {
        public string poiId = "poi_default";
        public string displayName = "兴趣点";
        public PoiType poiType = PoiType.Observe;

        [Header("Position")]
        public Vector3 localDirection = Vector3.forward;

        [Header("Requirements")]
        public string[] requiredTools = { };
        public string[] contentTags = { };

        [Header("Content")]
        public string linkedEventId;
        public string linkedObserveEntryId;
    }

    public enum PoiType
    {
        Observe,
        Restore,
        Cooperate,
        Shelter,
        Social,
        Choice,
        Vista
    }
}
