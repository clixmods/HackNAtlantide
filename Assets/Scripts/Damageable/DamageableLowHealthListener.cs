using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(IDamageable))]
public class DamageableLowHealthListener : MonoBehaviour
{
    private IDamageable _damageable;
    private bool _isLowHealth;
    public UnityEvent LowHealthEnter;
    public UnityEvent LowHealthExit;

    #region MonoBehaviour

    private void Awake()
    {
        _damageable = GetComponent<IDamageable>();
        DamageableOnChangeHealth(_damageable.health, _damageable.maxHealth);
    }

    private void OnEnable()
    {
        _damageable.OnChangeHealth += DamageableOnChangeHealth;
    }

    private void OnDisable()
    {
        _damageable.OnChangeHealth -= DamageableOnChangeHealth;
    }

    #endregion

    private void DamageableOnChangeHealth(float health, float maxHealth)
    {
        
        if (maxHealth - health < 0.1f || (health > 1 && _isLowHealth))
        {
            _isLowHealth = false;
            LowHealthExit?.Invoke();
            return;
        }

        if (health <= 1 && !_isLowHealth)
        {
            _isLowHealth = true;
            LowHealthEnter?.Invoke();
            return;
        }
    }
}