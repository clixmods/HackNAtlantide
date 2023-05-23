using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AscenceurInteractable : MonoBehaviour,IInteractable
{
    [SerializeField] Transform _startPos;
    [SerializeField] Transform _endPos;
    [SerializeField] float _speedUp;
    [SerializeField] float _speedDown;
    bool _isInteract;
    float time = 0;
    BoxCollider _collider;
    [SerializeField] PlayerStaminaScriptableObject _playerStamina;
    [SerializeField] float costStamina;
    float distance;
    float totaluse;
    private void Awake()
    {
        _collider = GetComponent<BoxCollider>();
        distance = Mathf.Abs(_endPos.position.y - _startPos.position.y);
        Debug.Log(distance);

    }
    private void Update()
    {
        

        
        if(_isInteract)
        {
            float cost = costStamina * Time.deltaTime * _speedUp / distance;
            if (_playerStamina.CanUseStamina(cost))
            {
                if (transform.position.y < _endPos.position.y)
                {
                    _playerStamina.UseStamina(cost);
                    transform.position += Vector3.up * Time.deltaTime * _speedUp;


                    totaluse += cost;
                    Debug.Log(totaluse);
                }
            }
            else
            {
                _isInteract = false;
                _playerStamina.CanRechargeStamina = true;
            }
        }
        else
        {
            if (transform.position.y > _startPos.position.y)
            {
                transform.position += Vector3.down * _speedDown * Time.deltaTime;
                _collider.enabled = false;
            }
            else
            {
                _collider.enabled = true;
            }
        }
    }
    public void CancelInteract()
    {
        _isInteract = false;
        _playerStamina.CanRechargeStamina = true;
    }

    public bool Interact()
    {
        _collider.enabled = true;
        _isInteract = true;
        _playerStamina.CanRechargeStamina = false;
        return true;
    }

    public void ResetInteract()
    {
        _isInteract= false;
    }

    public void ResetTransform()
    {
        throw new System.NotImplementedException();
    }
}
