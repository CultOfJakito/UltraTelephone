using System;
using HarmonyLib;
using UltraTelephone.Agent;
using UnityEngine;
using UnityEngine.UI;

namespace UltraTelephone.Patches
{
    [HarmonyPatch(typeof(LevelSelectPanel))]
    public static class LevelSelectPanelPatch
    {
        [HarmonyPatch("OnEnable"), HarmonyPostfix]
        static void OnEnablePostfix(LevelSelectPanel __instance)
        {
            Text text = __instance.transform.Find("Name").GetComponent<Text>();
            string levelName = AgentRegistry.LevelDatabase[__instance.levelNumber];
            if (!TelephoneData.Data.standsIncomplete.Contains(levelName))
                text.color = Color.red;
        }
    }
}
