using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public interface IDamageable
{
    event EventHandler OnDamage;  
    event EventHandler OnDeath;
    /// <summary>
    /// Event when the health have a changed, warning all damageable inheritors need to invoke it when its happen
    /// </summary>
    event EventHandler OnChangeHealth;
    /// <summary>
    /// Max health of the damageable
    /// </summary>
    public float maxHealth { get; }
    /// <summary>
    /// Current health of the damageable
    /// </summary>
    public float health { get; }
    public void DoDamage(float damage);
    /// <summary>
    /// Method executed when the damageable is dead, warning all inheritors need to call it when it's necessary
    /// </summary>
    public void Dead();
    public void AddHealth(float health);

}
