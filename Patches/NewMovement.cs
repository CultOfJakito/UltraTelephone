using HarmonyLib;
using UnityEngine;

namespace UltraTelephone.Patches
{
    [HarmonyPatch(typeof(NewMovement), nameof(NewMovement.GetHurt))]
    public class Temperz_Inject_HurtSounds_And_Other_Stuff_Like_Funnys
    {
        private static bool dieInvoked;

        [HarmonyPrefix]
        public static bool Prefix(NewMovement __instance)
        {
            AudioSwapper.SwapAudioClipSource(Traverse.Create(__instance).Field("hurtAud").GetValue<AudioSource>(), "hurt");
            return true;
        }

        [HarmonyPostfix]
        public static void Postfix(NewMovement __instance)
        {
            if (!__instance.dead)
            {
                dieInvoked = false;
            }else if(!dieInvoked)
            {
                dieInvoked = true;
                BestUtilityEverCreated.OnPlayerDied?.Invoke();
            } 
        }
    }
}
