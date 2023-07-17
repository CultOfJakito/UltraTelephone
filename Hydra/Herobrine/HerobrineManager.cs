using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace UltraTelephone.Hydra
{
    public static class HerobrineManager
    {
        private static GameObject herobrinePrefab;
        private static Herobrine herobrine;

        private static bool initialized;

        public static void Init()
        {
            if (initialized)
                return;

            if (!HydrasConfig.Herobrine_Enabled)
                return;

            initialized = true;
            HydraLoader.OnBundleLoaded += LoadHerobrine;
        }

        private static void LoadHerobrine()
        {
            HydraLoader.OnBundleLoaded -= LoadHerobrine;
            herobrinePrefab = HydraLoader.LoadAsset<GameObject>("Herobrine");
            Debug.Log("LOADED HEROBRINE");
            SpawnHerobrine();
        }

        private static void SpawnHerobrine()
        {
            GameObject herobrineGO = GameObject.Instantiate(herobrinePrefab);
            herobrine = herobrineGO.GetComponent<Herobrine>();
            GameObject.DontDestroyOnLoad(herobrineGO);
            Debug.Log("SPAWNED HEROBRINE");
        }
    }
}
