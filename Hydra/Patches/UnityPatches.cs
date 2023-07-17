using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace UltraTelephone.Hydra.Patches
{
    public static class UnityPatches
    {
        [HarmonyPatch(typeof(Rigidbody))]
        public static class PhysicsPatches
        {
            [HarmonyPatch(nameof(Rigidbody.AddForce), new[] { typeof(Vector3), typeof(ForceMode) })]
            [HarmonyPrefix]
            public static bool AddForcePatch(Rigidbody __instance, ref Vector3 force, ForceMode mode)
            {
                if(UnityEngine.Random.value > 0.75f && HydrasConfig.Patches_WackyPhysics)
                {
                    force *= (UnityEngine.Random.value > 0.4999f) ? (UnityEngine.Random.Range(0.5f,1.0f)) : UnityEngine.Random.Range(1.5f, 20.0f);
                }

                return true;
            }



        }


    }
}
