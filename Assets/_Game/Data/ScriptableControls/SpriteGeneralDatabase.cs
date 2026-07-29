using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// General sprite database for the game.
    /// Contains all sprite references.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Sprite General Database")]
    public sealed class SpriteGeneralDatabase : ScriptableObject
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

        [Header("Status Sprites")]
        public Sprite[] moodSprites;
        public Sprite[] weatherSprites;
        public Sprite[] biomeSprites;
    }
}
