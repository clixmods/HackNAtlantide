using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.UI;

[ExecuteAlways]
public class InputIconImage : MonoBehaviour
{
    private static PlayerControls _playerControls ;
     public InputActionReference _inputActionReference;
    private Image _image;
    [SerializeField] protected InputActionIcons inputActionIcons;
    private static string _bindingGroup;
    public static string bindingGroup => _bindingGroup;
    private void Awake()
    {
        _playerControls ??= new PlayerControls();
        _image = GetComponent<Image>();
        _bindingGroup ??= _playerControls.controlSchemes[0].bindingGroup;
        InputSystem.onAnyButtonPress.Call(OnButtonPressed);
    }
    private void OnButtonPressed(InputControl button)
    {
        var list = _playerControls.controlSchemes.ToArray();
        for (int i = 0; i < list.Length; i++)
        {
            if (list[i].SupportsDevice(button.device))
            {
                _bindingGroup = list[i].bindingGroup;
            }
        }
    }
  

    // Update is called once per frame
    void Update()
    {
        if (_inputActionReference != null)
        {
            for (int i = 0; i < _inputActionReference.action.bindings.Count; i++)
            {
                if (_inputActionReference.action.bindings[i].groups == bindingGroup)
                {
                    _image.sprite = inputActionIcons.dictionaryInputsIcons[_inputActionReference.action.bindings[i].path];
                    break;
                }
            }
        }
       
    }
}
