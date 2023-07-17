using System;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;


public static class HydraLoader
{
    private static List<CustomAssetPrefab> assetsToRegister = new List<CustomAssetPrefab>();

    private static List<CustomAssetData> dataToRegister = new List<CustomAssetData>();

    public static Dictionary<string, UnityEngine.Object> dataRegistry = new Dictionary<string, UnityEngine.Object>();

    public static Dictionary<string, GameObject> prefabRegistry = new Dictionary<string, GameObject>();

    private static AssetBundle assetBundle;

    public static bool dataRegistered = false;
    public static bool assetsRegistered = false;

    public static Action OnBundleLoaded;

    public static bool RegisterAll(byte[] assetBundleObject)
    {
        try
        {
            Debug.Log("HydraLoader: loading mod files");
            assetBundle = AssetBundle.LoadFromMemory(assetBundleObject);
            RegisterDataFiles();
            RegisterCustomAssets();
            OnBundleLoaded?.Invoke();
            Debug.Log("HydraLoader: loading complete");
            return true;
        }
        catch (Exception e)
        {
            Debug.Log("HydraLoader: loading failed");
            Debug.LogException(e);
            return false;
        }
    }

    //Cant be bothered to rewrite this loader to fix the clutter so this is the bandaid :)
    public static T LoadAsset<T>(string assetName) where T : UnityEngine.Object
    {

        if(assetBundle == null)
        {
            Debug.LogError("Bundle is not yet loaded. Please subscribe to OnBundleLoaded, then load the asset");
            return null;
        }

        return assetBundle.LoadAsset<T>(assetName);
    }

    public static void RegisterDataFiles()
    {
        if (!dataRegistered)
        {
            foreach (CustomAssetData assetData in dataToRegister)
            {
                if (assetData.hasData)
                {
                    dataRegistry.Add(assetData.name, assetData.dataFile);
                }
                else
                {
                    dataRegistry.Add(assetData.name, assetBundle.LoadAsset(assetData.name, assetData.dataType));
                }

            }
            Debug.Log(String.Format("HydraLoader: {0} asset datas registered successfully", dataRegistry.Count));
            dataRegistered = true;
        }
    }

    public static void RegisterCustomAssets()
    {
        if (!assetsRegistered)
        {
            foreach (CustomAssetPrefab asset in assetsToRegister)
            {
                GameObject newPrefab = assetBundle.LoadAsset<GameObject>(asset.name);
                for (int i = 0; i < asset.modules.Length; i++)
                {
                    newPrefab.AddComponent(asset.modules[i].GetType());
                }
                prefabRegistry.Add(asset.name, newPrefab);
            }
            Debug.Log(String.Format("HydraLoader: {0} prefabs registered successfully", prefabRegistry.Count));

            assetsRegistered = true;
        }
    }

    public class CustomAssetPrefab
    {
        public Component[] modules;
        public string name;

        public CustomAssetPrefab(string assetName, Component[] componentsToAdd)
        {
            this.name = assetName;
            this.modules = componentsToAdd;
            assetsToRegister.Add(this);
        }
    }

    public class CustomAssetData
    {
        public string name;
        public DataFile dataFile;
        public bool hasData = false;
        public Type dataType;

        public CustomAssetData(string dataName, DataFile dataFile) //For loading custom script data, try not to use this.
        {
            this.hasData = true;
            this.name = dataName;
            this.dataFile = dataFile;
            dataToRegister.Add(this);
            //Debug.Log(String.Format("{0} of type: {1} registered successfully.", dataName, dataFile.GetType().ToString()));
        }

        public CustomAssetData(string dataName, Type type) //For loading general assets
        {
            this.name = dataName;
            this.dataType = type;
            dataToRegister.Add(this);
            //Debug.Log(String.Format("{0} of type: {1} registered successfully.", dataName, type.ToString()));
        }
    }
}

public class DataFile : UnityEngine.Object { }

