using System;
using System.Collections;
using System.Collections.Generic;
using Attack;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public struct InputUnityEvent
{
    public InputButtonScriptableObject InputButtonScriptableObject;
    public UnityEvent OnInput;

    public void Init()
    {
        InputButtonScriptableObject.OnValueChanged += InputButtonScriptableObjectOnOnValueChanged;
    }
    public void OnDestroy()
    {
        InputButtonScriptableObject.OnValueChanged -= InputButtonScriptableObjectOnOnValueChanged;
    }
    private void InputButtonScriptableObjectOnOnValueChanged(bool obj)
    {
        if (obj)
        {
            OnInput?.Invoke();
        }
    }
}
public class PlayerEvents : MonoBehaviour
{
    [SerializeField] private InputUnityEvent[] _inputsUnityEvent;
    public UnityEvent OnDamage, OnDeath, OnAttackHit , OnDashAttackHit, OnInteract;
    private IDamageable _damageable;
    private IAttackCollider _attackCollider;
    private PlayerCombat _playerCombat;

    private void Awake()
    {
        _playerCombat = GetComponentInChildren<PlayerCombat>();
        _damageable = GetComponentInChildren<IDamageable>(); 
        _damageable .OnDamage += OnOnDamage;
        _damageable.OnDeath += OnOnDeath;
        _attackCollider = GetComponentInChildren<IAttackCollider>();
        _attackCollider.OnCollideWithIDamageable += OnOnCollideWithIDamageable;
       for (int i = 0; i < _inputsUnityEvent.Length; i++)
       {
           _inputsUnityEvent[i].Init();
       }
    }

    private void OnDestroy()
    {
        _damageable.OnDamage -= OnOnDamage;
        _damageable.OnDeath -= OnOnDeath;
        _attackCollider.OnCollideWithIDamageable -= OnOnCollideWithIDamageable;
        for (int i = 0; i < _inputsUnityEvent.Length; i++)
        {
            _inputsUnityEvent[i].OnDestroy();
        }
    }

    private void OnOnCollideWithIDamageable(object sender, EventArgs e)
    {
        if (_playerCombat.IsDashingAttack)
        {
            OnDashAttackHit?.Invoke();
        }
        else
        {
            OnAttackHit?.Invoke();
        }
        
    }
    private void OnOnDeath(object sender, EventArgs e)
    {
        OnDeath?.Invoke();
    }
    private void OnOnDamage(object sender, EventArgs e)
    {
        OnDamage?.Invoke();
    }
}
