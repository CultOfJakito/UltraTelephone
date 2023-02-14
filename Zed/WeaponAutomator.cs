using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine.InputSystem;

public static class WeaponAutomator
{
    // Let all hell break loose

    private static WeaponAutomatorComponent component;

    public static WeaponAutomatorComponent Component
    {
        get
        {
            if(component == null)
            {
                return NewComponent();
            }
            return component;
        }
    }

    private static WeaponAutomatorComponent NewComponent()
    {
        GameObject newAutomatorObject = new GameObject("Weapon Automator");
        component = newAutomatorObject.AddComponent<WeaponAutomatorComponent>();
        return component;
    }

    public static void Init()
    {
        NewComponent();
    }
}

public class WeaponAutomatorComponent : MonoBehaviour
{
    private static InputAction idkKey;
    private static bool keysReady = false;
    private static Mod thisMod = new Mod("ukdiscord_ultratelephone", RefreshKeys);

    ConfigEntry<bool> _isRandom, _useVariations, _isEnabled;
    ConfigEntry<float> _interval, _minInterval, _maxInterval;

    float interval, minInterval, maxInterval;
    public bool isRandom, useVariations, started, canSwitch;
    Harmony harm;

    private float waitTime = 0.0f;
    private GunControl gc;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        BestUtilityEverCreated.OnLevelChanged += OnLevelLoad;
        Init();
    }

    private void OnLevelLoad(BestUtilityEverCreated.UltrakillLevelType levelType)
    {
        if (BestUtilityEverCreated.InLevel())
        {
            gc = GunControl.Instance;
        }
    }

    public void Init()
    {
        canSwitch = true;
        _isEnabled = UltraTelephone.Plugin.UltraTelephone.Config.Bind<bool>("ZedPatch Enabled?", "Enable ZedDev patch for maximum suffering", true, "Is the randomizer enabled or not?");
        _isRandom = UltraTelephone.Plugin.UltraTelephone.Config.Bind<bool>("Random", "Use a random Interval", false, "Is the interval between gun switching random?");
        _useVariations = UltraTelephone.Plugin.UltraTelephone.Config.Bind<bool>("Variations", "Use weapon variations", true, "Account for weapon variations?");
        _minInterval = UltraTelephone.Plugin.UltraTelephone.Config.Bind<float>("Min Interval", "Minimum Interval", 1f, "Minimum amount of time before switching");
        _maxInterval = UltraTelephone.Plugin.UltraTelephone.Config.Bind<float>("Max Interval", "Maximum Interval", 3f, "Maximum amount of time before switching");
        _interval = UltraTelephone.Plugin.UltraTelephone.Config.Bind<float>("Interval", "Fixed Interval", 2f, "Fixed amount of time before switching");
        isRandom = _isRandom.Value;
        useVariations = _useVariations.Value;
        minInterval = _minInterval.Value;
        maxInterval = _maxInterval.Value;
        interval = _interval.Value;
        if (_isEnabled.Value)
        {
            harm = Harmony.CreateAndPatchAll(typeof(ZedPatches));
            StartCoroutine(Switch());
            SimpleLogger.Log("Ready for randomness! (and pain)");
        }
        else SimpleLogger.Log("Sad ZedDev noises :'(");
    }

    IEnumerator Switch()
    {
        while (canSwitch)
        {
            //Stop automation while the AllGunsEnabled bruh moment is active.
            while(UltraTelephone.Hydra.AllGuns.AllGunsEnabled)
            {
                yield return new WaitForSeconds(2);
            }

            if (gc != null && BestUtilityEverCreated.InLevel())
            {
                List<int> availableWeapons = new List<int>();
                for(int i =0; i<gc.slots.Count ;i++)
                {
                    if(gc.slots[i].Count > 0 && i < 6)
                    {
                        availableWeapons.Add(i);
                    }
                }
                waitTime = interval;
                if (availableWeapons.Count > 0)
                {
                    // Reserved for "when" a 6th weapon drops, else why is there a slot 6 variable in the code?
                    // because of the spawner arm
                    // Oh ok that makes sense. Im just gonna leave it here anyways :)

                    /*foreach(GameObject g in gc.slot6)
                    {

                    }*/

                    waitTime = (!isRandom) ? interval : UnityEngine.Random.Range(minInterval, maxInterval);

                    int weapon = availableWeapons[UnityEngine.Random.Range(0, availableWeapons.Count - 1)];
                    int variation = 1;

                    if (useVariations)
                    {
                        variation = UnityEngine.Random.Range(0, 2);
                    }

                    for (int i = 0; i < variation; i++)
                    {
                        DoSwitch(weapon);
                    }
                }
            }

            yield return new WaitForSeconds(waitTime);
        }     
    }

    private void DoSwitch(int num)
    {
        if(gc != null)
        {
            try
            {
                gc.SwitchWeapon(num);
            }
            catch (System.Exception e)
            {
                SimpleLogger.Log("We did not switch weapons so we will do someting funny. \nGunControl.SwitchWeapon() is cursed....");
                UltraTelephone.Hydra.Jumpscare.Scare();
            }
        }
    }

    public static void RefreshKeys()
    {
        Debug.Log("Refreshed keys");
        keysReady = false;
        KeyBindings.keys.TryGetValue("ukdiscord_ultratelephone.IDK", out idkKey);
        keysReady = true;
    }

    private void Update()
    {
        if (keysReady)
        {
            if (idkKey.WasPressedThisFrame())
            {
                SimpleLogger.Log("I have some idea what i'm doing"); // NO YOU FUCKING DON'T, YOU CALLED NEW INSTEAD OF INSTANTIATE SO YOU CLEARLY DON'T

                //What is this.
                GameObject laughingSkull = new GameObject();
                laughingSkull.AddComponent<LaughingSkull>();
            }
        }
    }

    private void OnDestroy()
    {
        BestUtilityEverCreated.OnLevelChanged -= OnLevelLoad;
    }
}
