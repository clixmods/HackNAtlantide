using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LoadingWorkerBehaviour : MonoBehaviour
{
    public static List<LoadingWorkerBehaviour> LoadingWorkerBehaviours = new List<LoadingWorkerBehaviour>();
    
    public abstract bool WorkIsDone
    {
        get;
        set;
    }
    
    protected virtual void Awake()
    {
        LoadingWorkerBehaviour.LoadingWorkerBehaviours.Add(this);
    }

    protected virtual void OnDestroy()
    {
        LoadingWorkerBehaviour.LoadingWorkerBehaviours.Remove(this);
    }
}
