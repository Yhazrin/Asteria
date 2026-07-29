using UnityEngine;

namespace Asteria.Building
{
    /// <summary>
    /// Shows a preview of a facility before placement.
    /// Allows rotation and validates placement.
    /// </summary>
    public sealed class BuildingPreview : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float rotationSpeed = 90f;
        [SerializeField] float gridSize = 1f;
        [SerializeField] Color validColor = new(0.3f, 0.8f, 0.3f, 0.5f);
        [SerializeField] Color invalidColor = new(0.8f, 0.3f, 0.3f, 0.5f);

        GameObject _previewObject;
        BuildAnchor _currentAnchor;
        FacilityDefinition _currentFacility;
        float _currentRotation;
        bool _isValid;
        bool _isPlacing;

        Material _previewMaterial;

        /// <summary>
        /// Start placing a facility.
        /// </summary>
        public void StartPlacement(FacilityDefinition facility, BuildAnchor anchor)
        {
            _currentFacility = facility;
            _currentAnchor = anchor;
            _currentRotation = 0f;
            _isPlacing = true;

            CreatePreview();
            UpdatePreviewPosition();
        }

        /// <summary>
        /// Cancel placement.
        /// </summary>
        public void CancelPlacement()
        {
            _isPlacing = false;
            DestroyPreview();
        }

        /// <summary>
        /// Confirm placement.
        /// </summary>
        public bool ConfirmPlacement()
        {
            if (!_isPlacing || !_isValid) return false;

            var buildingSystem = FindFirstObjectByType<BuildingSystem>();
            if (buildingSystem == null) return false;

            bool success = buildingSystem.TryBuild(
                _currentFacility.facilityId,
                _currentAnchor.AnchorId,
                _currentRotation);

            if (success)
            {
                _isPlacing = false;
                DestroyPreview();
                Debug.Log($"[Building] Placed {_currentFacility.displayName}");
            }

            return success;
        }

        void Update()
        {
            if (!_isPlacing) return;

            // Rotation input
            if (Input.GetKey(KeyCode.R))
            {
                _currentRotation += rotationSpeed * Time.deltaTime;
                if (_currentRotation >= 360f) _currentRotation -= 360f;
            }

            UpdatePreviewPosition();
            UpdateValidity();
        }

        void CreatePreview()
        {
            if (_previewObject != null) DestroyPreview();

            // Create preview mesh
            _previewObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            _previewObject.name = "BuildingPreview";
            _previewObject.transform.localScale = _currentFacility.previewScale;

            // Remove collider from preview
            var col = _previewObject.GetComponent<Collider>();
            if (col != null) Destroy(col);

            // Create preview material
            _previewMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit")
                ?? Shader.Find("Sprites/Default"));
            _previewMaterial.color = validColor;

            var renderer = _previewObject.GetComponent<MeshRenderer>();
            if (renderer != null)
            {
                renderer.material = _previewMaterial;
                // Make transparent
                if (renderer.material.HasProperty("_Surface"))
                {
                    renderer.material.SetFloat("_Surface", 1); // Transparent
                }
            }
        }

        void DestroyPreview()
        {
            if (_previewObject != null)
            {
                Destroy(_previewObject);
                _previewObject = null;
            }
        }

        void UpdatePreviewPosition()
        {
            if (_previewObject == null || _currentAnchor == null) return;

            _previewObject.transform.position = _currentAnchor.transform.position + Vector3.up * 2f;
            _previewObject.transform.rotation = Quaternion.Euler(0f, _currentRotation, 0f);
        }

        void UpdateValidity()
        {
            if (_currentAnchor == null || _currentFacility == null)
            {
                _isValid = false;
                return;
            }

            // Check if anchor accepts this facility type
            _isValid = _currentAnchor.IsEmpty;

            if (_currentAnchor.AllowedFacilityTypes != null && _currentAnchor.AllowedFacilityTypes.Length > 0)
            {
                bool typeAllowed = false;
                foreach (var allowed in _currentAnchor.AllowedFacilityTypes)
                {
                    if (allowed == _currentFacility.facilityType)
                    {
                        typeAllowed = true;
                        break;
                    }
                }
                _isValid = _isValid && typeAllowed;
            }

            // Update preview color
            if (_previewMaterial != null)
            {
                _previewMaterial.color = _isValid ? validColor : invalidColor;
            }
        }

        void OnDestroy()
        {
            DestroyPreview();
        }
    }
}
