using UnityEngine;

namespace Asteria.Residents
{
    /// <summary>
    /// Creates the default resident definitions for the Alpha build.
    /// ROADMAP_V2.md Milestone I: 6-12 residents.
    /// </summary>
    public static class DefaultResidents
    {
        /// <summary>
        /// Create the default 6 residents for the home planet.
        /// </summary>
        public static ResidentDefinition[] CreateDefaultResidentDefinitions()
        {
            return new[]
            {
                // 莲 - warm, social (existing)
                CreateResident("lian", "莲", new Color(0.85f, 0.75f, 0.8f),
                    0.6f, 0.4f, 0.7f, 0.3f, 0.2f,
                    new[] { "会给所有植物取名字" }),

                // 凯 - curious, bold (existing)
                CreateResident("kai", "凯", new Color(0.7f, 0.8f, 0.85f),
                    -0.3f, 0.8f, 0.1f, 0.6f, 0.7f,
                    new[] { "害怕下坡却喜欢高处" }),

                // 晴 - cheerful, organized
                CreateResident("qing", "晴", new Color(0.95f, 0.85f, 0.7f),
                    0.5f, 0.2f, 0.6f, 0.8f, 0.1f,
                    new[] { "总想把严肃场合变成合影" }),

                // 霜 - reserved, curious
                CreateResident("shuang", "霜", new Color(0.75f, 0.85f, 0.9f),
                    -0.5f, 0.9f, -0.3f, 0.4f, 0.5f,
                    new[] { "对风铃声异常敏感" }),

                // 岩 - bold, organized
                CreateResident("yan", "岩", new Color(0.8f, 0.75f, 0.65f),
                    0.1f, 0.3f, 0.2f, 0.7f, 0.8f,
                    new[] { "一紧张就开始整理东西" }),

                // 云 - dreamy, warm
                CreateResident("yun", "云", new Color(0.9f, 0.88f, 0.92f),
                    0.3f, 0.6f, 0.8f, -0.2f, -0.1f,
                    new[] { "喜欢在高处发呆" }),
            };
        }

        static ResidentDefinition CreateResident(string id, string name, Color color,
            float soc, float cur, float war, float ord, float bol, string[] quirks)
        {
            var def = ScriptableObject.CreateInstance<ResidentDefinition>();
            def.InitializeRuntime(id, name, color,
                soc: soc, cur: cur, war: war, ord: ord, bol: bol,
                quirkList: quirks);
            return def;
        }
    }
}
