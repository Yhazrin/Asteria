using System.Collections.Generic;
using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Database of UI sprites and references for the game.
    /// Contains all UI assets needed by the interface system.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/UI Database")]
    public sealed class UIDatabase : ScriptableObject
    {
        [Header("Icons")]
        public Sprite iconObserve;
        public Sprite iconRestore;
        public Sprite iconCooperate;
        public Sprite iconTool;
        public Sprite iconInventory;
        public Sprite iconMap;
        public Sprite iconSettings;
        public Sprite iconPhoto;

        [Header("Tool Icons")]
        public Sprite iconResonanceMirror;
        public Sprite iconWarmLight;
        public Sprite iconBeacon;
        public Sprite iconRepairBeam;
        public Sprite iconTetherRope;
        public Sprite iconEcoJar;

        [Header("Status Icons")]
        public Sprite iconCold;
        public Sprite iconLost;
        public Sprite iconSpore;
        public Sprite iconUnstable;
        public Sprite iconRescue;

        [Header("Biome Icons")]
        public Sprite iconWind;
        public Sprite iconMist;
        public Sprite iconNight;
        public Sprite iconIce;
        public Sprite iconBloom;
        public Sprite iconRuin;

        [Header("Mood Icons")]
        public Sprite iconHappy;
        public Sprite iconSad;
        public Sprite iconAngry;
        public Sprite iconCurious;
        public Sprite iconSurprised;
    }
}
