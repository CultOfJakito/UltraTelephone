using System;
using System.Collections.Generic;
using BepInEx;
using ULTRAKILL;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using BepInEx.Configuration;
using System.Collections;
using HarmonyLib;
using UltraTelephone.Patches;
using UltraTelephone.Agent;

namespace UltraTelephone
{
    [BepInPlugin("ukdiscord_ultratelephone", PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        
        public static Plugin plugin;

        // I'm not sorry :D
        private void Awake()
        {
            plugin = this;

            if (TelephoneData.CheckDataPresent())
            {
                SimpleLogger.Log("Hello? Hello? Hello? UltraTelephone is starting.");
            }else
            {
                Debug.LogError("UltraTelephone could not find necessary data. Did you forget to copy the UltraTelephone_Data directory?");
                this.enabled = false;
                return;
            }

            /*Fun static functions from Hydra
             * Jumpscare.Scare();
             * BirdFreer.FreeBird();
             * RandomSounds.PlayRandomSound();
             * 
             */


            BestUtilityEverCreated.Initialize(); //this class its perfectly useful see it for uses!
            Harmony harmony = new Harmony("ukdiscord_ultratelephone");
            harmony.PatchAll();
            StartCoroutine(WafflePatches.Randomise());
            harmony.PatchAll(typeof(WafflePatches));
            StartCoroutine(AudioSwapper.Initialize(this));
            StartCoroutine(AgentRegistry.GetAudio());
            FoodStandInitializer.Init();
            WeaponAutomator.Init();
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
            

            // IF SOMEONE DECIDES TO USE THE KEYBIDNS THEY CAN FIX THE CLASS :(
            // OH ALSO YOU'LL NEED TO FIX UIPatch.cs it is being returned idk what is for
            
            //KeyBindings.Init();
            //KeyBindings.RegisterMod(thisMod);
            //KeyBindings.RegisterKey(thisMod, "IDK", Keyboard.current.f3Key.path);
        }

 
        
    }
}

