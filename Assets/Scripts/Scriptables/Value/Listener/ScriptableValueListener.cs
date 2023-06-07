using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScriptableValueListener<T> : MonoBehaviour 
{
    [SerializeField] protected ScriptableValue<T> _scriptableEvent;
    public UnityEvent<T> LaunchEvent;
    [SerializeField] private bool launchEventOnAwake;
    [SerializeField] private bool launchEventOnStart;
    protected virtual void Awake()
    {
        _scriptableEvent.OnValueChanged += LaunchScriptableValueEvent;
        if (launchEventOnAwake)
        {
            LaunchScriptableValueEvent(_scriptableEvent.Value);
        }
    }

    private void Start()
    {
        if (launchEventOnStart)
        {
            LaunchScriptableValueEvent(_scriptableEvent.Value);
        }
    }

    private void OnDestroy()
    {
        _scriptableEvent.OnValueChanged -= LaunchScriptableValueEvent;
    }

    protected virtual void LaunchScriptableValueEvent(T value)
    {
        LaunchEvent?.Invoke(value);
    }
}
