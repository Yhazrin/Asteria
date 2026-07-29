using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// FAQ database for the game.
    /// Contains all frequently asked questions and answers.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/FAQ Database")]
    public sealed class FAQDatabase : ScriptableObject
    {
        [Header("FAQ")]
        public FAQEntry[] entries = new FAQEntry[]
        {
            new FAQEntry
            {
                question = "Asteria 是什么类型的游戏？",
                answer = "Asteria 是一款 2-4 人球面探索与生活模拟游戏。玩家在真实球面小世界上建立社区，并通过短局远征结识新居民。"
            },
            new FAQEntry
            {
                question = "可以单人玩吗？",
                answer = "可以。所有系统都支持单人模式，AI 星友可以代替部分合作需求。"
            },
            new FAQEntry
            {
                question = "支持多少人联机？",
                answer = "首版支持 2-4 人好友联机，采用房主权威模式。"
            },
            new FAQEntry
            {
                question = "有生存压力吗？",
                answer = "有事件型轻生存，但不是持续饥饿/口渴。压力只在特定天气和远征阶段出现。"
            },
            new FAQEntry
            {
                question = "可以建造房屋吗？",
                answer = "可以，但采用固定锚点式建设，不做自由堆砌。设施必须改变居民行为。"
            },
            new FAQEntry
            {
                question = "星球可以绕一圈吗？",
                answer = "可以。玩家真正生活在球面，可以绕行极点、走到背面。"
            },
            new FAQEntry
            {
                question = "有角色创建吗？",
                answer = "首版使用预设居民，不做随机抽卡。新居民通过远征救援等方式加入。"
            },
            new FAQEntry
            {
                question = "支持离线玩吗？",
                answer = "支持。所有系统都可以离线运行，联机是可选的。"
            },
        };
    }

    [System.Serializable]
    public class FAQEntry
    {
        public string question;
        [TextArea(2, 4)] public string answer;
    }
}
