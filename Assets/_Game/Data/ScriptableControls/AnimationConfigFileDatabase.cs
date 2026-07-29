using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Animation configuration file database for the game.
    /// Contains all animation parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Animation Config File Database")]
    public sealed class AnimationConfigFileDatabase : ScriptableObject
    {
        [Header("Character")]
        public float walkAnimationSpeed = 1f;
        public float runAnimationSpeed = 1.5f;
        public float idleAnimationSpeed = 1f;

        [Header("Resident")]
        public float residentMoveSpeed = 4f;
        public float residentRotationSpeed = 8f;

        [Header("Creature")]
        public float creatureMoveSpeed = 3f;
        public float creatureRotationSpeed = 5f;

        [Header("Environment")]
        public float grassWaveSpeed = 1f;
        public float treeWaveSpeed = 0.5f;
        public float waterWaveSpeed = 1f;
    }
}
