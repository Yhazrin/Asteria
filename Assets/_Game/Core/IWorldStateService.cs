namespace Asteria
{
    /// <summary>
    /// Provides read-only access to the current world state.
    /// Used by UI and event systems to query conditions.
    /// </summary>
    public interface IWorldStateService
    {
        /// <summary>Current weather condition ID.</summary>
        string CurrentWeather { get; }

        /// <summary>Current biome ID the player is in.</summary>
        string CurrentBiome { get; }

        /// <summary>Number of active players in the session.</summary>
        int ActivePlayerCount { get; }

        /// <summary>True if a pressure event is currently active.</summary>
        bool IsPressureActive { get; }

        /// <summary>Set the current weather (called by event director).</summary>
        void SetWeather(string weatherId);

        /// <summary>Set the current biome (called by world generation).</summary>
        void SetBiome(string biomeId);
    }
}
