using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class PlayerStamina : MonoBehaviour
{
    [SerializeField] PlayerStaminaScriptableObject _staminaSO;
    private bool _rechargeStamina;
    

    private void OnEnable()
    {
        _staminaSO.OnUseStamina += RemoveStamina;
    }
    private void OnDisable()
    {
        _staminaSO.OnUseStamina -= RemoveStamina;
    }
    private void Start()
    {
        _staminaSO.ResetStamina();
        _rechargeStamina = true;
    }
    private void Update()
    {
        if(!_staminaSO.IsMaxStamina())
        {
            if(_rechargeStamina)
            {
                _staminaSO.Value += Time.deltaTime * _staminaSO.SpeedToRecharge;   
            }
        }
        else
        {
            if(_staminaSO.WaitForRecharge)
            {
                _staminaSO.OnStaminaIsFilledAfterRecharge?.Invoke();
                _staminaSO.WaitForRecharge = false;
            }
        }
    }
    void RemoveStamina(float value)
    {
        _staminaSO.Value -= value;
        if(_staminaSO.Value<0)
        {
            _staminaSO.Value = 0;
            _staminaSO.OnStaminaIsEmpty?.Invoke();
            StartCoroutine(WaitToRechargeStamina());
        }
    }
    IEnumerator WaitToRechargeStamina()
    {
        _rechargeStamina = false;
        _staminaSO.WaitForRecharge = true;
        yield return new WaitForSeconds(1f);
        _rechargeStamina = true;
    }
    
}
