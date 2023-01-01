using System;
using HarmonyLib;
using UnityEngine;

namespace UltraTelephone.Patches
{
    [HarmonyPatch(typeof(LaughingSkull), "Start")]
    public class ClearwaterPatch
    {
        [HarmonyPostfix]
        public static void doOneThing(ref LaughingSkull __instance, ref AudioSource ___aud)
        {
            ___aud = AudioSwapper.SwapClipWithFile(___aud);
            ___aud.Play();
            
        }
    }
    
    [HarmonyPatch(typeof(LaughingSkull), "PlayAudio")]
    public class ClearwaterPatchAudio
    {
        [HarmonyPrefix]
        public static bool doOtherThing(ref AudioSource ___aud)
        {
            if(!___aud.isPlaying)
            {
                ___aud.Play();
            }
            return false;

        }
    }
}