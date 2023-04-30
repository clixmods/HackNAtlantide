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
    public UnityEvent OnDamage, OnDeath, OnAttackHit , OnInteract;

    private void Awake()
    {
       GetComponent<IDamageable>().OnDamage += OnOnDamage;
       GetComponent<IDamageable>().OnDeath += OnOnDeath;
       GetComponentInChildren<IAttackCollider>().OnCollideWithIDamageable += OnOnCollideWithIDamageable;
       for (int i = 0; i < _inputsUnityEvent.Length; i++)
       {
           _inputsUnityEvent[i].Init();
       }
    }
    
    private void OnOnCollideWithIDamageable(object sender, EventArgs e)
    {
        OnAttackHit?.Invoke();
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
