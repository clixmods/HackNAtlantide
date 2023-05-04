using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FocusListener : MonoBehaviour
{
    public UnityEvent FocusEnable;
    public UnityEvent FocusDisable;
    public UnityEvent FocusNoTarget;
    public UnityEvent FocusSwitch;
    private void Awake()
    {
        Focus.OnFocusEnable += FocusOnOnFocusEnable;
        Focus.OnFocusDisable += FocusOnOnFocusDisable;
        Focus.OnFocusNoTarget += FocusOnOnFocusNoTarget;
        Focus.OnFocusSwitch += FocusOnOnFocusSwitch;
    }

    private void FocusOnOnFocusSwitch(ITargetable target)
    {
        FocusSwitch?.Invoke();
    }

    private void FocusOnOnFocusNoTarget()
    {
        FocusNoTarget?.Invoke();
    }

    private void OnDestroy()
    {
        Focus.OnFocusEnable -= FocusOnOnFocusEnable;
        Focus.OnFocusDisable -= FocusOnOnFocusDisable;
        Focus.OnFocusNoTarget -= FocusOnOnFocusNoTarget;
        Focus.OnFocusSwitch -= FocusOnOnFocusSwitch;
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
