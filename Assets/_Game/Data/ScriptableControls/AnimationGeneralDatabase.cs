using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// General animation database for the game.
    /// Contains all animation references.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Animation General Database")]
    public sealed class AnimationGeneralDatabase : ScriptableObject
    {
        [Header("Character Animations")]
        public RuntimeAnimatorController residentAnimator;
        public RuntimeAnimatorController creatureAnimator;
        public RuntimeAnimatorController playerAnimator;

        [Header("Environment Animations")]
        public RuntimeAnimatorController treeAnimator;
        public RuntimeAnimatorController flowerAnimator;
        public RuntimeAnimatorController waterAnimator;

        [Header("UI Animations")]
        public RuntimeAnimatorController uiAnimator;
        public RuntimeAnimatorController popupAnimator;
    }
}
