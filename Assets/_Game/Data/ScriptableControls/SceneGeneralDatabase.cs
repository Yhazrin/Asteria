using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// General scene database for the game.
    /// Contains all scene references and settings.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Scene General Database")]
    public sealed class SceneGeneralDatabase : ScriptableObject
    {
        [Header("Core Scenes")]
        public string bootstrapScene = "Bootstrap";
        public string homePlanetScene = "HomePlanet";
        public string expeditionScene = "SphereMoveDemo";

        [Header("Scene Settings")]
        public string[] buildScenes = {
            "Assets/_Game/Core/Scenes/Bootstrap.unity",
            "Assets/_Game/Core/Scenes/HomePlanet.unity",
            "Assets/_Game/Planet/Scenes/SphereMoveDemo.unity"
        };

        [Header("Loading")]
        public float sceneTransitionTime = 2f;
        public bool showLoadingScreen = true;
    }
}
