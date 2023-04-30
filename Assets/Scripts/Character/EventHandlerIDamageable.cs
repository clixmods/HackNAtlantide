using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventHandlerIDamageable : MonoBehaviour
{
    public UnityEvent OnDamage;
    public UnityEvent OnDeath;
    // Start is called before the first frame update
    void Awake()
    {
        var idamageable = GetComponent<IDamageable>();
        idamageable.OnDamage += IdamageableOnOnDamage;
        idamageable.OnDeath += IdamageableOnOnDeath;
    }

    private void IdamageableOnOnDeath(object sender, EventArgs e)
    {
        OnDeath?.Invoke();
    }

    private void IdamageableOnOnDamage(object sender, EventArgs e)
    {
        OnDamage?.Invoke();
    }
}
