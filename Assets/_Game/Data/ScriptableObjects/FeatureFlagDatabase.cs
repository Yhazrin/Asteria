using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Feature flag database for gradual feature rollout.
    /// Contains all feature toggles for the game.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Feature Flag Database")]
    public sealed class FeatureFlagDatabase : ScriptableObject
    {
        [Header("Core Features")]
        public bool enableSaveSystem = true;
        public bool enableHomePlanet = true;
        public bool enableExpedition = true;
        public bool enableResidents = true;

        [Header("Social Features")]
        public bool enableSocialEvents = true;
        public bool enableWishes = true;
        public bool enableDialogue = true;
        public bool enableRelationships = true;

        [Header("Building Features")]
        public bool enableBuilding = true;
        public bool enableFacilities = true;
        public bool enableAnchors = true;

        [Header("Expedition Features")]
        public bool enablePressure = true;
        public bool enableTools = true;
        public bool enableCreatures = true;
        public bool enableWeather = true;

        [Header("Multiplayer Features")]
        public bool enableMultiplayer = false; // Disabled until NGO integration
        public bool enableLobby = false;
        public bool enableVoiceChat = false;

        [Header("UI Features")]
        public bool enableNewUI = true;
        public bool enableCompass = true;
        public bool enableMiniMap = true;
        public bool enablePhotoMode = true;

        [Header("Audio Features")]
        public bool enableMusic = true;
        public bool enableSFX = true;
        public bool enableAmbient = true;
        public bool enableSpatialAudio = true;
    }
}
