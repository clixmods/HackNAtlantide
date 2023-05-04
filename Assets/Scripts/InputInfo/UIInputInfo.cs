using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIInputInfo : MonoBehaviour
{
    private InputIconImage _inputIconImage;
    private TextMeshProUGUI _textMeshProUGUI;
    private void Awake()
    {
        _inputIconImage = GetComponentInChildren<InputIconImage>();
        _textMeshProUGUI = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void Send(InputInfoScriptableObject inputInfoScriptableObject)
    {
        _inputIconImage._inputActionReference = inputInfoScriptableObject.input;
        _textMeshProUGUI.text = inputInfoScriptableObject.inputText;
    }
}
