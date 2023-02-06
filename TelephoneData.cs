using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using UnityEngine;
using Newtonsoft.Json;

public static class TelephoneData
{
    private const string folderName = "UltraTelephone_Data";
    private static string dataFilePath = GetDataPath("data", "save.telephone");

    public static string GetDataPath(params string[] subpath)
    {
        string modDir = Assembly.GetExecutingAssembly().Location;
        modDir = Path.GetDirectoryName(modDir);
        string localPath = Path.Combine(modDir, folderName);

        if(subpath.Length > 0)
        {
            string subLocalPath = Path.Combine(subpath);
            localPath = Path.Combine(localPath, subLocalPath);
        }

        return localPath;
    }

    private static string[] dataToCheck = new string[] { "tex", "data", "audio" };

    public static bool CheckDataPresent()
    {

        if(!Directory.Exists(GetDataPath()))
        {
            Debug.LogError($"UltraTelephone_Data is missing. Please check mod installation.");
            return false;
        }

        for (int i = 0; i <dataToCheck.Length; i++)
        {
            if (!Directory.Exists(GetDataPath(dataToCheck[i])))
            {
                Debug.LogError($"UltraTelephone_Data: missing folder {dataToCheck[i]}");
                return false;
            }
        }

        return true;
    }

    private static TelephonePersistentData data;
    public static bool AutoSave = true;

    public static TelephonePersistentData Data
    {
        get
        {
            if (data == null)
            {
                LoadData();
            }

            return data;
        }

        set
        {
            if (value != null)
            {
                if (data != value)
                {
                    data = value;
                    OnDataChanged?.Invoke(data);
                    if (AutoSave)
                    {
                        SaveData();
                    }
                    return;
                }
            }
        }
    }

    public static bool IsFirstTime()
    {
        if (!File.Exists(dataFilePath))
        {
            return true;
        }
        return false;
    }

    public static void SaveData()
    {
        string serializedData = JsonConvert.SerializeObject(Data);
        File.WriteAllText(dataFilePath, serializedData);
        Debug.Log($"UTel_Save: Game data saved to {dataFilePath}");
    }

    public static void LoadData()
    {
        Debug.Log("UTel_Save: Searching for datafile.");
        if (File.Exists(dataFilePath))
        {
            string jsonData;
            using (StreamReader reader = new StreamReader(dataFilePath))
            {
                jsonData = reader.ReadToEnd();
            }

            data = JsonConvert.DeserializeObject<TelephonePersistentData>(jsonData);
        }

        if(data == null)
        {
            NewData();
        }
    }

    public static void NewData()
    {
        Debug.Log("UltraTelephone: Creating new save-file.");
        Data = TelephonePersistentData.Default;
        SaveData();
    }

    public delegate void OnDataChangedHandler(TelephonePersistentData newData);
    public static OnDataChangedHandler OnDataChanged;
}

[System.Serializable]
public class TelephonePersistentData
{
    public bool coconutted;
    public bool launchedOnce;
    public int coconutMurders;

    public static TelephonePersistentData Default
    {
        get
        {
            TelephonePersistentData newData = new TelephonePersistentData
            {
                coconutted = false,
                launchedOnce = true,
                coconutMurders = 0,
            };

            return newData;
        }
    }
}
