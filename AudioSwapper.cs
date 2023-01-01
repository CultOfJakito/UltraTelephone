using System;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using Random = System.Random;

namespace UltraTelephone
{
    public static class AudioSwapper
    {
        public static string clipFolder = Directory.GetCurrentDirectory() + "\\BepInEx\\config\\ultratelephone\\audio\\";
        
        public static AudioSource SwapClipWithFile(AudioSource source)
        {
            AudioClip sourceClip = source.clip;
            
            Random rng = new Random();
            int selectedClip = rng.Next(1,9);
            
            string filePath = "file://" + clipFolder + "laugh" + selectedClip.ToString() + ".mp3";

            UnityWebRequest fileRequest = UnityWebRequestMultimedia.GetAudioClip(filePath,AudioType.MPEG);
            fileRequest.SendWebRequest();
            try
            {
                while (!fileRequest.isDone) {}
 
                if (fileRequest.isNetworkError || fileRequest.isHttpError) {Console.WriteLine("Audio swap failed");}
                else
                {
                    Console.WriteLine("5");
                    source.clip = DownloadHandlerAudioClip.GetContent(fileRequest);
                }
            }
            catch (Exception err)
            {
            
            }
            return source;
        }
    }
}