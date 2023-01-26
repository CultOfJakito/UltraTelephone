using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public static class RandomSounds
{
    public static void PlayRandomSound()
    {
        PlaySound(UltraTelephone.AudioSwapper.GetRandomAudioClip());
    }

    public static void PlayRandomSoundFromSubdirectory(string subdirName)
    {
        if(UltraTelephone.AudioSwapper.TryGetAudioClipFromSubdirectory(subdirName, out AudioClip clip))
        {
            PlaySound(clip);
        }
    }

    public static void PlayRandomSoundFromSubdirectory(string subdirName, string soundName)
    {
        if (UltraTelephone.AudioSwapper.TryGetAudioClipFromSubdirectory(subdirName, soundName, out AudioClip clip))
        {
            PlaySound(clip);
        }
    }

    public static void PlaySound(AudioClip clip)
    {
        Vector3 pos = Vector3.zero;

        if (BestUtilityEverCreated.InLevel())
        {
            pos = NewMovement.Instance.transform.position;
        }
        else
        {
            pos = CameraController.Instance.transform.position;
        }

        AudioSource.PlayClipAtPoint(clip, pos);
    }
}