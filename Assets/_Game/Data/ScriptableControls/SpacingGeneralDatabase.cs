using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// General spacing database for the game.
    /// Contains all spacing parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Spacing General Database")]
    public sealed class SpacingGeneralDatabase : ScriptableObject
    {
        [Header("Base Spacing")]
        public float spaceXXS = 2f;
        public float spaceXS = 4f;
        public float spaceS = 8f;
        public float spaceM = 16f;
        public float spaceL = 24f;
        public float spaceXL = 32f;
        public float spaceXXL = 48f;

        [Header("Component Spacing")]
        public float buttonPadding = 8f;
        public float cardPadding = 16f;
        public float panelPadding = 12f;
        public float sectionSpacing = 24f;
    }
}
