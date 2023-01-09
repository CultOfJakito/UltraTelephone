using System;
using BepInEx;
using ULTRAKILL;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System.Collections;
using HarmonyLib;
using UltraTelephone.Patches;

namespace UltraTelephone
{
    [BepInPlugin("ukdiscord_ultratelephone", PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        private static InputAction idkKey;
        private static bool keysReady = false;
        private static Mod thisMod = new Mod("ukdiscord_ultratelephone", RefreshKeys);
        private void Awake()
        {
            BestUtilityEverCreated.Initialize(); //this class its perfectly useful see it for uses!
            Harmony harmony = new Harmony("ukdiscord_ultratelephone");
            harmony.PatchAll();
            StartCoroutine(WafflePatches.Randomise());
            harmony.PatchAll(typeof(WafflePatches));
            StartCoroutine(AudioSwapper.Initialize(this));
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
            
            // IF SOMEONE DECIDES TO USE THE KEYBIDNS THEY CAN FIX THE CLASS :(
            // OH ALSO YOU'LL NEED TO FIX UIPatch.cs it is being returned idk what is for

            //KeyBindings.Init();
            //SceneManager.sceneLoaded += Scene;
            //KeyBindings.RegisterMod(thisMod);
            //KeyBindings.RegisterKey(thisMod, "IDK", Keyboard.current.f3Key.path);
        }
        public static void RefreshKeys()
        {
            Debug.Log("Refreshed keys");
            keysReady = false;
            KeyBindings.keys.TryGetValue("ukdiscord_ultratelephone.IDK", out idkKey);
            keysReady = true;
        }
        private void Scene(Scene scene, LoadSceneMode mode)
        {
            if (scene.name.Contains(""))
            {
                
            }
        }
        private void Update()
        {
            if (keysReady)
            {
                if (idkKey.WasPressedThisFrame())
                {
                    Logger.LogInfo("I have some idea what i'm doing"); // NO YOU FUCKING DON'T, YOU CALLED NEW INSTEAD OF INSTANTIATE SO YOU CLEARLY DON'T
                    GameObject laughingSkull = new GameObject();
                    laughingSkull.AddComponent<LaughingSkull>();
                }
            }
        }
    }
}
