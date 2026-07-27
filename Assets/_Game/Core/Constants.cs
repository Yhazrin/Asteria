namespace Asteria
{
    /// <summary>
    /// Shared constants to avoid hardcoded string literals across the codebase.
    /// </summary>
    public static class AsteriaConstants
    {
        // Scene names
        public const string BootstrapSceneName = "Bootstrap";
        public const string HomeSceneName = "HomePlanet";
        public const string ExpeditionSceneName = "SphereMoveDemo";

        // Shader names
        public const string URPShaderName = "Universal Render Pipeline/Lit";
        public const string FallbackShaderName = "Sprites/Default";

        // Log prefixes
        public const string LogPrefix = "[Asteria]";

        // Default planet parameters
        public const float DefaultHomePlanetRadius = 180f;
        public const float DefaultExpeditionPlanetRadius = 300f;
        public const float DefaultGravityStrength = 9.81f;

        // Resident interaction
        public const float ResidentInteractionDistance = 4f;
        public const float ResidentInteractionCooldown = 15f;
        public const float ResidentInteractionCheckInterval = 3f;
        public const float ResidentScheduleDurationMin = 30f;
        public const float ResidentScheduleDurationMax = 90f;

        // Pressure system
        public const float PressureTriggerDelay = 180f;  // 3 minutes
        public const float PressureFallThreshold = 2f;

        // Save system
        public const int CurrentSchemaVersion = 1;
        public const string SaveFileName = "save.json";
        public const int MaxBackupCount = 3;
    }
}
