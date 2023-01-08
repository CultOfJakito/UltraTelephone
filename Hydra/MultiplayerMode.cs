using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class MultiplayerMode : MonoBehaviour
{
    public Vector2 shortDelayMinMax = new Vector2(0.025f, 0.15f);
    public Vector2 medDelayMinMax = new Vector2(0.25f, 0.5f);
    public Vector2 longDelayMinMax = new Vector2(0.75f, 3.0f);
    public List<Vector3> cachedPositions = new List<Vector3>();

    public bool simulateLag;

    private float timeTillNextAction;
    private Transform playerTransform;

    private void Awake()
    {
        BestUtilityEverCreated.OnLevelChanged += OnLevelChanged;
    }

    private void OnLevelChanged(BestUtilityEverCreated.UltrakillLevelType ltype)
    {
        if(BestUtilityEverCreated.InLevel())
        {
            playerTransform = NewMovement.Instance.gameObject.transform;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            simulateLag = !simulateLag;
        }

        if (playerTransform == null || !simulateLag)
        {
            return;
        }

        if (simulateLag && timeTillNextAction <= 0.0f)
        {
            SimulateLag();
        }
            

        timeTillNextAction = Mathf.Clamp(timeTillNextAction - Time.deltaTime, 0.0f, Mathf.Infinity);
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
        if(playerTransform != null)
        {
            playerTransform.position = newPos;
        }
    }

    private void CachePos()
    {
        if(playerTransform != null)
        {
            cachedPositions.Add(playerTransform.position);
        }
    }
}

