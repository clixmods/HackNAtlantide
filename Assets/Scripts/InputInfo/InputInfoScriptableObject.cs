using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "Data/Input/Input Info", fileName = "InputInfo_")]
public class InputInfoScriptableObject : ScriptableObject
{
    public static Action<InputInfoScriptableObject> InputInfoInit;
    public static Action<InputInfoScriptableObject> InputInfoSend;
    public static Action<InputInfoScriptableObject> InputInfoRemove;
    public string inputText;
    public InputActionReference input;
    
    private void OnEnable()
    {
        InputInfoInit?.Invoke(this);
    }
    public void ShowInputInfo()
    {
        InputInfoSend?.Invoke(this);
    }

    public  void RemoveInputInfo()
    {
        InputInfoRemove?.Invoke(this);
    }
}
