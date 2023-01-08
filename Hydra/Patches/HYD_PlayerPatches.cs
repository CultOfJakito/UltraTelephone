using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using UnityEngine.UI;
using UnityEngine;

public class HYD_PlayerPatches
{
    [HarmonyPatch(typeof(Punch), "CheckForProjectile")]
    public static class ParryPatch
    {
        public static bool Prefix(Punch __instance, Transform target, bool __result, ref bool ___hitSomething)
        {
            if (target.TryGetComponent<MadMass>(out MadMass madMass))
            {
                if(madMass.Alive)
                {
                    __instance.anim.Play("Hook", 0, 0.065f);
                    MonoSingleton<TimeController>.Instance.ParryFlash();
                    madMass.Die();
                    ___hitSomething = true;
                    __result = true;
                    return false;
                }
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(FinalPit), "SendInfo")]
    public static class FinalPitEventPatch
    {
        public static void Postfix()
        {
            BestUtilityEverCreated.OnLevelComplete?.Invoke();
        }
    }

    [HarmonyPatch(typeof(PlayerActivatorRelay), "Activate")]
    public static class PlayerActivatorEventPatch
    {
        public static void Postfix()
        {
            BestUtilityEverCreated.OnPlayerActivated?.Invoke();
        }
    }

    [HarmonyPatch(typeof(Coin), "ReflectRevolver")]
    public static class CoinRealismPatch
    {
        public static bool Prefix(Coin __instance)
        {
            float rand = UnityEngine.Random.Range(0.0f,100.0f);

            if(rand > 5.0f)
            {
                if (HydraLoader.prefabRegistry.TryGetValue("CoinFart", out GameObject coinFart))
                {
                    GameObject.Instantiate(coinFart, __instance.transform.position, Quaternion.identity);
                    GameObject.Destroy(__instance.gameObject);
                    return false;
                }
            }

            return true;
        }
    }
}
