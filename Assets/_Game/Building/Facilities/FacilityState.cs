using System;
using UnityEngine;

namespace Asteria.Building
{
    /// <summary>
    /// Runtime state of an installed facility. Serializable for save/load.
    /// </summary>
    [Serializable]
    public class FacilityState
    {
        [SerializeField] string facilityId;
        [SerializeField] string displayName;
        [SerializeField] string facilityType;
        [SerializeField] string installedAnchorId;
        [SerializeField] float rotationAngle;
        [SerializeField] string colorVariantId;

        BuildAnchor _anchor;

        public string FacilityId => facilityId;
        public string DisplayName => displayName;
        public string FacilityType => facilityType;
        public string InstalledAnchorId => installedAnchorId;
        public float RotationAngle => rotationAngle;
        public BuildAnchor Anchor => _anchor;

        public FacilityState(FacilityDefinition definition)
        {
            facilityId = definition.facilityId;
            displayName = definition.displayName;
            facilityType = definition.facilityType;
        }

        public void OnInstalled(BuildAnchor anchor)
        {
            _anchor = anchor;
            installedAnchorId = anchor.AnchorId;
        }

        public void OnRemoved()
        {
            _anchor = null;
            installedAnchorId = null;
        }

        public void SetRotation(float angle)
        {
            rotationAngle = angle;
        }
    }
}
