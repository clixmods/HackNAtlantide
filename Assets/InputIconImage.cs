using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[ExecuteAlways]
public class InputIconImage : MonoBehaviour
{
    [SerializeField] private InputActionReference _inputActionReference;
    private Image _image;
    [SerializeField] protected InputActionIcons inputActionIcons;

    private void Awake()
    {
        _image = GetComponent<Image>();
        _bindingGroup = _playerControls.controlSchemes[0].bindingGroup;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        InputDevice lastUsedDevice = null;
        float lastEventTime = 0;
        foreach (var device in InputSystem.devices)
        {
            if (device.lastUpdateTime > lastEventTime)
            {
                lastUsedDevice = device;
                lastEventTime = (float)device.lastUpdateTime;
            }
        }
        if (_inputActionReference != null && lastUsedDevice != null)
        {
            for (int i = 0; i < _inputActionReference.action.bindings.Count; i++)
            {
                if (_inputActionReference.action.bindings[i].i == lastUsedDevice.parent)
                {
                    _image.sprite = inputActionIcons.dictionaryInputsIcons[_inputActionReference.action.bindings[i].path];
                    break;
                }
            }
        }
       
    }
}
