using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public interface IDamageable
{
    event EventHandler OnDamage;  
    event EventHandler OnDeath;
    event EventHandler OnChangeHealth;
    public float maxHealth { get; }
    public float health { get; }
    public void DoDamage(float damage);
    public void Dead();
    public void AddHealth(float health);

}
