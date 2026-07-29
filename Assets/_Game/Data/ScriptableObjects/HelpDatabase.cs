using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Help and tutorial content for the game.
    /// Contains contextual help messages and tips.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Help Database")]
    public sealed class HelpDatabase : ScriptableObject
    {
        [Header("Controls Help")]
        public string helpMovement = "WASD 移动，Shift 奔跑，Space 跳跃";
        public string helpCamera = "鼠标控制视角，Esc 释放鼠标";
        public string helpInteract = "E 键与物体互动";
        public string helpTools = "1 和 2 切换工具，鼠标左键使用";

        [Header("Gameplay Help")]
        public string helpObserve = "走近发光物体，按 E 观察并记录";
        public string helpRestore = "按住 E 修复损坏的设施";
        public string helpCooperate = "多人站在特定位置同时按 E";
        public string helpExpedition = "在远征信标处按 E 出发远征";

        [Header("UI Help")]
        public string helpCodex = "图鉴记录了你所有的发现";
        public string helpInventory = "按 I 打开背包查看物品";
        public string helpMap = "按 M 打开多人面板";
        public string helpPhoto = "按 P 进入拍照模式";

        [Header("Tips")]
        public string[] tips = new string[]
        {
            "球面世界很大，不要害怕探索未知区域。",
            "居民有自己的生活，观察他们的行为会发现有趣的故事。",
            "天气会变化，暴风来临时记得找避风处。",
            "多人合作能让某些任务更容易完成。",
            "远征带回的发现会改变家园的环境。",
            "居民的愿望连接着家园和远征，试着帮他们实现。",
            "每种工具都有独特的用途，合理搭配使用。",
            "夜晚的星空很美，别忘了抬头看看。",
        };
    }
}
