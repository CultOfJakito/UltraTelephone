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
            if (HydraLoader.prefabRegistry.TryGetValue("CoinFart", out GameObject coinFart))
            {
                float rand = UnityEngine.Random.Range(0.0f, 100.0f);
                Vector3 pos = __instance.transform.position;
                if (rand > 30.0f)
                {
                    return true;
                }
                else if (rand > 20.0f)
                {
                    Jumpscare.Scare(true);
                    GameObject.Instantiate(coinFart, pos, Quaternion.identity);
                }
                else if (rand > 0.0f)
                {
                    GameObject.Instantiate(coinFart, pos, Quaternion.identity);
                }

                GameObject.Destroy(__instance.gameObject);
                return false;
            }
            
            return true;
        }
    }

    [HarmonyPatch(typeof(TimeController),"ParryFlash")]
    public static class ParryFunnyTimePatch
    {
        public static bool Prefix(ref GameObject ___parryLight)
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
                UltraTelephone.AudioSwapper.SwapAudioClipSource(src, "parry");
            }

            /*
             * old
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
            */
            return true;
        }
    }

    [HarmonyPatch(typeof(NewMovement), "Launch")]
    public static class LaunchTweak
    {

        public static bool Prefix(NewMovement __instance, ref Vector3 direction)
        {
            float health = (float)__instance.hp;
            direction *= UnityEngine.Random.Range(0.2f, (health / 20));
            return true;
        }
    }

    public static float BombMultiplier = 1.0f;

    public static int CurrentClusterPool = 0;

    public static int MaxCluster { get; private set; } = 1600;

    [HarmonyPatch(typeof(Explosion), "Start")]
    public static class BombTweak
    {
        public static bool Prefix(Explosion __instance)
        {
            __instance.maxSize *= Mathf.Max(1.0f, BombMultiplier);
            __instance.maxSize = Mathf.Clamp(__instance.maxSize, 0.0f, 500.0f);

            if (ClusterExplosives.ClusterExplosivesEnabled)
            {
                int rand = UnityEngine.Random.Range(0, 3);
                if(TryCluster())
                {
                    ++CurrentClusterPool;

                    Vector3 randomOffset = UnityEngine.Random.insideUnitSphere;
                    randomOffset *= __instance.maxSize*1.45f;

                    Vector3 dupePosition = __instance.transform.position + randomOffset;

                    GameObject dupedExplosion = GameObject.Instantiate<GameObject>(__instance.gameObject, dupePosition, __instance.transform.rotation);
                    dupedExplosion.transform.parent = __instance.transform.parent;
                }
            }
            return true;
        }

        private static bool TryCluster()
        {
            float rand = UnityEngine.Random.value;
            if(rand > Mathf.InverseLerp(0,MaxCluster,CurrentClusterPool))
            {
                return true;
            }

            return false;
        }
    }

    [HarmonyPatch(typeof(Revolver), "Update")]
    public static class CoinAmmoPatch_Update
    {
        public static bool Prefix(Revolver __instance, ref float ___coinCharge)
        {
            if (__instance.gunVariation == 1)
            {
                if (CoinCollectorManager.CollectedCoins > 0)
                {
                    ___coinCharge = 100.0f;
                }
                else
                {
                    ___coinCharge = 0.0f;
                }
            }

            return true;
        }
    }

    [HarmonyPatch(typeof(Revolver), "ThrowCoin")]
    public static class CoinAmmoPatch_ThrowCoin
    {
        public static bool Prefix(Revolver __instance, ref float ___coinCharge)
        {
            if (__instance.gunVariation == 1)
            {
                if(CoinCollectorManager.Instance.SpendCoins(1))
                {
                    return true;
                }
                return false;
            }
            return true;
        }
    }

    //For Zed <3
    //This prevents weapon switching with scroll wheel.
    [HarmonyPatch(typeof(GunControl), "Update")]
    public static class ScrollStopPatch
    {
        public static bool Prefix(ref float ___scrollCooldown)
        {
            ___scrollCooldown = 5.0f;
            return true;
        }
    }

}
