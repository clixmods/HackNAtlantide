using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaSlider : MonoBehaviour
{
    Slider slider;
    [SerializeField] PlayerStaminaScriptableObject _staminaSO;
    private void Awake()
    {
        slider = GetComponent<Slider>();
    }
    private void OnEnable()
    {
        _staminaSO.OnValueChanged += ChangeValue;
    }
    private void OnDisable()
    {
        _staminaSO.OnValueChanged -= ChangeValue;
    }

    void ChangeValue(float value)
    {
        slider.value = value;
    }
}
