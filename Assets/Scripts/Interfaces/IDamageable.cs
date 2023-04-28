using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public interface IDamageable
{
    event EventHandler OnDamage;  
    event EventHandler OnDeath; 
    public float health { get; }
    public void TakeDamage(float damage);
    public void Dead();
    public void AddHealth(float health);

}
