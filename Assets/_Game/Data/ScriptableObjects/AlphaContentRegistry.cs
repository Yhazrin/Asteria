using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Registry of all content for the Alpha build.
    /// Lists all residents, events, wishes, POIs, and facilities
    /// that must be present for the first testable build.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Alpha Content Registry")]
    public sealed class AlphaContentRegistry : ScriptableObject
    {
        [Header("Home Planet")]
        public int targetResidentCount = 6;
        public int targetFacilityCount = 3;
        public int targetSocialEventCount = 12;
        public int targetWishCount = 6;

        [Header("Expedition")]
        public string expeditionArchetypeId = "wind_grassland";
        public int targetPoiCount = 8;
        public int targetPressureEventCount = 1;
        public int targetRestoreChainCount = 1;
        public int targetCooperateChainCount = 1;

        [Header("Multiplayer")]
        public int minPlayers = 1;
        public int maxPlayers = 4;

        [Header("Quality Gates")]
        public float targetFps = 60f;
        public float maxSessionMinutes = 30f;
        public bool requireSaveUpgrade = true;
        public bool requireNewPlayerFlow = true;

        /// <summary>
        /// Validate that the current content meets Alpha requirements.
        /// </summary>
        public ValidationResult Validate(
            int residentCount,
            int facilityCount,
            int eventCount,
            int wishCount,
            int poiCount)
        {
            var errors = new System.Collections.Generic.List<string>();

            if (residentCount < targetResidentCount)
                errors.Add($"Residents: {residentCount}/{targetResidentCount}");
            if (facilityCount < targetFacilityCount)
                errors.Add($"Facilities: {facilityCount}/{targetFacilityCount}");
            if (eventCount < targetSocialEventCount)
                errors.Add($"Events: {eventCount}/{targetSocialEventCount}");
            if (wishCount < targetWishCount)
                errors.Add($"Wishes: {wishCount}/{targetWishCount}");
            if (poiCount < targetPoiCount)
                errors.Add($"POIs: {poiCount}/{targetPoiCount}");

            return new ValidationResult(errors);
        }
    }

    public class ValidationResult
    {
        public System.Collections.Generic.IReadOnlyList<string> Errors { get; }
        public bool IsValid => Errors.Count == 0;

        public ValidationResult(System.Collections.Generic.List<string> errors)
        {
            Errors = errors.AsReadOnly();
        }
    }
}
