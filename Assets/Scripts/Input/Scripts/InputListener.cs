using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;

public class InputListener : MonoBehaviour
{
    [SerializeField] InputScriptableObject<bool> inputToListen;
    public UnityEvent InputValueTrue;
    public UnityEvent InputValueFalse;
    private void Awake()
    {
        inputToListen.OnValueChanged += obj =>
        {
            if (obj)
            {
                InputValueTrue?.Invoke();
            }
            else
            {
                InputValueFalse?.Invoke();
            }
            
        };
    }
}
