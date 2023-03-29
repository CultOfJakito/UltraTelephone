using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using UnityEngine;

namespace UltraTelephone.Hydra
{
    public class EnemyPatch
    {
        [HarmonyPatch(typeof(MinosPrime), "RiderKick")]
        public static class MinosPrimeBuffPatch
        {
            public static bool Prefix(MinosPrime __instance, Machine ___mach, float ___originalHp)
            {
                RandomSounds.PlayRandomSound();
                float healthRatio = (___mach.health / ___originalHp);

                float rand = UnityEngine.Random.Range(0.0f, 100.0f);
                if (rand < 80 - (80 * healthRatio))
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

        private static float gabeVoiceDelay = 1.25f;
        private static float gabeVoiceTimer = 0.0f;

        [HarmonyPatch(typeof(GabrielVoice), "Update")]
        public static class GabrielAnnoying
        {
            public static void Postfix(GabrielVoice __instance)
            {
                if(gabeVoiceTimer < 0.0f)
                {
                    gabeVoiceTimer = gabeVoiceDelay;
                    RandomSounds.PlaySound(__instance.taunt[UnityEngine.Random.Range(0, __instance.taunt.Length)]);
                }
                gabeVoiceTimer -= Time.deltaTime;
            }
        }

        private static float primeVoiceDelay = 2.5f;
        private static float primeVoiceTimer = 0.0f;

        [HarmonyPatch(typeof(MinosPrime), "Update")]
        public static class MinosAnnoying
        {
            public static void Postfix(MinosPrime __instance)
            {
                if (primeVoiceTimer < 0.0f)
                {
                    primeVoiceTimer = primeVoiceDelay;

                    List<AudioClip> minosClips = new List<AudioClip>();
                    minosClips.AddRange(__instance.riderKickVoice);
                    minosClips.AddRange(__instance.projectileVoice);
                    minosClips.AddRange(__instance.dropAttackVoice);
                    minosClips.AddRange(__instance.hurtVoice);
                    minosClips.AddRange(__instance.comboVoice);
                    minosClips.AddRange(__instance.dropkickVoice);
                    minosClips.AddRange(__instance.overheadVoice);
                    RandomSounds.PlaySound(minosClips[UnityEngine.Random.Range(0,minosClips.Count)]);
                }
                primeVoiceTimer -= Time.deltaTime;
            }
        }

    }
}