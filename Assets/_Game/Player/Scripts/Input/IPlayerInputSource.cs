namespace Asteria.Player
{
    /// <summary>
    /// Abstracts player input so SphericalMotor and SphericalThirdPersonCamera
    /// do not depend on a specific input backend (old Input Manager, new Input System, etc.).
    /// </summary>
    public interface IPlayerInputSource
    {
        /// <summary>Horizontal axis (-1..1), camera-relative.</summary>
        float Horizontal { get; }

        /// <summary>Vertical axis (-1..1), camera-relative.</summary>
        float Vertical { get; }

        /// <summary>True on the frame the player presses jump.</summary>
        bool JumpPressed { get; }

        /// <summary>True while the player holds run/sprint.</summary>
        bool RunHeld { get; }

        /// <summary>Mouse X delta since last frame (pixels).</summary>
        float MouseX { get; }

        /// <summary>Mouse Y delta since last frame (pixels).</summary>
        float MouseY { get; }

        /// <summary>True on the frame Escape is pressed.</summary>
        bool EscapePressed { get; }

        /// <summary>True on the frame left mouse button is pressed.</summary>
        bool LeftMouseDown { get; }
    }
}
