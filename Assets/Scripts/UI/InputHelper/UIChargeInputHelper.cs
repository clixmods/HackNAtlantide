using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.UI;
using Vector3 = UnityEngine.Vector3;

[AddComponentMenu("#Custom/UI/UI Charge Input Helper")]
public class UIChargeInputHelper : UIInputHelper
{
    [SerializeField] private Image imageInputBackground;
    public override UIInputHelper Init(GameObject prefab, Transform transformTarget, Sprite image, float maxDistanceToShow,
        InputActionReference input = default)
    {
        var value = base.Init(prefab, transformTarget, image, maxDistanceToShow, input);
        imageInputBackground.sprite = image;
        return value;
    }

    protected override void UpdateInputIcon()
    {
        if (_inputActionReference != null)
        {
            for (int i = 0; i < _inputActionReference.action.bindings.Count; i++)
            {
                if (_inputActionReference.action.bindings[i].groups == bindingGroup)
                {
                    _imageInput.sprite = inputActionIcons.dictionaryInputsIcons[_inputActionReference.action.bindings[i].path];
                    imageInputBackground.sprite = _imageInput.sprite;
                    break;
                }
            }
        }
    }

    public void SetFillValue(float value)
    {
        _imageInput.fillAmount = value;
    }
}
