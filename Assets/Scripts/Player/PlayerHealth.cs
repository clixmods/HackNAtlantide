using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class PlayerHealth : MonoBehaviour,IDamageable
{
    [SerializeField] private float _maxHealth;
    private float _health;
    public float health { get { return _health; } private set { _health = value; } }

    void Start()
    {
        _health = _maxHealth;
    }

    public void Dead()
    {
        Debug.Log("dead");
    }

    public void TakeDamage(float damage)
    {
        _health -= damage;
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
