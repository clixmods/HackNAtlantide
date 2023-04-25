using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public interface IDamageable
{
    public float health { get; set; }
    public void TakeDamage(float damage);

}
