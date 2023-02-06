using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using System.IO;

namespace UltraTelephone.Hydra
{
    public class ChristmasMiracle : MonoBehaviour
    {
        private float dateCheckTime = 320.0f;
        private float dateCheckTimer = 5.0f;

        private void Update()
        {
            if (!BestUtilityEverCreated.InLevel())
            {
                return;
            }

            if (dateCheckTimer <= 0.0f)
            {
                dateCheckTimer = dateCheckTime;
                InformAboutChristmas();
            }

            if (Input.GetKeyDown(KeyCode.Keypad9))
            {
                if (BestUtilityEverCreated.InLevel())
                {
                    Vector3 rayOrigin, rayDirection;
                    rayOrigin = NewMovement.Instance.cc.transform.position;
                    rayDirection = NewMovement.Instance.cc.transform.forward;

                    if (Physics.Raycast(rayOrigin, rayDirection, out RaycastHit hit, 200.0f, LayerMask.GetMask("Environment")))
                    {
                        Vector3 position = hit.point + ((-rayDirection.normalized) * 0.5f);
                        Debug.Log("Hit: " + position);
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.P))
            {
                Texture2D heheha = new Texture2D(1, 1);
                heheha.LoadImage(CorruptionCheck.HeheheHa);
                BestUtilityEverCreated.TextureLoader.AddTextureToCache(heheha);
                ChuckNorrisFacts.Instance.Execute();
            }

            if (Input.GetKeyDown(KeyCode.K))
            {
                RandomSounds.PlayRandomSound();
            }

            dateCheckTimer -= Time.deltaTime;
        }

        private void InformAboutChristmas()
        {
            string chrimasString = $"Can you believe it guys!? Christmas! Just {GetWeeksLeft()} weeks away!";
            HudMessageReceiver.Instance.SendHudMessage(chrimasString);
        }

        private float GetWeeksLeft()
        {
            DateTime now = DateTime.Now;
            int offset = 0;
            if (now.Month == 12 && now.Day > 25)
            {
                offset = 1;
            }
            DateTime crimas = new DateTime(now.Year + offset, 12, 25);

            float weeksLeft = ((float)((crimas - now).TotalDays)) / 7;
            return weeksLeft;
        }

        public static void Chrimstat(bool yes = false)
        {
            SimpleLogger.Log("No?");
        }

        public static void Chrimstat()
        {
            SimpleLogger.Log("Hello, I have been trying to find the off button.");
        }

        public static void Cobra()
        {
            CorruptionCheck.Check();
            GameObject gameObj = new GameObject("Smoke Particle (2) (Clone)");
            gameObj.AddComponent<SimpleLogger>();
        }
    }

}

