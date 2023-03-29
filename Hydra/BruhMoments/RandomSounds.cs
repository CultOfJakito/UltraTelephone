using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace UltraTelephone.Hydra
{
    public static class RandomSounds
    {
        public static void PlayRandomSound()
        {
            if(!HydrasConfig.BruhMoments_RandomSound)
            {
                return;    
            }

            if (AudioSwapper.TryGetRandomAudioClip(out AudioClip clipToPlay))
            {
                PlaySound(clipToPlay);
            }
        }

        public static void PlayRandomSoundFromSubdirectory(string subdirName)
        {
            if (AudioSwapper.TryGetAudioClipFromSubdirectory(subdirName, out AudioClip clip))
            {
                PlaySound(clip);
            }
        }

        public static void PlaySoundFromSubdirectory(string subdirName, string soundName)
        {
            if (AudioSwapper.TryGetAudioClipFromSubdirectory(subdirName, soundName, out AudioClip clip))
            {
                PlaySound(clip);
            }
        }

        public static void PlaySoundAtPlayer(AudioClip clip)
        {
            if (clip == null)
            {
                Debug.LogError("Invalid clip passed to RandomSounds!");
                return;
            }

            Vector3 pos = Vector3.zero;

            if (BestUtilityEverCreated.InLevel())
            {
                pos = CameraController.Instance.transform.position;
                PlaySoundAtPoint(clip, pos);
            }
        }

        public static void PlaySound(AudioClip clip)
        {
            if(clip == null)
            {
                Debug.LogError("Invalid clip passed to RandomSounds!");
                return;
            }

            GameObject newAudioSource = new GameObject($"TelephoneAudio ({clip.name})");
            AudioSource src = newAudioSource.AddComponent<AudioSource>();
            newAudioSource.AddComponent<DestroyAfterTime>().timeLeft = 10.0f;
            src.playOnAwake = false;
            src.clip = clip;
            src.spatialBlend = 0.0f;
            src.Play();
        }

        public static void PlaySoundAtPoint(AudioClip clip, Vector3 position)
        {
            if (clip == null)
            {
                Debug.LogError("Invalid clip passed to RandomSounds!");
                return;
            }

            AudioSource.PlayClipAtPoint(clip, position);
        }
    }
}

