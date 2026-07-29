using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Automation configuration database for the game.
    /// Contains all automation parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Automation Config Database")]
    public sealed class AutomationConfigDatabase : ScriptableObject
    {
        [Header("Automation")]
        public bool enableAutomation = false;
        public float automationInterval = 60f;

        [Header("Tasks")]
        public bool autoBuild = true;
        public bool autoTest = true;
        public bool autoDeploy = false;

        [Header("CI/CD")]
        public bool enableCICD = false;
        public string ciEndpoint = "";
    }
}
