using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace UltraTelephone.Patches
{
    public class WafflePatches
    {
        private static int ShotRng = 1;
        private static int MoveRng = 2;

        public static System.Collections.IEnumerator Randomise()
        {
            while (true)
            {
                if (NewMovement.Instance != null)
                {
                    yield return new WaitForSeconds(2);
                    try
                    {
                        ShotRng = UnityEngine.Random.Range(0, 16);
                        MoveRng = UnityEngine.Random.Range(0, 11);
                        NewMovement.Instance.walkSpeed = UnityEngine.Random.Range(720, 780);
                        //SimpleLogger.Log($"Generated RNG | Shot: {ShotRng}, Move: {MoveRng}, Speed: {NewMovement.Instance.walkSpeed}");
                    }
                    catch (Exception ex)
                    {
                        //Debug.LogError("guh? " + ex.ToString());
                    }
                } else
                {
                    yield return null;
                }
            }
        }

        [HarmonyPatch(typeof(Revolver), "Shoot")]
        [HarmonyPrefix]
        public static bool PatchShootRev()
        {
            if(ShotRng == 0)
            {
                Debug.Log("fuck you");
                HudMessageReceiver.Instance.SendHudMessage("You have run out of ammo", "", "", 0, true);
                return false;
            }
            return true;
        }

        [HarmonyPatch(typeof(Shotgun), "Shoot")]
        [HarmonyPrefix]
        public static bool PatchShootShotty()
        {
            if (ShotRng == 0)
            {
                SimpleLogger.Log("fuck you");
                HudMessageReceiver.Instance.SendHudMessage("You have run out of ammo", "", "", 0, true);
                return false;
            }
            return true;
        }

        [HarmonyPatch(typeof(Nailgun), "Shoot")]
        [HarmonyPrefix]
        public static bool PatchShootNail()
        {
            if (ShotRng == 0)
            {
                SimpleLogger.Log("fuck you");
                HudMessageReceiver.Instance.SendHudMessage("You have run out of ammo", "", "", 0, true);
                return false;
            }
            return true;
        }

        [HarmonyPatch(typeof(Railcannon), "Shoot")]
        [HarmonyPrefix]
        public static bool PatchShootRail()
        {
            if (ShotRng == 0)
            {
                SimpleLogger.Log("fuck you");
                HudMessageReceiver.Instance.SendHudMessage("You have run out of ammo", "", "", 0, true);
                return false;
            }
            return true;
        }

        [HarmonyPatch(typeof(RocketLauncher), "Shoot")]
        [HarmonyPrefix]
        public static bool PatchShootRockit()
        {
            if (ShotRng == 0)
            {
                SimpleLogger.Log("fuck you");
                HudMessageReceiver.Instance.SendHudMessage("You have run out of ammo", "", "", 0, true);
                return false;
            }
            return true;
        }

        [HarmonyPatch(typeof(NewMovement), "Dodge")]
        [HarmonyPrefix]
        public static bool PatchDodge()
        {
            if (MoveRng == 0)
            {
                SimpleLogger.Log("fuck you");
                HudMessageReceiver.Instance.SendHudMessage("Your jet jammed.", "", "", 0, true);
                return false;
            }
            return true;
        }

        [HarmonyPatch(typeof(NewMovement), "Jump")]
        [HarmonyPrefix]
        public static bool PatchJump()
        {
            if (MoveRng == 0)
            {
                SimpleLogger.Log("fuck you");
                HudMessageReceiver.Instance.SendHudMessage("Your legs broke.", "", "", 0, true);
                return false;
            }
            return true;
        }
    }
}
