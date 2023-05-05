using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScriptableEventListener : MonoBehaviour
{
    [SerializeField] private EventScriptableObject _scriptableEvent;
    public UnityEvent LaunchEvent;
    private void Awake()
    {
        _scriptableEvent.OnEvent += ScriptableEventOnOnEvent;
    }

    private void OnDestroy()
    {
        _scriptableEvent.OnEvent -= ScriptableEventOnOnEvent;
    }

    private void ScriptableEventOnOnEvent()
    {
        LaunchEvent?.Invoke();
    }
}
