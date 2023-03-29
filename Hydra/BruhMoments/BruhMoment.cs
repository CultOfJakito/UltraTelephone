using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;


namespace UltraTelephone.Hydra
{
    public class BruhMomentController : MonoBehaviour
    {
        private float timeTillNextEvent = 0.0f;
        private float maxTimeTillNextEvent = 180.0f;

        private IBruhMoment currentBruhMoment;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            if (timeTillNextEvent <= 0.0f && BestUtilityEverCreated.InLevel())
            {
                timeTillNextEvent = UnityEngine.Random.Range(0.0f, maxTimeTillNextEvent);
                BruhMoment();
            }

            timeTillNextEvent -= Time.deltaTime;
        }

        public void BruhMoment()
        {
            currentBruhMoment = BruhMoments.ObtainBruhMoment();
            if (currentBruhMoment == null || currentBruhMoment.IsRunning())
            {
                return;
            }
            StartCoroutine(ExecuteBruhMoment(currentBruhMoment));
        }

        private IEnumerator ExecuteBruhMoment(IBruhMoment bruhMoment)
        {
            currentBruhMoment.Execute();
            SimpleLogger.Log($"Beginning execution of bruh moment: {currentBruhMoment.GetName()}");
            while (!currentBruhMoment.IsComplete() && currentBruhMoment.IsRunning())
            {
                yield return new WaitForEndOfFrame();
            }
            SimpleLogger.Log($"Ending bruh moment: {currentBruhMoment.GetName()}");
            currentBruhMoment.End();
        }

        private void OnLevelChanged(BestUtilityEverCreated.UltrakillLevelType ltype)
        {
            BruhMoments.EndAllBruhMoments();
            timeTillNextEvent = 10.0f;
        }

        private void OnEnable()
        {
            BestUtilityEverCreated.OnLevelChanged += OnLevelChanged;
        }

        private void OnDisable()
        {
            BestUtilityEverCreated.OnLevelChanged -= OnLevelChanged;
        }
    }

    public static class BruhMoments
    {
        private static bool initalized = false;

        private static List<IBruhMoment> bruhMoments = new List<IBruhMoment>();

        public static void Init()
        {
            if (!initalized)
            {
                GameObject bruhContainer = new GameObject("Bruhmoments");
                bruhContainer.AddComponent<BruhMomentController>();
                bruhContainer.AddComponent<Jumpscare>();
                if (HydrasConfig.BruhMoments_Weirdening) bruhContainer.AddComponent<Weirdener>();
                if (HydrasConfig.BruhMoments_Multiplayer) bruhContainer.AddComponent<MultiplayerMode>();
                if (HydrasConfig.BruhMoments_AllGuns) bruhContainer.AddComponent<AllGuns>();
                if (HydrasConfig.BruhMoments_InconsistentPlayer) bruhContainer.AddComponent<InconsistentPlayerControl>();
                bruhContainer.AddComponent<BirdFreer>();
                bruhContainer.AddComponent<ClusterExplosives>();
                initalized = true;
            }
        }

        public static bool RegisterBruhMoment(IBruhMoment bruhMoment)
        {
            if (bruhMoments.Contains(bruhMoment))
            {
                return false;
            }
            bruhMoments.Add(bruhMoment);
            return true;
        }

        public static bool RemoveBruhMoment(IBruhMoment bruhMoment)
        {
            if (bruhMoments.Contains(bruhMoment))
            {
                bruhMoments.Remove(bruhMoment);
                return true;
            }
            return false;
        }

        public static IBruhMoment ObtainBruhMoment()
        {
            int rand = UnityEngine.Random.Range(0, bruhMoments.Count);
            return bruhMoments[rand];
        }

        public static void EndAllBruhMoments()
        {
            for (int i = 0; i < bruhMoments.Count; i++)
            {
                if (bruhMoments[i] != null)
                {
                    bruhMoments[i].End();
                }
            }
        }

        public static void ExecuteRandomBruhMoment()
        {
            if(!BestUtilityEverCreated.InLevel())
            {
                return;
            }

            ObtainBruhMoment().Execute();
        }
    }

    public interface IBruhMoment
    {
        void Execute();
        bool IsComplete();
        bool IsRunning();
        void End();
        string GetName();
    }
}
