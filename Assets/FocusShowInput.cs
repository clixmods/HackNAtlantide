using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class FocusShowInput : MonoBehaviour
{
    private Focus _focus;
    [SerializeField] private InputInfoScriptableObject inputEnableFocus;
    [SerializeField] private InputInfoScriptableObject inputDisableFocus;
    [SerializeField] private InputInfoScriptableObject inputSwitchFocus;
    void Awake()
    {
        _focus = GetComponent<Focus>();
    }

    private void OnEnable()
    {
        Focus.OnFocusSwitch += FocusOnOnFocusSwitch;
        Focus.OnFocusEnable += FocusOnOnFocusEnable;
        Focus.OnFocusDisable += FocusOnOnFocusDisable;
        Focus.OnFocusNoTarget += FocusOnOnFocusNoTarget;
    }
    private void OnDisable()
    {
        Focus.OnFocusSwitch -= FocusOnOnFocusSwitch;
        Focus.OnFocusEnable -= FocusOnOnFocusEnable;
        Focus.OnFocusDisable -= FocusOnOnFocusDisable;
        Focus.OnFocusNoTarget -= FocusOnOnFocusNoTarget;
    }

    private void FocusOnOnFocusNoTarget()
    {
        inputEnableFocus.RemoveInputInfo();
        inputDisableFocus.RemoveInputInfo();
        inputSwitchFocus.RemoveInputInfo();
    }

    private void FocusOnOnFocusDisable()
    {
        inputSwitchFocus.RemoveInputInfo();
        if (_focus.FocusablesAvailable.Count > 0)
        {
            inputEnableFocus.ShowInputInfo();
            inputDisableFocus.RemoveInputInfo();
        }
        else
        {
            inputEnableFocus.RemoveInputInfo();
            inputDisableFocus.RemoveInputInfo();
        }
            
    }

    private void FocusOnOnFocusEnable()
    {
        inputEnableFocus.RemoveInputInfo();
        inputDisableFocus.ShowInputInfo();
        if (_focus.FocusablesAvailable.Count > 1)
        {
            inputSwitchFocus.ShowInputInfo();
        }
    }

    private void FocusOnOnFocusSwitch(IFocusable target)
    {
        if (!Focus.FocusIsEnable && _focus.FocusablesAvailable.Count <= 0)
        {
            inputEnableFocus.RemoveInputInfo();
            inputDisableFocus.RemoveInputInfo();
            inputSwitchFocus.RemoveInputInfo();
        }

        if (Focus.FocusIsEnable)
        {
            if (_focus.FocusablesAvailable.Count > 1)
            {
                inputSwitchFocus.ShowInputInfo();
            }
            else
            {
                inputSwitchFocus.RemoveInputInfo();
            }
        }
    }

   
}
