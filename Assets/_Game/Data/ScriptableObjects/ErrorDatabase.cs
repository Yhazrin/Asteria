using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Error message definitions for the game.
    /// Contains user-friendly error messages and recovery instructions.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Error Database")]
    public sealed class ErrorDatabase : ScriptableObject
    {
        [Header("Save Errors")]
        public string errSaveFailed = "保存失败。已保留备份存档。";
        public string errLoadFailed = "加载失败。正在尝试备份存档...";
        public string errSaveCorrupted = "存档已损坏。已恢复到最近的备份。";
        public string errSchemaMismatch = "存档版本不匹配。正在尝试迁移...";

        [Header("Network Errors")]
        public string errConnectionFailed = "连接失败。请检查网络连接。";
        public string errDisconnected = "已断开连接。正在尝试重连...";
        public string errHostDisconnected = "房主已断开。会话结束。";
        public string errReconnectFailed = "重连失败。请重新加入。";

        [Header("Gameplay Errors")]
        public string errToolNoEnergy = "工具能量不足。";
        public string errAnchorOccupied = "该位置已有设施。";
        public string errInvalidPlacement = "无法在此处放置。";
        public string errCooldownActive = "冷却中，请稍后再试。";

        [Header("System Errors")]
        public string errContentMissing = "缺少内容文件。请验证游戏完整性。";
        public string errShaderMissing = "着色器缺失。请重新安装 URP 包。";
        public string errMemoryLow = "内存不足。建议降低画质设置。";
    }
}
