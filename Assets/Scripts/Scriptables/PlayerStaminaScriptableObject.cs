using System;
using System.Collections;
using System.Collections.Generic;
using _2DGame.Scripts.Save;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/PlayerStamina")]
public class PlayerStaminaScriptableObject : ScriptableObjectSaveable
{
    [SerializeField] private float _maxValue;
    [SerializeField] private float _value;
    [SerializeField] private float _speedToRecharge = 1f;
    public float SpeedToRecharge => _speedToRecharge;
    public float Value
    {
        get => _value;
        set
        {
            _value = value > _maxValue ? _maxValue : value;
            OnValueChanged?.Invoke(_value);
        }
    }

    public float MaxStamina => _maxValue;
    public bool IsMaxStamina()
    { 
        return _value >= _maxValue; 
    }

    public Action<float> OnValueChanged;
    public Action OnStaminaIsEmpty;
    public Action FailUseStamina;
    public bool CanUseStamina(float amount)
    {
        if(Value - amount >= 0)
        {
            return true;
        }
        FailUseStamina?.Invoke();
        return false; 
    }
    public Action<float> OnUseStamina;
    public void UseStamina(float value)
    {
        OnUseStamina?.Invoke(value);
    }
    public void ResetStamina()
    {
        Value = _maxValue;
    }

    #region Saveable

    class PlayerStamina : SaveData
    {
        public float currentStamina;
        public float maxStamina;
    }
    
    public override void OnLoad(string data)
    {
        PlayerStamina playerStamina = JsonUtility.FromJson<PlayerStamina>(data);
        _maxValue = playerStamina.maxStamina;
        Value = playerStamina.currentStamina;
    }

    public override void OnSave(out SaveData saveData)
    {
        PlayerStamina playerStaminaData = new PlayerStamina();
        playerStaminaData.currentStamina = Value;
        playerStaminaData.maxStamina = _maxValue;
        saveData = playerStaminaData;
    }

    public override void OnReset()
    {
        Value = MaxStamina;
    }

    #endregion
}
