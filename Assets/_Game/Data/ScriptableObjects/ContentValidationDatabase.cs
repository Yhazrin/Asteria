using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Content validation rules for the game.
    /// Ensures all content meets quality standards.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Content Validation Database")]
    public sealed class ContentValidationDatabase : ScriptableObject
    {
        [Header("Validation Rules")]
        public ValidationRule[] rules = new ValidationRule[]
        {
            new ValidationRule
            {
                ruleId = "val_resident_id_unique",
                description = "Resident IDs must be unique",
                category = "data",
                severity = "error"
            },
            new ValidationRule
            {
                ruleId = "val_event_id_unique",
                description = "Event IDs must be unique",
                category = "data",
                severity = "error"
            },
            new ValidationRule
            {
                ruleId = "val_poi_has_type",
                description = "POIs must have a type",
                category = "content",
                severity = "error"
            },
            new ValidationRule
            {
                ruleId = "val_biome_has_color",
                description = "Biomes must have an ambient color",
                category = "visual",
                severity = "warning"
            },
            new ValidationRule
            {
                ruleId = "val_tool_has_description",
                description = "Tools must have a description",
                category = "content",
                severity = "warning"
            },
            new ValidationRule
            {
                ruleId = "val_event_has_duration",
                description = "Events must have a duration",
                category = "gameplay",
                severity = "warning"
            },
            new ValidationRule
            {
                ruleId = "val_resident_has_personality",
                description = "Residents must have personality values",
                category = "data",
                severity = "error"
            },
            new ValidationRule
            {
                ruleId = "val_facility_has_anchor",
                description = "Facilities must specify required anchor size",
                category = "gameplay",
                severity = "error"
            },
        };
    }

    [System.Serializable]
    public class ValidationRule
    {
        public string ruleId;
        public string description;
        public string category;
        public string severity; // "error", "warning", "info"
    }
}
