using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class PlayerClowner : MonoBehaviour
{
    private void ClownOnPlayer()
    {
       switch(PickAction())
        {
            case PlayerClowner.Clown.OpenFunny:
                UltraTelephone.Temperz.Funnys.OpenFunny();
                break;
            case PlayerClowner.Clown.JumpScare:
                Jumpscare.Scare();
                break;
        }
    }

    private Clown PickAction()
    {
        Array values = Enum.GetValues(typeof(Clown));
        System.Random rand = new System.Random();
        Clown randomClown = (Clown)values.GetValue(rand.Next(values.Length));
        return randomClown;
    }

    private void OnEnable()
    {
        BestUtilityEverCreated.OnPlayerDied += ClownOnPlayer;
    }

    private void OnDisable()
    {
        BestUtilityEverCreated.OnPlayerDied -= ClownOnPlayer;
    }

    public enum Clown { OpenFunny, JumpScare, RandomAudio }
}