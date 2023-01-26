using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using UnityEngine;

public class HYD_EnemiesPatch
{
    [HarmonyPatch(typeof(MinosPrime), "RiderKick")]
    public static class MinosPrimeBuffPatch
    {
        public static bool Prefix(MinosPrime __instance, Machine ___mach, float ___originalHp)
        {
            RandomSounds.PlayRandomSound();
            float healthRatio = (___mach.health / ___originalHp);

            float rand = UnityEngine.Random.Range(0.0f,100.0f);
            if(rand < 80-(80*healthRatio))
            {
                GameObject newTwin = GameObject.Instantiate<GameObject>(__instance.gameObject, __instance.transform.position, __instance.transform.rotation);
                newTwin.transform.position -= newTwin.transform.forward * 0.2f;
                newTwin.transform.parent = __instance.transform.parent;
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(MinosPrime), "Start")]
    public static class MinosPrimeBuffPatchTwo
    {
        public static void Postfix(ref float ___originalHp)
        {
            ___originalHp *= 2.0f;
        }
    }

    
}