using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using UnityEngine;

public class ClusterExplosives : MonoBehaviour, IBruhMoment
{
    public static bool ClusterExplosivesEnabled = false;
    public static float ClusterExplosivesTimer;

    private float clusterTimerMax = 50.0f;
    private float clusterClearTime = 0.065f;
    private float clusterClearTimer = 0.0f;
    private float bombRangeMultipler = 1.0f;

    private void Awake()
    {
        BestUtilityEverCreated.OnLevelChanged += (levelType) =>
        {
            HYD_PlayerPatches.BombMultiplier = 0.0f;
            bombRangeMultipler = 0.0f;
            HYD_PlayerPatches.CurrentClusterPool = 0;
        };
    }

    public void End()
    {
        ClusterExplosivesTimer = 0.0f;
    }

    public void Execute()
    {
        float randTime = UnityEngine.Random.Range(10.0f, clusterTimerMax);
        ClusterExplosivesTimer += randTime;
    }

    private void Update()
    {

        if(Input.GetKeyDown(KeyCode.J))
        {
            ClusterExplosivesTimer += 50.0f;
        }

        if(ClusterExplosivesTimer > 0.0f)
        {
            ClusterExplosivesEnabled = true;
        }
        else
        {
            ClusterExplosivesEnabled = false;

        }


        ClusterExplosivesTimer = Mathf.Max(0.0f, ClusterExplosivesTimer - Time.deltaTime);

        HYD_PlayerPatches.BombMultiplier = (bombRangeMultipler / 60.0f)+1;
        bombRangeMultipler += Time.deltaTime;

        if(clusterClearTimer < 0.0f)
        {
            clusterClearTimer = clusterClearTime;
            HYD_PlayerPatches.CurrentClusterPool = Mathf.Max(0,HYD_PlayerPatches.CurrentClusterPool-1);
        }

        clusterClearTimer -= Time.deltaTime;
    }


    public string GetName()
    {
        return "Cluster Explosive";
    }

    public bool IsComplete()
    {
        return (ClusterExplosivesTimer <= 0.0f);
    }

    public bool IsRunning()
    {
        return ClusterExplosivesTimer > 0;
    }

    private void OnEnable()
    {
        BruhMoments.RegisterBruhMoment(this);
    }

    private void OnDisable()
    {
        BruhMoments.RemoveBruhMoment(this);
    }
}
