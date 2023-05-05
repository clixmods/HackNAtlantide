using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScriptableValueListener<T> : MonoBehaviour 
{
    [SerializeField] private ValueScriptableObject<T> _scriptableEvent;
    public UnityEvent<T> LaunchEvent;
    private void Awake()
    {
        _scriptableEvent.OnValueChanged += ScriptableEventOnOnEvent;
    }

    private void OnDestroy()
    {
        _scriptableEvent.OnValueChanged -= ScriptableEventOnOnEvent;
    }

    private void ScriptableEventOnOnEvent(T value)
    {
        LaunchEvent?.Invoke(value);
    }
}
