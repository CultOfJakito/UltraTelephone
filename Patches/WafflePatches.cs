﻿using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

namespace UltraTelephone.Patches
{
    public class WafflePatches
    {
        private static int ShotRng = 1;
        private static int MoveRng = 2;
        private static float Recoil = 0.1f;

        public static System.Collections.IEnumerator Randomise()
        {
            Debug.Log("Waffle randomizer started.");

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
        //hippity hoppety your patches are my property (; 
        [HarmonyPatch(typeof(Revolver), "Shoot")]
        [HarmonyPrefix]
        public static bool PatchShootRev()
        {
            if (ShotRng == 0)
            {
                SimpleLogger.Log("fuck you");
                Jumpscare.Scare();
                return false;
            }
            else {
                CameraController.Instance.rotationX += (Recoil * FrenzyController.Instance.currentFrenzy * ShotRng);
                CameraController.Instance.rotationY += (Recoil * ShotRng * UnityEngine.Random.Range(-5, 5));
                return true;
            }
        }

        [HarmonyPatch(typeof(Shotgun), "Shoot")]
        [HarmonyPrefix]
        public static bool PatchShootShotty()
        {
            if (ShotRng == 0)
            {
                SimpleLogger.Log("fuck you");
                Jumpscare.Scare();
                return false;
            }
            else
            {
                CameraController.Instance.rotationX += (Recoil * FrenzyController.Instance.currentFrenzy * ShotRng);
                CameraController.Instance.rotationY += (Recoil * ShotRng * UnityEngine.Random.Range(-5, 5));
                return true;
            }
        }
        [HarmonyPatch(typeof(Nailgun), "Shoot")]
        [HarmonyPrefix]
        public static bool PatchShootNail()
        {
            if (ShotRng == 0)
            {
                SimpleLogger.Log("fuck you");
                Jumpscare.Scare();
                return false;
            }
            else
            {
                CameraController.Instance.rotationX += (Recoil * FrenzyController.Instance.currentFrenzy * ShotRng)/3;
                CameraController.Instance.rotationY += (Recoil * ShotRng * UnityEngine.Random.Range(-5, 5));
                return true;
            }
        }

        [HarmonyPatch(typeof(Railcannon), "Shoot")]
        [HarmonyPrefix]
        public static bool PatchShootRail()
        {
            if (ShotRng == 0)
            {
                SimpleLogger.Log("fuck you");
                Jumpscare.Scare();
                return false;
            }
            else
            {
                CameraController.Instance.rotationX += (Recoil * FrenzyController.Instance.currentFrenzy * ShotRng)*10;
                CameraController.Instance.rotationY += (Recoil * ShotRng * UnityEngine.Random.Range(-5, 5));
                return true;
            }
        }
        
        [HarmonyPatch(typeof(RocketLauncher), "Shoot")]
        [HarmonyPrefix]
        public static bool PatchShootRockit()
        {
            if (ShotRng == 0)
            {
                SimpleLogger.Log("fuck you");
                Jumpscare.Scare();
                return false;
            }
            else
            {
                CameraController.Instance.rotationX += (Recoil * FrenzyController.Instance.currentFrenzy * ShotRng);
                CameraController.Instance.rotationY += (Recoil * ShotRng * UnityEngine.Random.Range(-5, 5));
                return true;
            }
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
