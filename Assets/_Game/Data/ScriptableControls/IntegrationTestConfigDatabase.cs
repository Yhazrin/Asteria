using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Integration test configuration database for the game.
    /// Contains all integration test parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Integration Test Config Database")]
    public sealed class IntegrationTestConfigDatabase : ScriptableObject
    {
        [Header("Integration Test")]
        public bool enableIntegrationTest = false;
        public float testTimeout = 300f;

        [Header("Scenes")]
        public string[] testScenes = { "SphereMoveDemo", "HomePlanet" };

        [Header("Flow")]
        public bool testHomeToExpedition = true;
        public bool testExpeditionToHome = true;
        public bool testSaveLoad = true;
        public bool testResidentInteraction = true;
    }
}
