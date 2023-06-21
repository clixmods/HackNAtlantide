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
    [SerializeField] private bool SwitchIcons = false;
    [SerializeField] private float switchIconsDelay = 1;
    int currentIndex = 0;
    private static string _bindingGroup;
    public static string bindingGroup => _bindingGroup;
    private void Awake()
    {
        _playerControls ??= new PlayerControls();
        _image = GetComponent<Image>();
        _bindingGroup ??= _playerControls.controlSchemes[0].bindingGroup;
        InputSystem.onAnyButtonPress.Call(OnButtonPressed);
    }

    private void Start()
    {
        if(SwitchIcons)
            StartCoroutine(SwitchIcon());
        
        if ( !SwitchIcons && _inputActionReference != null)
        {
            for (int i = 0; i < _inputActionReference.action.bindings.Count; i++)
            {
                if (_inputActionReference.action.bindings[i].groups == bindingGroup)
                {
                    _image.sprite = inputActionIcons.dictionaryInputsIcons[_inputActionReference.action.bindings[i].path].icon;
                    break;
                }
            }
        }
    }

    IEnumerator SwitchIcon()
    {
        for (int i = 0; i < _inputActionReference.action.bindings.Count; i++)
        {
            if (_inputActionReference.action.bindings[i].groups == bindingGroup)
            {
                var additionnalIcons = inputActionIcons.dictionaryInputsIcons[_inputActionReference.action.bindings[i].path].additionnalIcons;
                if (additionnalIcons == null || additionnalIcons.Length == 0)
                {
                    continue;
                }
                _image.sprite = additionnalIcons[currentIndex];
                if (additionnalIcons.Length-1 > currentIndex) 
                {
                    currentIndex++;
                        
                }
                else
                {
                    currentIndex = 0;
                }

                break;
            }
                
                
        }
        while (true)
        {
            yield return new WaitForSecondsRealtime(switchIconsDelay);
            for (int i = 0; i < _inputActionReference.action.bindings.Count; i++)
            {
                if (_inputActionReference.action.bindings[i].groups == bindingGroup)
                {
                    var additionnalIcons = inputActionIcons.dictionaryInputsIcons[_inputActionReference.action.bindings[i].path].additionnalIcons;
                    if (additionnalIcons == null || additionnalIcons.Length == 0)
                    {
                        continue;
                    }
                    _image.sprite = additionnalIcons[currentIndex];
                    if (additionnalIcons.Length-1 > currentIndex) 
                    {
                        currentIndex++;
                        
                    }
                    else
                    {
                        currentIndex = 0;
                    }

                    break;
                }
                
                
            }
            
        }

        yield return null;
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
        if ( !SwitchIcons && _inputActionReference != null)
        {
            for (int i = 0; i < _inputActionReference.action.bindings.Count; i++)
            {
                if (_inputActionReference.action.bindings[i].groups == bindingGroup)
                {
                    _image.sprite = inputActionIcons.dictionaryInputsIcons[_inputActionReference.action.bindings[i].path].icon;
                    break;
                }
            }
        }
       
    }
}
