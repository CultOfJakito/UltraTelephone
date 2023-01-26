using System;
using HarmonyLib;
using UnityEngine;

namespace UltraTelephone.Patches
{
    // REDDSIT TEMPERZ HEAREAR TO TEXPALIN, TBASICALLY THIS ISN'T FUNNY ESO FUCK YOU I SET THE CLIP ELSE WHERE
    //[HarmonyPatch(typeof(LaughingSkull), "Start")]
    //public class ClearwaterPatch
    //{
    //    [HarmonyPostfix]
    //    public static void doOneThing(ref LaughingSkull __instance, ref AudioSource ___aud)
    //    {
    //        Debug.Log("Postfixing!");
    //        AudioSwapper.SwapClipWithFile(___aud);
    //        ___aud.Play();
    //    }
    //}

    [HarmonyPatch(typeof(LaughingSkull), "PlayAudio")]
    public class ClearwaterPatchAudio
    {
        [HarmonyPrefix]
        public static bool SwapAudioOnHah(ref AudioSource ___aud)
        {
            if (!___aud.isPlaying)
                AudioSwapper.SwapAudioClipSource(___aud, "death");
            return !___aud.isPlaying;
        }
    }
}