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

        // Let all hell break loose
        Harmony harm;
        ConfigEntry<bool> _isRandom,_useVariations;
        ConfigEntry<float> _interval,_minInterval,_maxInterval;
        ConfigEntry<GameType> _gameType;
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
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
            plugin = this;
            canSwitch = true;
            Init();
            SceneManager.sceneLoaded += Scene;
            harm = Harmony.CreateAndPatchAll(typeof(Patches));
            
            // IF SOMEONE DECIDES TO USE THE KEYBIDNS THEY CAN FIX THE CLASS :(
            // OH ALSO YOU'LL NEED TO FIX UIPatch.cs it is being returned idk what is for

            //KeyBindings.Init();
            //KeyBindings.RegisterMod(thisMod);
            //KeyBindings.RegisterKey(thisMod, "IDK", Keyboard.current.f3Key.path);
        }
        public void Init()
        {
            _isRandom = Config.Bind<bool>("Random","Use a random Interval",false,"Is the interval between gun switching random?");
            _useVariations = Config.Bind<bool>("Variations","Use weapon variations",true,"Account for weapon variations?");
            _minInterval = Config.Bind<float>("Min Interval","Minimum Interval",1f,"Minimum amount of time before switching");
            _maxInterval = Config.Bind<float>("Max Interval","Maximum Interval",3f,"Maximum amount of time before switching");
            _interval = Config.Bind<float>("Interval","Fixed Interval",3f,"Fixed amount of time before switching");
            gameType = _gameType.Value;
            isRandom = _isRandom.Value;
            useVariations = _useVariations.Value;
            minInterval = _minInterval.Value;
            maxInterval = _maxInterval.Value;
            interval = _interval.Value;
            Logger.LogInfo("Ready for randomness!");
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

            /*foreach(GameObject g in gc.slot6)
            {
                
            }*/
            yield return new WaitForSeconds(time);

            if(isRandom) nextTime = Random.Range(minInterval,maxInterval);
            else nextTime = interval;
            try
            {
                if(useVariations)
                {
                    int weapon,variation;
                    weapon = availableWeapons[Random.Range(0,availableWeapons.Count)];
                    variation = Random.Range(0,3);
                    for(int i = 0;i < variation;i++)
                    {
                        gc.SwitchWeapon(weapon);
                    }
                }
                else
                {
                    int weapon;
                    weapon = availableWeapons[Random.Range(0,availableWeapons.Count + 1)];
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
        IEnumerator SwitchImmediately()
        {
            canSwitch = false;
            //Debug.Log("SwitchImmediately");
            if(gameType != GameType.GunGame) yield break;
            List<int> availableWeapons = new List<int>();
            GunControl gc = GunControl.Instance;

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

            /*foreach(GameObject g in gc.slot6)
            {
                
            }*/
            if(useVariations)
            {
                int weapon,variation;
                weapon = availableWeapons[Random.Range(0,availableWeapons.Count)];
                variation = Random.Range(0,3);
                for(int i = 0;i < variation;i++)
                {
                    gc.SwitchWeapon(weapon);
                }
            }
            else
            {
                int weapon;
                weapon = availableWeapons[Random.Range(0,availableWeapons.Count + 1)];
                gc.SwitchWeapon(weapon);
            }
            canSwitch = true;
            yield break;
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
                if(gameType == GameType.Timed)
                {
                    float time;
                    if(isRandom) time = Random.Range(minInterval,maxInterval);
                    else time = interval;
                    StartCoroutine(Switch(time));
                } 
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
public class Patches
{
    // Disable weapon switching so the mod isn't useless
    [HarmonyPatch(typeof(PlayerInput),"Slot1",MethodType.Getter)]
    [HarmonyPrefix]
    public static bool DisableSlot1(ref InputActionState __result)
    {
        InputAction action = new InputAction();
        InputActionState state = new InputActionState(action);
        __result = state;
        return false;
    }
    [HarmonyPatch(typeof(PlayerInput),"Slot2",MethodType.Getter)]
    [HarmonyPrefix]
    public static bool DisableSlot2(ref InputActionState __result)
    {
        InputAction action = new InputAction();
        InputActionState state = new InputActionState(action);
        __result = state;
        return false;
    }
    [HarmonyPatch(typeof(PlayerInput),"Slot3",MethodType.Getter)]
    [HarmonyPrefix]
    public static bool DisableSlot3(ref InputActionState __result)
    {
        InputAction action = new InputAction();
        InputActionState state = new InputActionState(action);
        __result = state;
        return false;
    }
    [HarmonyPatch(typeof(PlayerInput),"Slot4",MethodType.Getter)]
    [HarmonyPrefix]
    public static bool DisableSlot4(ref InputActionState __result)
    {
        InputAction action = new InputAction();
        InputActionState state = new InputActionState(action);
        __result = state;
        return false;
    }
    [HarmonyPatch(typeof(PlayerInput),"Slot5",MethodType.Getter)]
    [HarmonyPrefix]
    public static bool DisableSlot5(ref InputActionState __result)
    {
        InputAction action = new InputAction();
        InputActionState state = new InputActionState(action);
        __result = state;
        return false;
    }
    [HarmonyPatch(typeof(PlayerInput),"ChangeVariation",MethodType.Getter)]
    [HarmonyPrefix]
    public static bool DisableChangeVariaion(ref InputActionState __result)
    {
        InputAction action = new InputAction();
        InputActionState state = new InputActionState(action);
        __result = state;
        return false;
    }
    [HarmonyPatch(typeof(PlayerInput),"LastWeapon",MethodType.Getter)]
    [HarmonyPrefix]
    public static bool DisableLastWeapon(ref InputActionState __result)
    {
        InputAction action = new InputAction();
        InputActionState state = new InputActionState(action);
        __result = state;
        return false;
    }
    [HarmonyPatch(typeof(PlayerInput),"NextWeapon",MethodType.Getter)]
    [HarmonyPrefix]
    public static bool DisableNextWeapon(ref InputActionState __result)
    {
        InputAction action = new InputAction();
        InputActionState state = new InputActionState(action);
        __result = state;
        return false;
    }
    [HarmonyPatch(typeof(PlayerInput),"PrevWeapon",MethodType.Getter)]
    [HarmonyPrefix]
    public static bool DisablePrevWeapon(ref InputActionState __result)
    {
        InputAction action = new InputAction();
        InputActionState state = new InputActionState(action);
        __result = state;
        return false;
    }
    [HarmonyPatch(typeof(PlayerInput),"WheelLook",MethodType.Getter)]
    [HarmonyPrefix]
    public static bool DisableWheelLook(ref InputActionState __result)
    {
        InputAction action = new InputAction();
        InputActionState state = new InputActionState(action);
        __result = state;
        return false;
    }
}

