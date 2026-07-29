using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Animation configuration database for the game.
    /// Contains all animation references.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Animation Database")]
    public sealed class AnimationDatabase : ScriptableObject
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
