using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Migration rules database.
    /// Contains all save migration rules for version upgrades.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Migration Database")]
    public sealed class MigrationDatabase : ScriptableObject
    {
        [Header("Migrations")]
        public MigrationRule[] rules = new MigrationRule[]
        {
            new MigrationRule
            {
                fromVersion = 1,
                toVersion = 2,
                description = "Add resident states to save",
                migrationSteps = new[]
                {
                    "Create empty residents array",
                    "Set default personality values",
                    "Initialize empty memories list"
                }
            },
            new MigrationRule
            {
                fromVersion = 2,
                toVersion = 3,
                description = "Add expedition history to save",
                migrationSteps = new[]
                {
                    "Create empty expedition history array",
                    "Initialize empty wishes array"
                }
            },
        };
    }

    [System.Serializable]
    public class MigrationRule
    {
        public int fromVersion;
        public int toVersion;
        public string description;
        public string[] migrationSteps;
    }
}
