using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Physics settings database for the game.
    /// Contains all Unity Physics settings.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Physics Settings Database")]
    public sealed class PhysicsSettingsDatabase : ScriptableObject
    {
        [Header("Gravity")]
        public float gravityX = 0f;
        public float gravityY = -9.81f;
        public float gravityZ = 0f;

        [Header("Simulation")]
        public bool autoSimulation = true;
        public bool autoSyncTransforms = false;
        public bool reuseCollisionCallbacks = true;
        public float defaultContactOffset = 0.01f;

        [Header("Solver")]
        public int solverIterations = 6;
        public int solverVelocityIterations = 1;
        public float defaultMaxAngularSpeed = 50f;
        public float defaultMaxDepenetrationVelocity = 10f;

        [Header("Layers")]
        public int defaultLayerCollisionMatrix = -1;
    }
}
