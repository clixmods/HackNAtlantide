using System;
using System.Collections;
using System.Collections.Generic;
using Attack;
using UnityEngine;

public class AttackCollider : MonoBehaviour , IAttackCollider
{
    public List<IDamageable> hitsted; 
    public event EventHandler OnCollideWithIDamageable;
    AnimationEvent _damageActiveAnimatorEvent;
    
    

    [SerializeField] private bool sendEventOnEnter = true;
    [SerializeField] private bool sendEventOnStay = false; 
    [SerializeField] private bool sendEventOnExit = false;

    [SerializeField] private LayerMask interactWithLayers;
    private void OnEnable()
    {
        hitsted = new List<IDamageable>();
    }

    private void OnDisable()
    {
        hitsted = new List<IDamageable>();
    }

    private void OnHit(IDamageable damageable)
    {
        if (hitsted.Contains(damageable)) return;
        
        hitsted.Add(damageable);
        OnCollideWithIDamageable?.Invoke(this, new DamageableEventArgs()
        {
            idamageable = damageable
        });
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!IsInteractable(other.gameObject)) return;
        
        if (!enabled) return;
        if (!sendEventOnEnter) return;
        if(other.TryGetComponent<IDamageable>(out var damageable))
        {
            Debug.Log(other.gameObject.name);
            OnHit(damageable);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (!IsInteractable(other.gameObject)) return;
        if (!enabled) return;
        if (!sendEventOnStay) return;
        if(other.TryGetComponent<IDamageable>(out var damageable))
        {
            OnHit(damageable);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (!IsInteractable(other.gameObject)) return;
        
        if (!enabled) return;
        if (!sendEventOnExit) return;
        if(other.TryGetComponent<IDamageable>(out var damageable))
        {
            OnHit(damageable);
        }
    }
    
    private bool IsInteractable(GameObject gameObject)
    {
        if (interactWithLayers == 0) return true;
        return interactWithLayers == (interactWithLayers | (1 << gameObject.layer));
    }
}