using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Jumpscare : MonoBehaviour, IBruhMoment
{
    private void Awake()
    {
        BruhMoments.RegisterBruhMoment(this);
    }

    public void Execute()
    {
        //show cat image
    }

    public bool IsRunning()
    {
        throw new NotImplementedException();
    }

    public bool IsComplete()
    {
        //fade out cat image
        return false;
    }

    public void End()
    {
        //dont show cat image

    }


}