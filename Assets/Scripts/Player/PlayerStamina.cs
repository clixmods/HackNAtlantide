using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class PlayerStamina : MonoBehaviour
{
    [SerializeField] PlayerStaminaScriptableObject _staminaSO;
    [SerializeField] ParticleSystem _useStaminaFX;
    [SerializeField] ParticleSystem _failUseStaminaFX;


    private void OnEnable()
    {
        _staminaSO.OnUseStamina += RemoveStamina;
        _staminaSO.FailUseStamina += FailStamina;
    }
    private void OnDisable()
    {
        _staminaSO.OnUseStamina -= RemoveStamina;
        _staminaSO.FailUseStamina -= FailStamina;
    }
    private void Start()
    {
        _staminaSO.ResetStamina();
    }
    private void Update()
    {
        if(!_staminaSO.IsMaxStamina())
        {
            _staminaSO.Value += Time.deltaTime * _staminaSO.SpeedToRecharge;
        }
    }
    void RemoveStamina(float value)
    {
        
        _staminaSO.Value -= value;
        if(_staminaSO.Value<0)
        {
            _staminaSO.Value = 0;
            _staminaSO.OnStaminaIsEmpty?.Invoke();
        }

        //feedBack
        _useStaminaFX.Play();
    }
    
    void FailStamina()
    {
        _failUseStaminaFX.Play();
    }
}
