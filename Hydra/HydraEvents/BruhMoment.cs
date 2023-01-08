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

    }

    private void Update()
    {
        if(timeTillNextEvent <= 0.0f)
        {
            timeTillNextEvent = UnityEngine.Random.Range(0.0f, maxTimeTillNextEvent);
            BruhMoment();
        }

        timeTillNextEvent -= Time.deltaTime;
    }

    public void BruhMoment()
    {

        currentBruhMoment = BruhMoments.ObtainBruhMoment();
        if(currentBruhMoment.IsComplete())
        {

        }
        StartCoroutine(ExecuteBruhMoment(currentBruhMoment));
    }

    private IEnumerator ExecuteBruhMoment(IBruhMoment bruhMoment)
    {
        currentBruhMoment.Execute();
        while(!currentBruhMoment.IsComplete() && currentBruhMoment.IsRunning())
        {
            yield return new WaitForEndOfFrame();
        }
        currentBruhMoment.End();
    }

}

public static class BruhMoments
{
    private static List<IBruhMoment> bruhMoments = new List<IBruhMoment>();

    public static bool RegisterBruhMoment(IBruhMoment bruhMoment)
    {
        if(bruhMoments.Contains(bruhMoment))
        {
            return false;
        }

        bruhMoments.Add(bruhMoment);
        return true;
    }

    public static IBruhMoment ObtainBruhMoment()
    {
        int rand = UnityEngine.Random.Range(0, bruhMoments.Count);
        return bruhMoments[rand];
    }
}

public interface IBruhMoment
{
    void Execute();
    bool IsComplete();
    bool IsRunning();
    void End();
}