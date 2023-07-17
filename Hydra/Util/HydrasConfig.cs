using System;
using System.Collections.Generic;
using System.Text;
using BepInEx.Configuration;
using BepInEx;

namespace UltraTelephone.Hydra
{
    public static class HydrasConfig
    {
        public static bool Madness_Enabled { get; private set; } = true;

        public static float Madness_TickDamage { get; private set; } = 2.0f;
        public static float Madness_MaxMadnessMultiplier { get; private set; } = 1.0f;
        public static float Madness_TickDelay { get; private set; } = 2.1f;
        public static float Madness_FullDamage { get; private set; } = 1.0f;

        //Bruh moments
        public static bool BruhMoments_FreeBird { get; private set; } = true;
        public static bool BruhMoments_ChuckNorris { get; private set; } = true;
        public static bool BruhMoments_Christmas { get; private set; } = true;
        public static bool BruhMoments_Jumpscare { get; private set; } = true;
        public static bool BruhMoments_Clowning { get; private set; } = true;
        public static bool BruhMoments_RandomSound { get; private set; } = true;
        public static bool BruhMoments_Weirdening { get; private set; } = true;
        public static bool BruhMoments_Ubisoft { get; private set; } = true;
        public static bool BruhMoments_ClusterBomb { get; private set; } = true;
        public static bool BruhMoments_Multiplayer { get; private set; } = true;
        public static bool BruhMoments_AllGuns { get; private set; } = true;
        public static bool BruhMoments_InconsistentPlayer { get; private set; } = true;

        //Patches
        public static bool Patches_AudioFuckery { get; private set; } = true;
        public static bool Patches_UWU { get; private set; } = true;
        public static bool Patches_BouncyExplosives { get; private set; } = true;

        public static bool Patches_WackyPhysics { get; private set; } = true;
        public static float Patches_WackyPhysicsMultiplier { get; private set; } = 20f;

        //Coins
        public static int CoinCollector_MaxCoins { get; private set; } = 420;

        //Generic
        public static bool Herobrine_Enabled { get; private set; } = true;
        public static float Herobrine_Frequency { get; private set; } = 30f;

        public static bool Plushie_Enabled { get; private set; } = true;
        public static bool WallOfShame_Enabled { get; private set; } = true;

        public static int MusicAlterationMode { get; private set; } = 1;

