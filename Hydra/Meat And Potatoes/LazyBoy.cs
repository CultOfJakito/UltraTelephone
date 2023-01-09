using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class LazyBoy
{ 
    private static GameObject chairPrefab;
    private static Transform levelSectionRoot;

    private static List<GameObject> placedChairs = new List<GameObject>();

    private static bool initialized = false;

    public static void Init()
    {
        if(!initialized)
        {
            initialized = true;
            if (HydraLoader.prefabRegistry.TryGetValue("VergilChair", out chairPrefab))
            {
                BestUtilityEverCreated.OnLevelChanged += _ => PlaceChair();
                SimpleLogger.Log("Chair force!");
            }
        }   
    }

    private static void PlaceChair()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        if(sceneName != "Level 4-4" || !BestUtilityEverCreated.InLevel())
        {
            return;
        }

        if(chairPrefab != null)
        {
            ObjectActivationCheck[] objActivators = Resources.FindObjectsOfTypeAll<ObjectActivationCheck>();

            for(int i=0; i<objActivators.Length; i++)
            {
                if(objActivators[i].gameObject.name == "7 - Boss Arena 1")
                {
                    levelSectionRoot = objActivators[i].gameObject.transform;
                    BeginPlacementSequence();
                    return;
                }
            }
        }
    }

    private static void BeginPlacementSequence()
    {
        if(levelSectionRoot == null)
        {
            return;
        }

        MeshRenderer[] meshRenderers = levelSectionRoot.GetComponentsInChildren<MeshRenderer>(true);

        for(int i=0; i < meshRenderers.Length; i++)
        {
            if(meshRenderers[i].gameObject.name == "Throne")
            {
                InjectSeat(meshRenderers[i]);
            }
        }
    }

    private static void InjectSeat(MeshRenderer throne)
    {
        throne.enabled = false;
        Transform throneHalf = throne.transform.GetChild(0);
        if(throneHalf != null)
        {
            if(throneHalf.TryGetComponent<MeshRenderer>(out MeshRenderer throneHalfMR))
            {
                throneHalfMR.enabled = false;
            }
        }

        GameObject newChair = GameObject.Instantiate<GameObject>(chairPrefab, throne.transform, false);
        newChair.transform.GetChild(0).localPosition = new Vector3(0.0f, -0.10f, -1.0f);
        placedChairs.Add(newChair);
    }

    public static void ResetChair()
    {
        int chairs = placedChairs.Count;

        for(int i = 0; i < chairs; i++)
        {
            GameObject.Destroy(placedChairs[i]);
        }

        placedChairs = new List<GameObject>();

        BeginPlacementSequence();
    }

}