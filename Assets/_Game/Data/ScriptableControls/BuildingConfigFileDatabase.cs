using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Building configuration file database for the game.
    /// Contains all building parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Building Config File Database")]
    public sealed class BuildingConfigFileDatabase : ScriptableObject
    {
        [Header("Anchors")]
        public int maxLargeAnchors = 6;
        public int maxMediumAnchors = 12;
        public int maxSmallAnchors = 20;

        [Header("Placement")]
        public float previewRotationSpeed = 90f;
        public float gridSize = 1f;
        public float buildCooldown = 1f;

        [Header("Colors")]
        public Color validPlacement = new(0.3f, 0.8f, 0.3f, 0.5f);
        public Color invalidPlacement = new(0.8f, 0.3f, 0.3f, 0.5f);
    }
}