        public static void LoadConfig()
        {
            Madness_Enabled = Plugin.UltraTelephone.Config.Bind<bool>("Hydra_Madness", "Enabled", true, "Enable Frenzy Mechanic.").Value;

            Madness_TickDamage = Plugin.UltraTelephone.Config.Bind<float>("Hydra_Madness", "TickDamage", 2.0f, "Damage for every tick of Frenzy.").Value;
            Madness_MaxMadnessMultiplier = Plugin.UltraTelephone.Config.Bind<float>("Hydra_Madness", "MaxMadnessMultiplier", 1.0f, "Damage for every tick of Frenzy.").Value;
            Madness_TickDelay = Plugin.UltraTelephone.Config.Bind<float>("Hydra_Madness", "TickDelay", 2.1f, "How long in between ticks of damage.").Value;
            Madness_FullDamage = Plugin.UltraTelephone.Config.Bind<float>("Hydra_Madness", "FullDamage", 1.0f, "Damage received multiplier for getting full frenzy meter.").Value;

            BruhMoments_Christmas = Plugin.UltraTelephone.Config.Bind<bool>("Hydra_BruhMoments", "Christmas", true, "Informs you of christmas chronology.").Value;
            BruhMoments_ChuckNorris = Plugin.UltraTelephone.Config.Bind<bool>("Hydra_BruhMoments", "ChuckNorris", true, "Informs you of ChuckNorris.").Value;
            BruhMoments_Jumpscare = Plugin.UltraTelephone.Config.Bind<bool>("Hydra_BruhMoments", "Jumpscare", true, "Jumpscare you.").Value;
            BruhMoments_Clowning = Plugin.UltraTelephone.Config.Bind<bool>("Hydra_BruhMoments", "Clowning", true, "Skill issue checker.").Value;
            BruhMoments_RandomSound = Plugin.UltraTelephone.Config.Bind<bool>("Hydra_BruhMoments", "RandomSound", true, "Play random sounds").Value;
            BruhMoments_Weirdening = Plugin.UltraTelephone.Config.Bind<bool>("Hydra_BruhMoments", "Weirdening", true, "Makes ultrakill weird.").Value;
            BruhMoments_Ubisoft = Plugin.UltraTelephone.Config.Bind<bool>("Hydra_BruhMoments", "Ubisoft", true, "Ubisoft-Inspired game design tweaks.").Value;
            BruhMoments_Multiplayer = Plugin.UltraTelephone.Config.Bind<bool>("Hydra_BruhMoments", "Multiplayer", true, "Enables multiplayer mode.").Value;
            BruhMoments_ClusterBomb = Plugin.UltraTelephone.Config.Bind<bool>("Hydra_BruhMoments", "ClusterBomb", true, "Turns bomb into bombs").Value;
            BruhMoments_FreeBird = Plugin.UltraTelephone.Config.Bind<bool>("Hydra_BruhMoments", "FreeBird", true, "Frees Birds").Value;
            BruhMoments_AllGuns = Plugin.UltraTelephone.Config.Bind<bool>("Hydra_BruhMoments", "AllGuns", true, "All guns at once").Value;
            BruhMoments_InconsistentPlayer = Plugin.UltraTelephone.Config.Bind<bool>("Hydra_BruhMoments", "InconsistentPlayer", true, "Inconsistent annoyance").Value;

            Patches_AudioFuckery = Plugin.UltraTelephone.Config.Bind<bool>("Hydra_Patches", "AudioFuckery", true, "Randomly swaps audio from different things").Value;
            Patches_UWU = Plugin.UltraTelephone.Config.Bind<bool>("Hydra_Patches", "UWUify", true, "Makes everything cute").Value;
            Patches_BouncyExplosives = Plugin.UltraTelephone.Config.Bind<bool>("Hydra_Patches", "BouncyExplosives", true, "Makes explosives bounce sometimes").Value;
            Patches_WackyPhysics = Plugin.UltraTelephone.Config.Bind<bool>("Hydra_Patches", "WackyPhysics", true, "Makes physics extreme").Value;
            Patches_WackyPhysicsMultiplier = Plugin.UltraTelephone.Config.Bind<float>("Hydra_Patches", "WackyPhysicsMultiplier", 20f, "Force multiplier").Value;

            Herobrine_Enabled = Plugin.UltraTelephone.Config.Bind<bool>("Hydra_Patches", "Herobrine", true, "Enables herobrine").Value;
            Herobrine_Frequency = Plugin.UltraTelephone.Config.Bind<float>("Hydra_Patches", "HerobrineFrequency", 40f, "Frequency of herobrine lower is more frequent").Value;

            Plushie_Enabled = Plugin.UltraTelephone.Config.Bind<bool>("Hydra_Patches", "HydraPlushies", true, "Enables Hydra plushies").Value;
            WallOfShame_Enabled = Plugin.UltraTelephone.Config.Bind<bool>("Hydra_Patches", "WallOfDecor", true, "Enables the wall of decorations").Value;

            MusicAlterationMode = Plugin.UltraTelephone.Config.Bind<int>("Hydra_Patches", "Nightcore", 1, "(0 - Off)(1 - Nightcore)(2 - Vaporwave)").Value;

            CoinCollector_MaxCoins = Plugin.UltraTelephone.Config.Bind<int>("Hydra_CoinCollector", "MaxCoins", 420, "How many coin spawned").Value;

        }

    }
}
