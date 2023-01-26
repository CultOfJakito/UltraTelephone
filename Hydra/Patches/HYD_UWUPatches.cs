using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using UnityEngine.UI;
using UnityEngine;

public class HYD_UWUPatches
{

    [HarmonyPatch(typeof(HudMessageReceiver), "SendHudMessage")]
    public static class HUDMessageReceiverUWUPatch
    {
        public static bool Prefix(HudMessageReceiver __instance, ref string newmessage)
        {
            newmessage = SimpleLogger.DecryptContent(newmessage);
            return true;
        }
    }

    [HarmonyPatch(typeof(HudMessage), nameof(HudMessage.PlayMessage))]
    public static class HUDMessageUWUPatch
    {
        public static bool Prefix(HudMessage __instance)
        {
            __instance.message = SimpleLogger.DecryptContent(__instance.message);
            __instance.message2 = SimpleLogger.DecryptContent(__instance.message2);
            return true;
        }
    }

    [HarmonyPatch(typeof(LevelNamePopup), "NameAppear")]
    public static class LayerUWUPatch
    {
        public static bool Prefix(LevelNamePopup __instance, ref string ___layerString, ref string ___nameString)
        {

            ___nameString = SimpleLogger.DecryptContent(___nameString);
            ___layerString = SimpleLogger.DecryptContent(___layerString);
            return true;
        }
    }

    [HarmonyPatch(typeof(StyleHUD), "GetLocalizedName")]
    public static class StyleHUDUWUPatch
    {
        public static void Postfix(string id, ref string __result)
        {
            __result = SimpleLogger.DecryptContent(__result);
        }
    }

    public static Font comicSans, heartless;

    [HarmonyPatch(typeof(Text), "OnEnable")]
    public static class UWUTextPatcher
    {
        public static void Postfix(Text __instance)
        {
            int rand = UnityEngine.Random.Range(0, 6);

            switch(rand)
            {
                case 0:
                    if(comicSans != null)
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
            if(text != null || text != "")
            {
                __instance.text = SimpleLogger.DecryptContent(text);
            }
        }
    }
}
