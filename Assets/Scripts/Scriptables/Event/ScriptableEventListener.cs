using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class ScriptableEventListener : MonoBehaviour
{
    [FormerlySerializedAs("_scriptableEvent")] [SerializeField] private ScriptableEvent scriptableScriptableEvent;
    public UnityEvent LaunchEvent;
    private void Awake()
    {
        scriptableScriptableEvent.OnEvent += ScriptableScriptableEventOnOnEvent;
    }

    private void OnDestroy()
    {
        scriptableScriptableEvent.OnEvent -= ScriptableScriptableEventOnOnEvent;
    }

    private void ScriptableScriptableEventOnOnEvent()
    {
        LaunchEvent?.Invoke();
    }
}
