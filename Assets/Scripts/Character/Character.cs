using System;
using UnityEngine;

public class Character : MonoBehaviour,  IDamageable
{
    [SerializeField] private float _maxHealth;
    private float _health;
    public event EventHandler OnDamage;
    public event EventHandler OnDeath;
    public event EventHandler OnChangeHealth;
    public float maxHealth 
    {
        get
        {
            return _maxHealth;
        }
        set
        {
            OnChangeHealth?.Invoke(this,null);
            _maxHealth = value;
        }
    }
    public float health
    {
        get
        {
            return _health;
        }
        set
        {
            OnChangeHealth?.Invoke(this,null);
            _health = value;
        }
    }
    void Start()
    {
        health = maxHealth;
    }
    
    public void DoDamage(float damage)
    {
        health -= damage;
        OnDamage?.Invoke(this,null);
        if(health <= 0f)
        {
            Dead();
        }
    }
    public virtual void Dead()
    {
        OnDeath?.Invoke(this, null);
    }

    public void AddHealth(float amount)
    {
        health += health;
        OnChangeHealth?.Invoke(this,null);
        if (amount > maxHealth)
        {
            health = maxHealth;
        }
    }
}

