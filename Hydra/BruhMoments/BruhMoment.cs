using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

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
        if(timeTillNextEvent <= 0.0f && BestUtilityEverCreated.InLevel())
        {
            timeTillNextEvent = UnityEngine.Random.Range(0.0f, maxTimeTillNextEvent);
            BruhMoment();
        }

        timeTillNextEvent -= Time.deltaTime;
    }

    public void BruhMoment()
    {
        currentBruhMoment = BruhMoments.ObtainBruhMoment();
        if(currentBruhMoment == null || currentBruhMoment.IsRunning())
        {
            return;
        }
        StartCoroutine(ExecuteBruhMoment(currentBruhMoment));
    }

    private IEnumerator ExecuteBruhMoment(IBruhMoment bruhMoment)
    {
        currentBruhMoment.Execute();
        SimpleLogger.Log($"Beginning execution of bruh moment: {currentBruhMoment.GetName()}");
        while(!currentBruhMoment.IsComplete() && currentBruhMoment.IsRunning())
        {
            yield return new WaitForEndOfFrame();
        }
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
        if(!initalized)
        {
            GameObject bruhContainer = new GameObject("Bruhmoments");
            bruhContainer.AddComponent<BruhMomentController>();
            bruhContainer.AddComponent<Jumpscare>();
            bruhContainer.AddComponent<Weirdener>();
            bruhContainer.AddComponent<MultiplayerMode>();
            initalized = true;
        }
    }

    public static bool RegisterBruhMoment(IBruhMoment bruhMoment)
    {
        if(bruhMoments.Contains(bruhMoment))
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
        for(int i=0; i< bruhMoments.Count; i++)
        {
            if(bruhMoments[i] != null)
            {
                bruhMoments[i].End();
            }
        }
    }
}

public interface IBruhMoment
{
    /// <summary>
    /// Called to start the bruh moment
    /// </summary>
    void Execute();

    /// <summary>
    /// Called every frame the bruh moment is running you must return if your bruh moment is complete or not
    /// </summary>
    /// <returns></returns>
    bool IsComplete();
    
    /// <summary>
    /// Called if bruh moment is running, similar to complete
    /// </summary>
    /// <returns></returns>
    bool IsRunning();

    /// <summary>
    /// Force stop of bruh moment, use this to cancel any coroutines or stop execution of logic
    /// </summary>
    void End();

    /// <summary>
    /// String name of your bruh moment
    /// </summary>
    /// <returns></returns>
    string GetName();
}