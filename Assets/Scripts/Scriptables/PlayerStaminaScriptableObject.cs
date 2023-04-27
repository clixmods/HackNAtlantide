using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/PlayerStamina")]
public class PlayerStaminaScriptableObject : ScriptableObject
{
    [SerializeField] private float _maxValue;
    [SerializeField] private float _value;
    public float Value
    {
        get => _value;
        set
        {
            _value = value > _maxValue ? _maxValue : value;
            OnValueChanged?.Invoke(_value);
        }
    }
    
    private bool waitForRecharge;
    public bool WaitForRecharge
    {
        get => waitForRecharge;
        set
        {
            waitForRecharge = value;
        }
    }
    public bool IsMaxStamina()
    { 
        return _value == _maxValue; 
    }

    public Action<float> OnValueChanged;
    public Action OnStaminaIsEmpty;
    public Action OnStaminaIsFilledAfterRecharge;
    public bool CanUseStamina()
    {
        return !waitForRecharge;
    }
    public Action<float> OnUseStamina;
    public void UseStamina(float value)
    {
        OnUseStamina?.Invoke(value);
    }
    public void ResetStamina()
    {
        Value = _maxValue;
        waitForRecharge=false;
    }
}
