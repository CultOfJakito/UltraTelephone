using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    public float timeLeft = 15.0f;

    private bool dying;

    private void Start()
    {
        KillAfterTime();
    }

    public void KillAfterTime()
    {
        if(!dying)
        {
            dying = true;
            StartCoroutine(KillTimer());
        }
    }

    private IEnumerator KillTimer()
    {
        yield return new WaitForSeconds(1);
        while(timeLeft > 0.0f)
        {
            yield return new WaitForEndOfFrame();
            timeLeft -= Time.deltaTime;
        }
    }

    private void Kill()
    {
        Destroy(gameObject);
    }

}
