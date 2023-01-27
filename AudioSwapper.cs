using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using Random = System.Random;

namespace UltraTelephone
{
    public static class AudioSwapper
    {
        public static string clipFolder = TelephoneData.GetDataPath("audio");
        private static bool initialized = false;

        private static Dictionary<string, List<AudioClip>> clipDB = new Dictionary<string, List<AudioClip>>();

        private static MonoBehaviour caller;

        public static IEnumerator Initialize(MonoBehaviour caller) // THANK YOU UKSR THE BEST MOD EVER MADE BY ME TEMPERZ87!!!! fify -Hydra :)
        {
            AudioSwapper.caller = caller;
            SimpleLogger.Log("TRYING TO INITIALIZE THE AUDIOSWAPPER");
            if (initialized)
            {
                Debug.Log("SOME FUCKING CUNT CALLED THE INIT FUNCTION TWICE! It was me :)");
                yield break;
            }

            LoadAllDetectedClips();
            initialized = true;
        }

        private static void LoadAllDetectedClips()
        {
            DirectoryInfo info = new DirectoryInfo(clipFolder);
            DirectoryInfo[] subdirectories = info.GetDirectories();
            foreach(DirectoryInfo directoryInfo in subdirectories)
            {
                TryLoadClipsFromSubdirectory(directoryInfo.Name);
            }
        }

        private static bool TryLoadClipsFromSubdirectory(string subdirectoryName)
        {
            DirectoryInfo info = new DirectoryInfo(Path.Combine(clipFolder, subdirectoryName));

            if (!info.Exists)
            {
                Debug.Log($"Created new directory {subdirectoryName} for audio clips.");
                info.Create();
                return false;
            }

            foreach (FileInfo file in info.GetFiles("*.wav", SearchOption.TopDirectoryOnly))
                caller.StartCoroutine(StartNewWWW(file.FullName, AudioType.WAV, subdirectoryName));
            foreach (FileInfo file in info.GetFiles("*.mp3", SearchOption.TopDirectoryOnly))
                caller.StartCoroutine(StartNewWWW(file.FullName, AudioType.MPEG, subdirectoryName));
            foreach (FileInfo file in info.GetFiles("*.ogg", SearchOption.TopDirectoryOnly))
                caller.StartCoroutine(StartNewWWW(file.FullName, AudioType.OGGVORBIS, subdirectoryName));

            if (!clipDB.ContainsKey(subdirectoryName))
            {
                return false;
            }

            if (!(clipDB[subdirectoryName].Count > 0))
            {
                return false;
            }

            return true;
        }

        private static IEnumerator StartNewWWW(string path, AudioType type, string subDirectoryName)
        {
            using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(path, type))
            {
                yield return www.SendWebRequest();

                if (www.isNetworkError)
                {
                    Debug.Log($"Failed to load {Path.GetFileName(path)} in subdirectory {subDirectoryName}");
                    Debug.Log(www.error);
                }
                else
                {                  
                    AudioClip newAudioClip = DownloadHandlerAudioClip.GetContent(www);
                    if (newAudioClip != null)
                    {
                        if (!clipDB.ContainsKey(subDirectoryName))
                        {
                            clipDB.Add(subDirectoryName, new List<AudioClip>());
                        }

                        if (!clipDB[subDirectoryName].Contains(newAudioClip))
                        {
                            clipDB[subDirectoryName].Add(newAudioClip);
                        }
                    }
                }
            }
        }

        public static AudioClip GetRandomAudioClip()
        {
            AudioClip fallbackClip = null;

            int randomDir = UnityEngine.Random.Range(0,clipDB.Count);

            int counter = 0;
            foreach (KeyValuePair<string, List<AudioClip>> keyPair in clipDB)
            {
                if(keyPair.Value[0] != null)
                {
                    fallbackClip = keyPair.Value[0];
                }

                if (randomDir == counter)
                {
                    int randIndex = UnityEngine.Random.Range(0, keyPair.Value.Count);
                    if (keyPair.Value[randIndex] != null)
                    {
                        return keyPair.Value[randIndex];
                    }
                }
                ++counter;
            }

            return fallbackClip;
        }

        public static AudioClip GetRandomAudioClipFromSubdirectory(string directoryName)
        {
            AudioClip newClip = null;

            Random rng = new Random();
            int selectedClip = rng.Next(0, clipDB[directoryName].Count);
            newClip = clipDB[directoryName][selectedClip];

            return newClip;
        }

        public static bool TryGetAudioClipFromSubdirectory(string directoryName, string clipName, out AudioClip clip)
        {
            clip = null;

            if (!clipDB.ContainsKey(directoryName))
            {
                if (TryLoadClipsFromSubdirectory(directoryName))
                {
                    Debug.Log($"Found {clipDB[directoryName].Count} clips within {directoryName}");
                }
                else
                {
                    return false;
                }
            }

            for (int i = 0; i < clipDB[directoryName].Count; i++)
            {
                if (clipDB[directoryName][i] != null)
                {
                    if (clipDB[directoryName][i].GetName() == clipName)
                    {
                        clip = clipDB[directoryName][i];
                        return true;
                    }
                }
                else
                {
                    clipDB[directoryName].RemoveAt(i);
                }
            }

            return false;
        }

        public static bool TryGetAudioClipFromSubdirectory(string directoryName, out AudioClip clip)
        {
            clip = null;

            bool clipsPresent = false;

            if (!clipDB.ContainsKey(directoryName))
            {
                if (TryLoadClipsFromSubdirectory(directoryName))
                {
                    clipsPresent = true;
                    Debug.Log($"Found {clipDB[directoryName].Count} clips within {directoryName}");
                }else
                {
                    return false;
                }
            }else
            {
                clipsPresent = true;
            }

            if (clipsPresent)
            {
                clip = GetRandomAudioClipFromSubdirectory(directoryName);
            }

            return (clip != null);
        }

        public static bool TryGetAudioClipByName(string name, out AudioClip clip)
        {
            clip = null;
            foreach(KeyValuePair<string, List<AudioClip>> keyPair in clipDB)
            {
                for(int i=0;i<keyPair.Value.Count; i++)
                {
                    if(keyPair.Value[i].GetName() == name)
                    {
                        clip = keyPair.Value[i];
                        return true;
                    }
                }
            }
            return false;
        }

        public static void SwapAudioClipSource(AudioSource source, string listKey)
        {
            if(TryGetAudioClipFromSubdirectory(listKey, out AudioClip newClip))
            {
                SwapClipFromList(source, clipDB[listKey]);
            }
        }

        private static void SwapClipFromList(AudioSource source, List<AudioClip> toUse)
        {
            if (!initialized)
            {
                SimpleLogger.Log("SOMEONES GRANDMA OF A COMPUTER COULDNT LOAD CLIPS FAST ENOUGH OR THE INITIALIZE FUNCTION OR AUDIOSWAPPER WASN'T CALLED!!!");
                return;
            }

            Random rng = new Random();
            int selectedClip = rng.Next(0, toUse.Count - 1);
            source.clip = toUse[selectedClip];
        }
    }
}