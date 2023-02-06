using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using UnityEngine.Networking;
using Newtonsoft.Json;
using UnityEngine;

namespace UltraTelephone.Hydra
{
    public class ChuckNorrisFacts : IBruhMoment
    {
        private static ChuckNorrisFacts _instance;

        private string[] facts = new string[0];

        public static ChuckNorrisFacts Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ChuckNorrisFacts();
                }

                return _instance;
            }
        }

        public ChuckNorrisFacts()
        {
            Plugin.plugin.StartCoroutine(TryGetFacts());
            BruhMoments.RegisterBruhMoment(this);
        }

        private IEnumerator TryGetFacts()
        {
            float factDelay = 0.05f;
            int factCount = 10;


            SimpleLogger.Log("Getting chuck norris facts");

            for(int i = 0; i < factCount; i++)
            {
                using (UnityWebRequest www = UnityWebRequest.Get("https://api.chucknorris.io/jokes/random"))
                {
                    yield return www.SendWebRequest();

                    if (www.isNetworkError || www.isHttpError)
                    {
                        SimpleLogger.Log("Couldn't get fact :(");
                        break;
                    }
                    else
                    {
                        // Show results as text
                        string json = www.downloadHandler.text;

                        ChuckNorrisFact newFact = (ChuckNorrisFact)JsonConvert.DeserializeObject<ChuckNorrisFact>(json);

                        SimpleLogger.Log("Chucj noris ");

                        if(newFact != null)
                        {
                            List<string> newFacts = new List<string>(facts);
                            newFacts.Add(newFact.value);
                            facts = newFacts.ToArray();
                        }                      
                    }
                }

                yield return new WaitForSeconds(factDelay);
            }
        }

        public void Init()
        {
            SimpleLogger.Log(Instance.GetName());
        }

        public void End()
        {

        }

        public void Execute()
        {
            if (facts.Length > 0 && HydrasConfig.BruhMoments_ChuckNorris)
            {
                string fact = facts[UnityEngine.Random.Range(0, facts.Length)];
                SimpleLogger.Log(fact);
                HudMessageReceiver.Instance.SendHudMessage(fact);
                RandomSounds.PlayRandomSound();
            }
        }

        public string GetName()
        {
            return "Chuck norris fact";
        }

        public bool IsComplete()
        {
            return true;
        }

        public bool IsRunning()
        {
            if(HydrasConfig.BruhMoments_ChuckNorris)
            {
                return true;
            }
            return false;
        }

        [System.Serializable]
        public class ChuckNorrisFact
        {
            public string[] categories;
            public string icon_url;
            public string id;
            public string updated_at;
            public string url;
            public string value;
        }
    }
}