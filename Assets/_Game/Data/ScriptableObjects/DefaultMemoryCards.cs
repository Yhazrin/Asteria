using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Creates default shared memory cards for the Alpha build.
    /// Implements the memory card system from CORE_GAMEPLAY_AND_SYSTEMS.md §8.
    /// </summary>
    public static class DefaultMemoryCards
    {
        public static SharedMemoryCard[] CreateDefaultCards()
        {
            return new[]
            {
                CreateCard("wind_bell_discovery", "风铃石的颤音",
                    "我们在风之草原发现了一块会发声的石头。靠近时能听见很轻的金属颤音。",
                    "风之草原", "发现风铃石"),

                CreateCard("storm_rescue", "风暴中的救援",
                    "全球强风来袭时，有人被困在峡谷里。我们用信标和牵引绳把他救了出来。",
                    "风之草原", "成功救援"),

                CreateCard("bipolar_aurora", "双极极光",
                    "我们在星球两侧同时激活了共鸣装置，天空出现了壮丽的极光。",
                    "风之草原", "双极共鸣完成"),

                CreateCard("seed_choice", "种子的去向",
                    "我们选择把风铃石的种子带回家，种在了温室里。",
                    "风之草原", "带回种子"),
            };
        }

        static SharedMemoryCard CreateCard(string id, string title, string desc,
            string planet, string discovery)
        {
            var card = ScriptableObject.CreateInstance<SharedMemoryCard>();
            card.cardId = id;
            card.title = title;
            card.description = desc;
            card.planetName = planet;
            card.keyDiscovery = discovery;
            return card;
        }
    }
}
