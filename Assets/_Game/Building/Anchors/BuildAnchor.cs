using UnityEngine;

namespace Asteria.Building
{
    /// <summary>
    /// A fixed placement anchor on the home planet surface.
    /// Residents and facilities reference anchors by ID.
    /// </summary>
    public sealed class BuildAnchor : MonoBehaviour
    {
        [SerializeField] string anchorId = "anchor_default";
        [SerializeField] AnchorSize size = AnchorSize.Medium;
        [SerializeField] string[] allowedFacilityTypes = { };

        FacilityState _installedFacility;

        public string AnchorId => anchorId;
        public AnchorSize Size => size;
        public string[] AllowedFacilityTypes => allowedFacilityTypes;
        public bool IsEmpty => _installedFacility == null;
        public FacilityState InstalledFacility => _installedFacility;

        /// <summary>
        /// Install a facility at this anchor. Returns true if successful.
        /// </summary>
        public bool TryInstall(FacilityState facility)
        {
            if (!IsEmpty)
            {
                Debug.LogWarning($"[Asteria] Anchor {anchorId} already has a facility.");
                return false;
            }

            if (!IsFacilityAllowed(facility))
            {
                Debug.LogWarning($"[Asteria] Facility {facility.FacilityId} not allowed at anchor {anchorId}.");
                return false;
            }

            _installedFacility = facility;
            facility.OnInstalled(this);
            Debug.Log($"[Asteria] Installed {facility.DisplayName} at anchor {anchorId}.");
            return true;
        }

        /// <summary>
        /// Remove the installed facility.
        /// </summary>
        public void RemoveFacility()
        {
            if (_installedFacility != null)
            {
                _installedFacility.OnRemoved();
                _installedFacility = null;
            }
        }

        bool IsFacilityAllowed(FacilityState facility)
        {
            if (allowedFacilityTypes == null || allowedFacilityTypes.Length == 0)
            {
                return true;
            }

            foreach (var allowed in allowedFacilityTypes)
            {
                if (allowed == facility.FacilityType)
                {
                    return true;
                }
            }

            return false;
        }
    }

    public enum AnchorSize { Large, Medium, Small }
}
