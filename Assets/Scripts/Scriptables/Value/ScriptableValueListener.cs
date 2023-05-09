using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScriptableValueListener<T> : MonoBehaviour 
{
    [SerializeField] protected ValueScriptableObject<T> _scriptableEvent;
    public UnityEvent<T> LaunchEvent;
    [SerializeField] private bool launchEventOnStart;
    protected virtual void Awake()
    {
        _scriptableEvent.OnValueChanged += LaunchScriptableValueEvent;
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

    private void LaunchScriptableValueEvent(T value)
    {
        LaunchEvent?.Invoke(value);
    }
}
