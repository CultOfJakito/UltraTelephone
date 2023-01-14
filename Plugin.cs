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
        private static InputAction idkKey;
        private static bool keysReady = false;
        private static Mod thisMod = new Mod("ukdiscord_ultratelephone", RefreshKeys);

        // Let all hell break loose
        Harmony harm;
        ConfigEntry<bool> _isRandom,_useVariations,_isEnabled;
        ConfigEntry<float> _interval,_minInterval,_maxInterval;
        float interval,minInterval,maxInterval;
        public bool isRandom,useVariations,started,canSwitch;
        public static Plugin plugin;

        // I'm not sorry :D
        private void Awake()
        {
            BestUtilityEverCreated.Initialize(); //this class its perfectly useful see it for uses!
            Harmony harmony = new Harmony("ukdiscord_ultratelephone");
            harmony.PatchAll();
            StartCoroutine(WafflePatches.Randomise());
            harmony.PatchAll(typeof(WafflePatches));
            StartCoroutine(AudioSwapper.Initialize(this));
            StartCoroutine(AgentRegistry.GetAudio());
            FoodStandInitializer.Init();
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
            plugin = this;
            canSwitch = true;
            Init();
            SceneManager.sceneLoaded += Scene;
            

            // IF SOMEONE DECIDES TO USE THE KEYBIDNS THEY CAN FIX THE CLASS :(
            // OH ALSO YOU'LL NEED TO FIX UIPatch.cs it is being returned idk what is for
            
            //KeyBindings.Init();
            //KeyBindings.RegisterMod(thisMod);
            //KeyBindings.RegisterKey(thisMod, "IDK", Keyboard.current.f3Key.path);
        }
        public void Init()
        {
            _isEnabled = Config.Bind<bool>("ZedPatch Enabled?","Enable ZedDev patch for maximum suffering",true, "Is the randomizer enabled or not?");
            _isRandom = Config.Bind<bool>("Random","Use a random Interval",false,"Is the interval between gun switching random?");
            _useVariations = Config.Bind<bool>("Variations","Use weapon variations",true,"Account for weapon variations?");
            _minInterval = Config.Bind<float>("Min Interval","Minimum Interval",1f,"Minimum amount of time before switching");
            _maxInterval = Config.Bind<float>("Max Interval","Maximum Interval",3f,"Maximum amount of time before switching");
            _interval = Config.Bind<float>("Interval","Fixed Interval",3f,"Fixed amount of time before switching");
            isRandom = _isRandom.Value;
            useVariations = _useVariations.Value;
            minInterval = _minInterval.Value;
            maxInterval = _maxInterval.Value;
            interval = _interval.Value;
            if(_isEnabled.Value)
            {
                harm = Harmony.CreateAndPatchAll(typeof(ZedPatches));
                Logger.LogInfo("Ready for randomness! (and pain)");
            }
            else Logger.LogInfo("Sad ZedDev noises :'(");

        }
        IEnumerator Switch(float time)
        {
            List<int> availableWeapons = new List<int>();
            if(started)
            {
                yield return new WaitForSeconds(3);
                started = false;
            }
            GunControl gc = GunControl.Instance;
            float nextTime;

            // Check all available weapons
            if(gc.slot1.Count > 0)
            {
                availableWeapons.Add(1);
            }
            if(gc.slot2.Count > 0)
            {
                availableWeapons.Add(2);
            }
            if(gc.slot3.Count > 0)
            {
                availableWeapons.Add(3);
            }
            if(gc.slot4.Count > 0)
            {
                availableWeapons.Add(4);
            }
            if(gc.slot5.Count > 0)
            {
                availableWeapons.Add(5);
            }
            // Reserved for when a 6th weapon drops, else why is there a slot 6 variable in the code?
            // because of the spawner arm

            /*foreach(GameObject g in gc.slot6)
            {
                
            }*/
            yield return new WaitForSeconds(time);

            if(isRandom) nextTime = UnityEngine.Random.Range(minInterval,maxInterval);
            else nextTime = interval;
            try
            {
                if(useVariations)
                {
                    int weapon,variation;
                    weapon = availableWeapons[UnityEngine.Random.Range(0,availableWeapons.Count)];
                    variation = UnityEngine.Random.Range(0,3);
                    for(int i = 0;i < variation;i++)
                    {
                        gc.SwitchWeapon(weapon);
                    }
                }
                else
                {
                    int weapon;
                    weapon = availableWeapons[UnityEngine.Random.Range(0,availableWeapons.Count + 1)];
                    gc.SwitchWeapon(weapon);
                }
            }
            catch
            {
                StartCoroutine(Switch(nextTime));
            }

            // Reset available weapons

            availableWeapons.Clear();
            StartCoroutine(Switch(nextTime));
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
            started = true;
            StopAllCoroutines();
            if(!scene.name.Contains("Menu"))
            {
                float time;
                if(isRandom) time = UnityEngine.Random.Range(minInterval,maxInterval);
                else time = interval;
                StartCoroutine(Switch(time));
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

