using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    [SerializeField] EnemyController _enemyController;
    [SerializeField] private float _maxHealth;
    private float _health;
    public event EventHandler OnDamage;
    public event EventHandler OnDeath;
    public float health { get { return _health; } private set { _health = value; } }

    void Start()
    {
        _health = _maxHealth;
    }

    public void Dead()
    {
        Destroy(gameObject.transform.parent.gameObject);
    }

    public void TakeDamage(float damage)
    {
        if (_enemyController.IsAwake)
        {
            OnDamage?.Invoke(this , null );
            _health -= damage;
            Debug.Log("Enemy damaged");
        }
        
        if (_health < 0f)
        {
            Dead();
        }
    }

    public void AddHealth(float health)
    {
        _health += health;
        if (health > _maxHealth)
        {
            _health = _maxHealth;
        }
    }
}
