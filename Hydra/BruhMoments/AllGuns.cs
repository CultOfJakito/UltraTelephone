using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace UltraTelephone.Hydra
{
    public class AllGuns : MonoBehaviour, IBruhMoment
    {
        public static bool AllGunsEnabled { get; private set; } = false;

        private float length = 20.0f;
        private float timer = 0.0f;

        private void Awake()
        {
            BruhMoments.RegisterBruhMoment(this);
        }

        private void Update()
        {
            if(AllGunsEnabled)
            {
                timer -= Time.deltaTime;
            }
        }

        public void End()
        {
            AllGunsEnabled = false;
        }

        public void Execute()
        {
            if(!BestUtilityEverCreated.InLevel())
            {
                return;
            }

            timer = length;
            AllGunsEnabled = true;

            for (int i=0;i< GunControl.Instance.allWeapons.Count; i++)
            {
                GunControl.Instance.allWeapons[i].SetActive(true);
            }    
        }

        public string GetName()
        {
            return "All Guns";
        }

        public bool IsComplete()
        {
            return (timer <= 0.0f);
        }

        public bool IsRunning()
        {
            return AllGunsEnabled;
        }
    }
}
