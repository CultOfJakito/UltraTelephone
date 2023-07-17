using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace UltraTelephone.Hydra.Patches
{
    public static class NightcorePatch
    {
        [HarmonyPatch(typeof(MusicManager))]
        public static class MusicManagerPatches
        {

            [HarmonyPatch("OnEnable")]
            [HarmonyPostfix]
            public static void NightcoreMode(MusicManager __instance)
            {
                float newPitch = 1f;
                switch (HydrasConfig.MusicAlterationMode)
                {
                    default:
                        return;
                    case 1:
                        newPitch = 1.5f;
                        break;
                    case 2:
                        newPitch = 0.65f;
                        break;
                }

                __instance.targetTheme.pitch = newPitch;
                __instance.battleTheme.pitch = newPitch;
                __instance.cleanTheme.pitch = newPitch;
                __instance.bossTheme.pitch = newPitch;
            }
        }
    }
}
