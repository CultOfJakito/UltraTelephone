using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using UnityEngine.UI;
using UnityEngine;

namespace UltraTelephone.Hydra
{
    public class UWUPatch
    {

        private static string[] faces = new string[] { "OwO", "UwU", "umu", ">m<", "^.^", ">:3c", "uwu", "O-O", "q-q", "X_x", "^o^", "Q.Q" };

        [HarmonyPatch(typeof(HudMessageReceiver), "SendHudMessage")]
        public static class HUDMessageReceiverUWUPatch
        {
            public static bool Prefix(HudMessageReceiver __instance, ref string newmessage)
            {
                if(HydrasConfig.Patches_UWU)
                {
                    newmessage = SimpleLogger.DecryptContent(newmessage);
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(HudMessage), nameof(HudMessage.PlayMessage))]
        public static class HUDMessageUWUPatch
        {
            public static bool Prefix(HudMessage __instance)
            {
                if (HydrasConfig.Patches_UWU)
                {
                    __instance.message = SimpleLogger.DecryptContent(__instance.message);
                    __instance.message2 = SimpleLogger.DecryptContent(__instance.message2);
                    __instance.message = faces[UnityEngine.Random.Range(0, faces.Length)] + " " + __instance.message;
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(LevelNamePopup), "NameAppear")]
        public static class LayerUWUPatch
        {
            public static bool Prefix(LevelNamePopup __instance, ref string ___layerString, ref string ___nameString)
            {
                if (HydrasConfig.Patches_UWU)
                {
                    ___nameString = SimpleLogger.DecryptContent(___nameString);
                    ___layerString = SimpleLogger.DecryptContent(___layerString);
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(StyleHUD), "GetLocalizedName")]
        public static class StyleHUDUWUPatch
        {
            public static void Postfix(string id, ref string __result)
            {
                if (HydrasConfig.Patches_UWU)
                {
                    __result = SimpleLogger.DecryptContent(__result);
                }
            }
        }

        public static Font comicSans, heartless;

        [HarmonyPatch(typeof(Text), "OnEnable")]
        public static class UWUTextPatcher
        {
            public static void Postfix(Text __instance)
            {
                if (!HydrasConfig.Patches_UWU)
                {
                    return;
                }

                    int rand = UnityEngine.Random.Range(0, 6);

                switch (rand)
                {
                    case 0:
                        if (comicSans != null)
                        {
                            __instance.font = comicSans;
                        }
                        break;
                    case 1:
                        if (heartless != null)
                        {
                            __instance.font = heartless;
                        }
                        break;
                }

                string text = __instance.text;
                if (text != null || text != "")
                {
                    __instance.text = SimpleLogger.DecryptContent(text);
                }

                if(rand == 2)
                {
                    __instance.text = faces[UnityEngine.Random.Range(0, faces.Length)] + " " + __instance.text;
                }

            }
        }

        [HarmonyPatch(typeof(Image), "OnEnable")]
        public static class LogoPatcher
        {
            public static void Postfix(Image __instance)
            {
                if (__instance.sprite != null)
                {
                    if (__instance.sprite.name == "logowideborderlesssmaller")
                    {
                        if (HydraLoader.dataRegistry.TryGetValue("UltraTelephoneHeader", out UnityEngine.Object obj))
                        {
                            Sprite newLogo = (Sprite)obj;
                            if (newLogo != null)
                            {
                                __instance.sprite = newLogo;
                            }
                        }
                    }
                }
            }
        }

        [HarmonyPatch(typeof(Button), "Press")]
        public static class ButtonPatcher
        {
            public static void Postfix()
            {
                if(UnityEngine.Random.value > 0.5f)
                {
                    RandomSounds.PlayRandomSound();
                }
            }
        }


        private static Dictionary<AudioClip, AudioClip> swapDictionary = new Dictionary<AudioClip, AudioClip>();
        private static Queue<AudioClip> swapQueue = new Queue<AudioClip>();

        [HarmonyPatch(typeof(AudioSource), "Play", new Type[] { })]
        public static class AudioMusher
        {
            public static bool Prefix(AudioSource __instance)
            {
                if(HydrasConfig.Patches_AudioFuckery)
                {
                    if (__instance.clip != null)
                    {
                        __instance.clip = GetSwappedClip(__instance.clip);
                    }
                }
                return true;
            }


            private static AudioClip GetSwappedClip(AudioClip clip)
            {

                if (swapDictionary.TryGetValue(clip, out AudioClip swappedClip))
                {
                    return swappedClip;
                }

                if(swapQueue.Count > 0)
                {
                    if(!swapQueue.Contains(clip))
                    {
                        AudioClip swapClip = swapQueue.Dequeue();
                        swapDictionary.Add(clip, swapClip);
                        swapDictionary.Add(swapClip, clip);
                        return swapClip;
                    }
                }else
                {
                    swapQueue.Enqueue(clip);
                }

                return clip;
            }
        }
    }

}

