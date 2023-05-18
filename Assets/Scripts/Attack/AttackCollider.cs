using System;
using System.Collections;
using System.Collections.Generic;
using Attack;
using UnityEngine;
using UnityEngine.Serialization;

public class AttackCollider : MonoBehaviour , IAttackCollider
{
    private List<IDamageable> _damageableHitted; 
    public event EventHandler OnCollideWithIDamageable;
    AnimationEvent _damageActiveAnimatorEvent;
    
    [SerializeField] private bool sendEventOnEnter = true;
    [SerializeField] private bool sendEventOnStay = false; 
    [SerializeField] private bool sendEventOnExit = false;

    [SerializeField] private LayerMask interactWithLayers;

    [SerializeField] ParticleSystem _hitVfx;
    private void OnEnable()
    {
        _damageableHitted = new List<IDamageable>();
    }

    private void OnDisable()
    {
        _damageableHitted = new List<IDamageable>();
    }

    private void OnHit(IDamageable damageable)
    {
        if (_damageableHitted.Contains(damageable)) return;
        
        _damageableHitted.Add(damageable);
        OnCollideWithIDamageable?.Invoke(this, new AttackDamageableEventArgs()
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
            _hitVfx.Play();
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
            _hitVfx.Play();
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