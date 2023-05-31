using System;
using System.Collections;
using System.Collections.Generic;
using _2DGame.Scripts.Save;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Data/PlayerStamina")]
public class PlayerStaminaScriptableObject : ScriptableValueFloatSaveable
{
    [SerializeField] private float _speedToRecharge = 1f;

    bool _canRechargeStamina;
    public bool CanRechargeStamina { get { return _canRechargeStamina; } set { _canRechargeStamina = value; } }
    public float SpeedToRecharge => _speedToRecharge;
    public override float Value
    {
        get
        {
            return base.Value;
        }
        set
        {
            base.Value = value;
            OnValueChanged?. Invoke(value);
        }
    }


    public float MaxStamina => MaxValue;
    public bool IsMaxStamina()
    { 
        return Value >= MaxValue; 
    }

    public Action<float> OnValueChanged;
    public Action OnStaminaIsEmpty;
    public Action FailUseStamina;
    public UnityEvent UnityEventFailUseStamina;
    public bool CanUseStamina(float amount)
    {
        if(Value - amount >= 0)
        {
            return true;
        }
        FailUseStamina?.Invoke();
        UnityEventFailUseStamina?.Invoke();
        return false; 
    }
    public Action<float> OnUseStamina;
    public void UseStamina(float value)
    {
        OnUseStamina?.Invoke(value);
    }
    public void ResetStamina()
    {
        Value = MaxStamina;
    }
    
}
