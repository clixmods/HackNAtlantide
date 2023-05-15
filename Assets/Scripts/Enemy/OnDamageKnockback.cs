using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDamageKnockback : MonoBehaviour
{
    private IDamageable _iDamageable;

    private void OnEnable()
    {
        _iDamageable = GetComponent<IDamageable>();
        _iDamageable.OnDamage += KnockBack;
    }

    private void KnockBack(object sender, EventArgs e)
    {
        
    }

    private void OnDisable()
    {
        _iDamageable.OnDamage -= KnockBack;
    }
}
