using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace UltraTelephone.Hydra
{
    public class PlayerClowner : MonoBehaviour
    {
        private void ClownOnPlayer()
        {

            switch (PickAction())
            {
                case PlayerClowner.Clown.OpenFunny:
                    if (UnityEngine.Random.value > 0.5f)
                    {
                        UltraTelephone.Temperz.Funnys.OpenFunny();
                    }
                    else
                    {
                        ClownOnPlayer();
                    }
                    break;
                case PlayerClowner.Clown.JumpScare:
                    Jumpscare.Scare();
                    break;
                case PlayerClowner.Clown.FreeBird:
                    BirdFreer.FreeBird();
                    break;
                case PlayerClowner.Clown.RandomAudio:
                    RandomSounds.PlayRandomSoundFromSubdirectory("random");
                    break;
                case PlayerClowner.Clown.ChuckNorris:
                    Hydra.ChuckNorrisFacts.Instance.Execute();
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

        public enum Clown { OpenFunny, JumpScare, RandomAudio, FreeBird, ChuckNorris }
    }
}

