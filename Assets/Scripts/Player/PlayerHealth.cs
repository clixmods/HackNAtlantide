using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class PlayerHealth : MonoBehaviour,IDamageable
{
    [SerializeField] private float _maxHealth;
    private float _health;
    public event EventHandler OnDamage;
    public event EventHandler OnDeath;
    public event EventHandler OnChangeHealth;
    public float maxHealth => _maxHealth;

    public float health
    {
        get
        {
            return _health;
        }
        private set
        {
            OnChangeHealth?.Invoke(this,null);
            _health = value;
        }
    }

    void Start()
    {
        _health = _maxHealth;
    }

    public void Dead()
    {
        OnDeath?.Invoke(this,null);
        // TODO - Make the dead function
    }

    public void TakeDamage(float damage)
    {
        _health -= damage;
        OnDamage?.Invoke(this,null);
        if(_health < 0f)
        {
            Dead();
        }
    }

    public void AddHealth(float health)
    {
        _health += health;
        
        if(health > _maxHealth)
        {
            _health = _maxHealth;
        }
    }
}
