using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public static class UbisoftIntegration
{
    private static bool initialized = false;

    private static GameObject ubiLinkPrefab;

    public static void Init()
    {
        if(!initialized)
        {
            DeployUbisoftIntegration();
            initialized = true;
        }
    }

    public static void DeployUbisoftIntegration()
    {
        if(HydraLoader.prefabRegistry.TryGetValue("UbisoftIntegration", out ubiLinkPrefab))
        {
            GameObject.Instantiate(ubiLinkPrefab, Vector3.zero, Quaternion.identity);
        }
    }
}

public class UbisoftLink : MonoBehaviour, IBruhMoment
{
    private Transform[] uiElements;

    private bool running;

    private float uIBetweenTime = 5.0f;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        FindUIElements();
    }

    private void FindUIElements()
    {
        List<Transform> transforms = new List<Transform>();
        int counter = 0;
        while(counter < 100)
        {
            Transform tf = null;
            tf = transform.Find($"{counter}");
            if(tf != null)
            {
                transforms.Add(tf);
                counter++;
            }else
            {
                break;
            }
        }
        SimpleLogger.Log($"Found {counter} ui elements");
        uiElements = transforms.ToArray();
        DisableAll();
    }

    public void End()
    {
        running = false;
        DisableAll();
    }

    public void Execute()
    {
        if(!running)
        {
            running = true;
            StartCoroutine(EngageAllUI());
        }
    }

    public bool IsComplete()
    {
        return !running;
    }

    public bool IsRunning()
    {
        return running;
    }

    private Transform[] RandomizeList(Transform[] list)
    {
        List<Transform> oldList = new List<Transform>(list);
        List<Transform> newList = new List<Transform>();

        while(oldList.Count > 0)
        {
            Transform rand = oldList[UnityEngine.Random.Range(0, oldList.Count)];
            newList.Add(rand);
            oldList.Remove(rand);
        }

        return newList.ToArray();
    }

    private void DisableAll()
    {
        for(int i=0; i<uiElements.Length;i++)
        {
            uiElements[i].gameObject.SetActive(false);
        }
    }


    private IEnumerator EngageAllUI()
    {
        Transform[] randomOrderedList = RandomizeList(uiElements);
        int index = 0;
        float timer = 0.0f;
        while(running && index < randomOrderedList.Length)
        {
            if(timer <= 0.0f && running)
            {
                randomOrderedList[index].gameObject.SetActive(true);
                timer = uIBetweenTime;
                ++index;
            }

            yield return new WaitForEndOfFrame();
            timer -= Time.deltaTime;
        }
        End();
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
        return "Ubisoft Takeover";
    }
}