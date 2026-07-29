using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Tutorial step definitions for the game.
    /// Guides new players through core mechanics.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Tutorial Database")]
    public sealed class TutorialDatabase : ScriptableObject
    {
        [Header("Tutorial Steps")]
        public TutorialStep[] steps = new TutorialStep[]
        {
            new TutorialStep
            {
                stepId = "tut_welcome",
                message = "欢迎来到 Asteria！这是一颗属于你的小星球。",
                trigger = "auto",
                duration = 4f,
                showControls = false
            },
            new TutorialStep
            {
                stepId = "tut_movement",
                message = "使用 WASD 移动，鼠标控制视角。",
                trigger = "move",
                duration = 5f,
                showControls = true
            },
            new TutorialStep
            {
                stepId = "tut_observe",
                message = "看到发光的石头了吗？走过去按 E 观察它。",
                trigger = "observe",
                duration = 6f,
                showControls = true
            },
            new TutorialStep
            {
                stepId = "tut_discovery",
                message = "太棒了！你的发现已记录在图鉴中。",
                trigger = "auto",
                duration = 3f,
                showControls = false
            },
            new TutorialStep
            {
                stepId = "tut_expedition",
                message = "走向金色信标，按 E 出发前往远征星球。",
                trigger = "expedition",
                duration = 6f,
                showControls = true
            },
            new TutorialStep
            {
                stepId = "tut_tools",
                message = "你有共鸣镜和暖光灯两个工具。按 1 和 2 切换。",
                trigger = "auto",
                duration = 5f,
                showControls = true
            },
            new TutorialStep
            {
                stepId = "tut_restore",
                message = "看到损坏的设施了吗？按住 E 修复它。",
                trigger = "restore",
                duration = 6f,
                showControls = true
            },
            new TutorialStep
            {
                stepId = "tut_return",
                message = "探索完毕后，走向蓝色信标返回家园。",
                trigger = "auto",
                duration = 5f,
                showControls = true
            },
        };
    }

    [System.Serializable]
    public class TutorialStep
    {
        public string stepId;
        [TextArea(1, 3)] public string message;
        public string trigger; // "auto", "move", "observe", "expedition", "restore"
        public float duration;
        public bool showControls;
    }
}
