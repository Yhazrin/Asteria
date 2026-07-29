using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Sprite configuration database for the game.
    /// Contains all sprite references.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Sprite Database")]
    public sealed class SpriteDatabase : ScriptableObject
    {
        [Header("UI Sprites")]
        public Sprite buttonSprite;
        public Sprite panelSprite;
        public Sprite iconSprite;

        [Header("Character Sprites")]
        public Sprite[] residentSprites;
        public Sprite[] creatureSprites;

        [Header("Item Sprites")]
        public Sprite[] toolSprites;
        public Sprite[] facilitySprites;

        [Header="Status Sprites")]
        public Sprite[] moodSprites;
        public Sprite[] weatherSprites;
        public Sprite[] biomeSprites;
    }
}
