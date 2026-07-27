using UnityEngine;

namespace Asteria.Player
{
    /// <summary>
    /// Minimal on-screen controls for the Phase 1 demo.
    /// </summary>
    public sealed class SphereMoveDemoHud : MonoBehaviour
    {
        void OnGUI()
        {
            const float pad = 16f;
            GUILayout.BeginArea(new Rect(pad, pad, 420f, 160f), GUI.skin.box);
            GUILayout.Label("Asteria — Sphere Move Demo (Phase 1)");
            GUILayout.Label("WASD 移动 · Shift 奔跑 · Space 跳跃");
            GUILayout.Label("鼠标 视角 · Esc 释放鼠标 · 左键重新锁定");
            GUILayout.Label("目标：绕球一圈，经过极点与背面标记");
            GUILayout.EndArea();
        }
    }
}
