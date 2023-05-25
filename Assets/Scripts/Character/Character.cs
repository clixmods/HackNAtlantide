﻿using System;
using UnityEngine;

public class Character : MonoBehaviour,  IDamageable
{
    [SerializeField] private float _maxHealth;

    private float currentSpeed;
    public float CurrentSpeed { get { return currentSpeed; } set { currentSpeed = value; } }

    [Header("Feedback")]
    [SerializeField] RumblerDataConstant _attackRumble;
    
    private float _health;

    [Header("Settings")] 
    [SerializeField] private bool isInvulnerable = false;

    private bool _isDead;
    public bool IsInvulnerable { get { return isInvulnerable; } set { OnInvulnerable?.Invoke(value); isInvulnerable = value; } }
    #region Events
    public event EventHandler OnDamage;
    public event EventHandler OnDeath;
    public event EventHandler OnChangeHealth;
    public Action<bool> OnInvulnerable;
    #endregion

    public virtual float maxHealth 
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
    public virtual float health
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
    void Awake()
    {
        InitHealth();
    }

    protected virtual void InitHealth()
    {
        health = maxHealth;
    }

    public virtual void DoDamage(float damage , Vector3 attackLocation)
    {
        if (isInvulnerable)
        {
            return;
        }
        if (health <= 0)
        {
            return;
        }
            
        
        health -= damage;
        AttackFeedback();
        OnDamage?.Invoke(this,new DoDamageEventArgs
            {
                damage = damage,
                attackPosition = attackLocation,
                attacker = this
            }
            
            );
        
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
        if (_isDead)
        {
            return;
        }
        _isDead = true;
        OnDeath?.Invoke(this, null);
    }

    public virtual void AddHealth(float amount)
    {
        health += amount;
        OnChangeHealth?.Invoke(this,null);
        if (health > maxHealth)
        {
            health = maxHealth;
        }
    }

    public void SetInvulnerability(bool value)
    {
        IsInvulnerable = value;
    }
}

