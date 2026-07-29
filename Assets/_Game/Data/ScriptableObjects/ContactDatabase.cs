using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Contact and support database for the game.
    /// Contains all support channels and contact information.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Contact Database")]
    public sealed class ContactDatabase : ScriptableObject
    {
        [Header("Support")]
        public string supportEmail = "support@asteria.game";
        public string discordUrl = "https://discord.gg/asteria";
        public string githubUrl = "https://github.com/Yhazrin/Asteria";

        [Header("Social")]
        public string twitterUrl = "https://twitter.com/asteria_game";
        public string websiteUrl = "https://asteria.game";

        [Header("Legal")]
        public string eulaUrl = "https://asteria.game/eula";
        public string privacyUrl = "https://asteria.game/privacy";
        public string termsUrl = "https://asteria.game/terms";

        [Header("Credits")]
        public string developerName = "Yhazrin";
        public string gameName = "Asteria";
        public string copyright = "© 2026 Yhazrin. All rights reserved.";
    }
}
