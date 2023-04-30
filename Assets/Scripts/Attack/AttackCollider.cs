using System;
using System.Collections;
using System.Collections.Generic;
using Attack;
using UnityEngine;

public class AttackCollider : MonoBehaviour , IAttackCollider
{
    public event EventHandler OnCollideWithIDamageable;
    AnimationEvent _damageActiveAnimatorEvent;

    [SerializeField] private bool sendEventOnEnter = true;
    [SerializeField] private bool sendEventOnStay = false; 
    [SerializeField] private bool sendEventOnExit = false; 
    private void OnTriggerEnter(Collider other)
    {
        if (!sendEventOnEnter) return;
        if(other.TryGetComponent<IDamageable>(out var damageable))
        {
            OnCollideWithIDamageable?.Invoke(this, new DamageableEventArgs()
            {
                idamageable = damageable
            });
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (!sendEventOnStay) return;
        if(other.TryGetComponent<IDamageable>(out var damageable))
        {
            OnCollideWithIDamageable?.Invoke(this, new DamageableEventArgs()
            {
                idamageable = damageable
            });
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (!sendEventOnExit) return;
        if(other.TryGetComponent<IDamageable>(out var damageable))
        {
            OnCollideWithIDamageable?.Invoke(this, new DamageableEventArgs()
            {
                idamageable = damageable
            });
        }
    }
}