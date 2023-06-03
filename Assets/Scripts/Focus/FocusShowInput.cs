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
    [SerializeField] private ScriptableValueListGameObject valueListGameObject;
    void Awake()
    {
        _focus = GetComponent<Focus>();
        
    }
    private void OnValueChanged(List<GameObject> obj)
    {
        if (!Focus.FocusIsEnable)
        {
            if ( obj.Count > 0)
            {
                inputEnableFocus.ShowInputInfo();
            }
            else 
            {
                inputEnableFocus.RemoveInputInfo();
            }
        }
    }

    private void OnEnable()
    {
        Focus.OnFocusSwitch += FocusSwitch;
        Focus.OnFocusEnable += FocusEnable;
        Focus.OnFocusDisable += FocusDisable;
        Focus.OnFocusNoTarget += OnFocusNoTarget;
        valueListGameObject.OnValueChanged += OnValueChanged;
    }
    private void OnDisable()
    {
        Focus.OnFocusSwitch -= FocusSwitch;
        Focus.OnFocusEnable -= FocusEnable;
        Focus.OnFocusDisable -= FocusDisable;
        Focus.OnFocusNoTarget -= OnFocusNoTarget;
        valueListGameObject.OnValueChanged -= OnValueChanged;
    }
    private void OnFocusNoTarget()
    {
        inputEnableFocus.RemoveInputInfo();
        inputDisableFocus.RemoveInputInfo();
        inputSwitchFocus.RemoveInputInfo();
        OnValueChanged(valueListGameObject.Value);
    }

    private void FocusDisable()
    {
        inputSwitchFocus.RemoveInputInfo();
        inputDisableFocus.RemoveInputInfo();
        if ( valueListGameObject.Count > 0)
        {
            inputEnableFocus.ShowInputInfo();
        }
        else 
        {
            inputEnableFocus.RemoveInputInfo();
        }
    }

    private void FocusEnable()
    {
        inputEnableFocus.RemoveInputInfo();
        inputDisableFocus.ShowInputInfo();
        if (_focus.FocusablesAvailable.Count > 1)
        {
            inputSwitchFocus.ShowInputInfo();
        }
    }

    private void FocusSwitch(IFocusable target)
    {
        if (!Focus.FocusIsEnable)
        {
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
