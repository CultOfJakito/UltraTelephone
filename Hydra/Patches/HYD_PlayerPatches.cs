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
                    Vector3 pos = __instance.transform.position;
                    GameObject.Destroy(__instance.gameObject);
                    if (rand > 50.0f)
                    {
                        GameObject.Instantiate(coinFart, pos, Quaternion.identity);
                    }
                    else if(rand > 25.0f)
                    {
                        Jumpscare.Scare(true);
                    }
                    else if(rand > 5.0f)
                    {

                    }

                    return false;
                }
            }

            return true;
        }
    }

    [HarmonyPatch(typeof(TimeController),"Start")]
    public static class ParryFunnyTimePatch
    {
        public static void Postfix(ref GameObject ___parryLight)
        {
            AudioClip newClip = null;
            if (HydraLoader.dataRegistry.TryGetValue("FunnyParryNoise", out UnityEngine.Object fpnObj))
            {
                newClip = (AudioClip)fpnObj;
                if (newClip != null)
                {
                    if (!___parryLight.TryGetComponent<AudioSource>(out AudioSource src))
                    {
                        AudioSource childSrc = ___parryLight.GetComponentInChildren<AudioSource>(true);
                        if (childSrc != null)
                        {
                            src = childSrc;
                        }

                    }

                    if (src != null)
                    {
                        src.clip = newClip;
                    }
                }            
            }
        }
    }      
}
