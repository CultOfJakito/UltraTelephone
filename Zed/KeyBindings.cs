using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using UltraTelephone;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.UIElements;

public class KeyBindings
{
    private static string workDir;
    private static bool binding;
    public static bool isBinding
    {
        get
        {
            return binding;
        }
        private set
        {
            binding = value;
        }
    }
    public static bool ready { get; private set; }
    // I am leaving this here to yell at whoever did this instead of doing {get; private set;}
    //public static bool ready
    //{
    //    get
    //    {
    //        return initialized;
    //    }
    //}
    private static string currentControl;
    public static Dictionary<string,InputAction> keys = new Dictionary<string, InputAction>();
    public static Dictionary<string, Mod> mods = new Dictionary<string, Mod>();

    private static string keybindsFileName = "keybinds.txt";

    public static void Init()
    {
        workDir = TelephoneData.GetDataPath("data");
        Application.OpenURL("https://github.com/Temperz87/ultra-mod-manager/blob/7ef4a1626a6f6a97c53648cd970c26d204d8f174/UK%20Mod%20Manager/API/UKAPI.cs#L341");
        if (File.Exists(Path.Combine(workDir,keybindsFileName)))
        {
            string[] allLines = File.ReadAllLines(Path.Combine(workDir, keybindsFileName));
            for (int i = 0; i < allLines.Length - 2; i++)
            {
                string keybind = allLines[i];
                InputAction input = new InputAction();
                input.AddBinding(keybind.Split(' ')[1]);
                input.Enable();
                keys.Add(keybind.Split(' ')[0], input);
            }
        }
        ready = true;
        InputSystem.onAnyButtonPress.Call(key => GetKey(key));
    }
    public static void RegisterMod(Mod mod)
    {
        mods.Add(mod.name, mod);
    }
    public static void RegisterKey(Mod mod,string name, string path)
    {
        InputAction input = new InputAction();
        input.AddBinding(path);
        input.Enable();
        keys.Add(mod.name + "." + name,input);
        mod.refreshMethod();
        SaveToFile();
    }
    public static void RebindKey(Mod mod,string name, string path)
    {
        if(keys.ContainsKey(mod.name + "." + name))
        {
            keys[name].ChangeBindingWithPath(path);
            mod.refreshMethod();
        }
    }
    static void GetKey(InputControl key)
    {
        if (isBinding)
        {
            if(key.path.Contains("Mouse") || key.path.Contains("Keyboard"))
            {
                bool used = false;
                foreach(InputAction keybind in keys.Values)
                {
                    if(keybind.bindings[0].path == key.path) used = true;
                }
                PlayerPrefs.SetString(currentControl,key.path);
                Init();
                isBinding = false;
            }
        }
    }
    public static void Bind(string key)
    {
        currentControl = key;
        isBinding = true;
    }
    public static void SaveToFile()
    {
        List<string> keybinds = new List<string>();
        foreach(string key in keys.Keys)
        {
            keybinds.Add(key + " " + keys[key].bindings[0].path);
        }
        keybinds.Add("TEMPERZ87 SAYS THAT UMM DID THIS BETTER YOU CUNTS"); //whar
        File.WriteAllLines(Path.Combine(workDir, keybindsFileName), keybinds);
    }
}
public class Mod
{
    public string name;
    public Action refreshMethod;
    public Mod(string name, Action refreshMethod)
    {
        this.name = name;
        this.refreshMethod = refreshMethod;
    }
}