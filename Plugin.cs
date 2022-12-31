using BepInEx;
using ULTRAKILL;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System.Collections;

namespace UltraTelephone
{
    [BepInPlugin("ukdiscord_ultratelephone", PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        private static InputAction idkKey;
        private static bool keysReady = false;
        private static Mod thisMod = new Mod("ukdiscord_ultratelephone", Useless);
        private void Awake()
        {
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
            KeyBindings.Init();
            SceneManager.sceneLoaded += Scene;
            KeyBindings.RegisterKey(thisMod,"IDK",Keyboard.current.f3Key.path);
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
            if(scene.name.Contains(""))
            {

            }
        }
        private void Update()
        {
            if(keysReady)
            {
                /*if(idkKey.WasPressedThisFrame())
                {
                    Logger.LogInfo("I have no idea what i'm doing");
                }*/
            }
        }
        private static void Useless()
        {

        }
    }
}
