using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FocusListener : MonoBehaviour
{
    public UnityEvent FocusEnable;
    public UnityEvent FocusDisable;
    private void Awake()
    {
        Focus.OnFocusEnable += FocusOnOnFocusEnable;
        Focus.OnFocusDisable += FocusOnOnFocusDisable;
    }
    private void OnDestroy()
    {
        Focus.OnFocusEnable -= FocusOnOnFocusEnable;
        Focus.OnFocusDisable -= FocusOnOnFocusDisable;
    }

    private void FocusOnOnFocusDisable()
    {
        FocusDisable?.Invoke();
    }

    private void FocusOnOnFocusEnable()
    {
        FocusEnable?.Invoke();
    }
}
