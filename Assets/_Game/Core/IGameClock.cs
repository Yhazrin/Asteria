namespace Asteria
{
    /// <summary>
    /// Abstracts game time. Single-player uses real time; multiplayer uses host-authoritative time.
    /// </summary>
    public interface IGameClock
    {
        /// <summary>Current in-game day number (starts at 1).</summary>
        int WorldDay { get; }

        /// <summary>Time of day as a 0-1 fraction (0 = midnight, 0.5 = noon).</summary>
        float TimeOfDay { get; }

        /// <summary>Real seconds elapsed since session start.</summary>
        float ElapsedSeconds { get; }

        /// <summary>True if the clock is currently advancing.</summary>
        bool IsRunning { get; }

        /// <summary>Advance the clock by delta seconds.</summary>
        void Tick(float deltaTime);

        /// <summary>Set the world day directly (used on load).</summary>
        void SetDay(int day);
    }
}
