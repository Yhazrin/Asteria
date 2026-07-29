using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Border file database for the game.
    /// Contains all border parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Border File Database")]
    public sealed class BorderFileDatabase : ScriptableObject
    {
        [Header("Border Radius")]
        public float radiusXS = 2f;
        public float radiusS = 4f;
        public float radiusM = 8f;
        public float radiusL = 12f;
        public float radiusXL = 16f;
        public float radiusRound = 9999f;

        [Header("Border Width")]
        public float widthThin = 1f;
        public float widthMedium = 2f;
        public float widthThick = 3f;

        [Header("Border Colors")]
        public Color borderColorLight = new(0.8f, 0.8f, 0.8f, 0.3f);
        public Color borderColorMedium = new(0.5f, 0.5f, 0.5f, 0.5f);
        public Color borderColorDark = new(0.3f, 0.3f, 0.3f, 0.7f);
    }
}
