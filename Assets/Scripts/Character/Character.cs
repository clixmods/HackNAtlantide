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
    void Awake()
    {
        health = maxHealth;
    }
    
    public virtual void DoDamage(float damage , Vector3 attackLocation)
    {
        if (health <= 0)
        {
            Debug.Log("damage");
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
        OnDeath?.Invoke(this, null);
    }

    public void AddHealth(float amount)
    {
        health += amount;
        OnChangeHealth?.Invoke(this,null);
        if (health > maxHealth)
        {
            health = maxHealth;
        }
    }
}

