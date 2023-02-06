using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace UltraTelephone.Hydra
{
    public class MultiplayerMode : MonoBehaviour, IBruhMoment
    {
        private GameObject netOutPrefab;

        public Vector2 shortDelayMinMax = new Vector2(0.025f, 0.15f);
        public Vector2 medDelayMinMax = new Vector2(0.25f, 0.5f);
        public Vector2 longDelayMinMax = new Vector2(0.75f, 3.0f);
        public List<Vector3> cachedPositions = new List<Vector3>();

        public bool simulateLag;

        private float bruhTime = 25.0f;
        private float currentBruhTime = 0.0f;

        private float timeTillNextAction;
        private Transform playerTransform;

        private GameObject netOutObj;
        private Transform uiTransform;

        private bool blindPreviously = false;

        private float hardTime = 25.0f;
        private float hardTimer = 0.0f;

        private void Awake()
        {
            HydraLoader.prefabRegistry.TryGetValue("Lagometer", out netOutPrefab);
            BestUtilityEverCreated.OnLevelChanged += OnLevelChanged;
        }

        private void SpawnUI()
        {
            if (netOutPrefab != null)
            {
                netOutObj = GameObject.Instantiate<GameObject>(netOutPrefab, Vector3.zero, Quaternion.identity);
                uiTransform = netOutObj.transform.GetChild(0);
            }
        }

        private void OnLevelChanged(BestUtilityEverCreated.UltrakillLevelType ltype)
        {
            if (BestUtilityEverCreated.InLevel())
            {
                playerTransform = NewMovement.Instance.gameObject.transform;
                SpawnUI();
            }
        }

        private void SetUIState(bool enabled)
        {
            if (uiTransform != null)
            {
                uiTransform.gameObject.SetActive(enabled);
            }
        }

        private void Update()
        {
            SetUIState(simulateLag);

            if (playerTransform == null || !simulateLag)
            {
                return;
            }

            if (simulateLag && timeTillNextAction <= 0.0f)
            {
                SimulateLag();
            }

            timeTillNextAction = Mathf.Clamp(timeTillNextAction - Time.deltaTime, 0.0f, Mathf.Infinity);
            currentBruhTime -= Time.deltaTime;
            hardTimer -= Time.deltaTime;

            if (simulateLag)
            {
                if (hardTimer < 0.0f)
                {
                    End();
                }
            }
        }

        private void SimulateLag()
        {

            float lagDelayLength = UnityEngine.Random.Range(0, 100);
            float nextLagDelay = 0.0f;

            if (lagDelayLength < 20.0f)
            {
                nextLagDelay = UnityEngine.Random.Range(shortDelayMinMax.x, shortDelayMinMax.y);
            }
            else if (lagDelayLength < 80.0f)
            {
                nextLagDelay = UnityEngine.Random.Range(medDelayMinMax.x, medDelayMinMax.y);
            }
            else if (lagDelayLength < 100.0f)
            {
                nextLagDelay = UnityEngine.Random.Range(longDelayMinMax.x, longDelayMinMax.y);
            }

            timeTillNextAction += nextLagDelay;

            float randAction = UnityEngine.Random.Range(0, 100);

            if (randAction < 65.0f && (cachedPositions.Count > 0))
            {
                RestoreRand();
            }
            else if (randAction < 100.0f)
            {
                CachePos();
            }
        }

        private void RestoreRand()
        {

            int rand = UnityEngine.Random.Range(0, cachedPositions.Count);
            Vector3 newPos = cachedPositions[rand];
            cachedPositions.Remove(cachedPositions[rand]);

            while (cachedPositions.Count > 0)
            {
                if (!ResolvePosition(newPos))
                {
                    rand = UnityEngine.Random.Range(0, cachedPositions.Count);
                    newPos = cachedPositions[rand];
                    cachedPositions.Remove(cachedPositions[rand]);
                }
                else
                {
                    if (playerTransform != null)
                    {
                        playerTransform.position = newPos;
                        break;
                    }
                }

            }
        }

        private bool ResolvePosition(Vector3 positionToCheck)
        {
            RaycastHit[] hits = Physics.RaycastAll(positionToCheck, Vector3.down, 25.0f, LayerMask.GetMask("Default", "Environment", "Outdoors"));

            int hitCount = 0;

            if (hits.Length > 0)
            {
                for (int i = 0; i < hits.Length; i++)
                {
                    if (!hits[i].collider.isTrigger)
                    {
                        ++hitCount;
                    }
                }
            }

            return hitCount > 0;

        }

        private void CachePos()
        {
            if (playerTransform != null)
            {
                cachedPositions.Add(playerTransform.position);
            }
        }

        public void Execute()
        {
            currentBruhTime = bruhTime;
            hardTimer = hardTime;
            simulateLag = true;
        }

        public bool IsComplete()
        {
            return currentBruhTime <= 0.0f;
        }

        public bool IsRunning()
        {
            return simulateLag;
        }

        public void End()
        {
            simulateLag = false;
            currentBruhTime = 0.0f;
        }

        private void OnEnable()
        {
            BruhMoments.RegisterBruhMoment(this);
        }

        private void OnDisable()
        {
            BruhMoments.RemoveBruhMoment(this);
        }

        public string GetName()
        {
            return "Lag";
        }
    }


}

