using System;
using UnityEngine;

public class Character : MonoBehaviour,  IDamageable
{
    [SerializeField] private float _maxHealth;
    
    [Header("Feedback")]
    [SerializeField] RumblerDataConstant _attackRumble;
    
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
    
    public virtual void DoDamage(float damage , Vector3 attackLocation)
    {
        health -= damage;
        AttackFeedback();
        OnDamage?.Invoke(this,null);
        if(health <= 0f)
        {
            Dead();
        }
    }
    void AttackFeedback()
    {
        Rumbler.instance.RumbleConstant(_attackRumble);
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

