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
        public static string clipFolder = Directory.GetCurrentDirectory() + "/BepInEx/config/ultratelephone/audio";
        private static bool initialized = false;
        private static List<AudioClip> allDeathClips = new List<AudioClip>();
        private static List<AudioClip> allHurtClips = new List<AudioClip>();

        public static IEnumerator Initialize(MonoBehaviour caller) // THANK YOU UKSR THE BEST MOD EVER MADE BY ME TEMPERZ87!!!!
        {
            Debug.Log("TRYING TO INITIALIZE THE AUDIOSWAPPER");
            if (initialized)
            {
                Debug.Log("SOME FUCKING CUNT CALLED THE INIT FUNCTION TWICE!");
                yield break;
            }

            DirectoryInfo info = new DirectoryInfo(clipFolder);
            if (!info.Exists)
            {
                Directory.CreateDirectory(info.FullName);
                Directory.CreateDirectory(info.FullName + "/death");
                Directory.CreateDirectory(info.FullName + "/hurt");
                yield break;
            }
            info = new DirectoryInfo(clipFolder + "/death");
            foreach (FileInfo file in info.GetFiles("*.wav", SearchOption.AllDirectories))
                caller.StartCoroutine(StartNewWWW(file.FullName, AudioType.WAV, allDeathClips));
            foreach (FileInfo file in info.GetFiles("*.mp3", SearchOption.AllDirectories))
                caller.StartCoroutine(StartNewWWW(file.FullName, AudioType.MPEG, allDeathClips));
            foreach (FileInfo file in info.GetFiles("*.ogg", SearchOption.AllDirectories))
                caller.StartCoroutine(StartNewWWW(file.FullName, AudioType.OGGVORBIS, allDeathClips));
            
            info = new DirectoryInfo(clipFolder + "/hurt");
            foreach (FileInfo file in info.GetFiles("*.wav", SearchOption.AllDirectories))
                caller.StartCoroutine(StartNewWWW(file.FullName, AudioType.WAV, allHurtClips));
            foreach (FileInfo file in info.GetFiles("*.mp3", SearchOption.AllDirectories))
                caller.StartCoroutine(StartNewWWW(file.FullName, AudioType.MPEG, allHurtClips));
            foreach (FileInfo file in info.GetFiles("*.ogg", SearchOption.AllDirectories))
                caller.StartCoroutine(StartNewWWW(file.FullName, AudioType.OGGVORBIS, allHurtClips));

            initialized = true;
        }

        private static IEnumerator StartNewWWW(string path, AudioType type, List<AudioClip> list)
        {
            Debug.Log("Trying to load clip at " + path);
            using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(path, type))
            {
                yield return www.SendWebRequest();

                if (www.isNetworkError)
                {
                    Debug.Log("couldn't load clip " + path);
                    Debug.Log(www.error);
                }
                else
                {
                    list.Add(DownloadHandlerAudioClip.GetContent(www));
                    Debug.Log("Loaded load clip at " + path);
                }
            }
        }

        public static void SwapDeathSource(AudioSource source) => SwapClipFromList(source, allDeathClips);
        public static void SwapHurtSource(AudioSource source) => SwapClipFromList(source, allHurtClips);

        private static void SwapClipFromList(AudioSource source, List<AudioClip> toUse)
        {
            if (!initialized)
            {
                Debug.Log("SOMEONES GRANDMA OF A COMPUTER COULDNT LOAD CLIPS FAST ENOUGH OR THE INITIALIZE FUNCTION OR AUDIOSWAPPER WASN'T CALLED!!!");
                return;
            }
            if (allDeathClips.Count <= 0)
            {
                Debug.Log("NO CLIPS TO SWAP DUMBASS");
                return;
            }
            Random rng = new Random();
            int selectedClip = rng.Next(0, toUse.Count - 1);
            source.clip = toUse[selectedClip];
        }
    }
}