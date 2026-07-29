using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Database of audio clips for the game.
    /// Contains all audio references needed by AUDIO_DESIGN.md.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Audio Database")]
    public sealed class AudioDatabase : ScriptableObject
    {
        [Header("Music")]
        public AudioClip musHomeDaily;
        public AudioClip musExpeditionStart;
        public AudioClip musExpeditionExplore;
        public AudioClip musExpeditionPressure;
        public AudioClip musExpeditionRestore;
        public AudioClip musReturnHome;

        [Header("Ambient")]
        public AudioClip ambWindGrassland;
        public AudioClip ambMistForest;
        public AudioClip ambNightValley;
        public AudioClip ambHomePlaza;

        [Header("SFX - Interaction")]
        public AudioClip sfxObserveComplete;
        public AudioClip sfxRestoreComplete;
        public AudioClip sfxCooperateComplete;
        public AudioClip sfxUIClick;
        public AudioClip sfxDiscovery;

        [Header("SFX - Character")]
        public AudioClip sfxFootstepGrass;
        public AudioClip sfxFootstepStone;
        public AudioClip sfxJumpTakeoff;
        public AudioClip sfxJumpLand;

        [Header("SFX - Creature")]
        public AudioClip sfxCreatureCurious;
        public AudioClip sfxCreatureShy;
        public AudioClip sfxCreatureGroup;

        [Header("SFX - Environment")]
        public AudioClip sfxWindLight;
        public AudioClip sfxWindStrong;
        public AudioClip sfxRain;
        public AudioClip sfxThunder;
        public AudioClip sfxGrassRustle;
        public AudioClip sfxWaterStream;
    }
}
