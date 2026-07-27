using UnityEngine;

namespace Asteria.Player
{
    /// <summary>
    /// Adapts the old Input Manager (Input.GetAxisRaw / Input.GetKey / Input.GetButton)
    /// to the IPlayerInputSource interface. This is the Phase 1 default adapter.
    /// </summary>
    public sealed class LegacyInputAdapter : MonoBehaviour, IPlayerInputSource
    {
        public float Horizontal => Input.GetAxisRaw("Horizontal");
        public float Vertical => Input.GetAxisRaw("Vertical");
        public bool JumpPressed => Input.GetButtonDown("Jump");
        public bool RunHeld => Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        public float MouseX => Input.GetAxisRaw("Mouse X");
        public float MouseY => Input.GetAxisRaw("Mouse Y");
        public bool EscapePressed => Input.GetKeyDown(KeyCode.Escape);
        public bool LeftMouseDown => Input.GetMouseButtonDown(0);
    }
}
