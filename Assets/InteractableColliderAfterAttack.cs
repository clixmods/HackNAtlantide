using Attack;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class InteractableColliderAfterAttack : MonoBehaviour
{
    SphereCollider _collider;
    [SerializeField] float _speedExplosion;
    [SerializeField] float _maxRadius;
    private void Start()
    {
        _collider = GetComponent<SphereCollider>();
        _collider.radius = 0.5f;
        _collider.isTrigger = true;
    }
    private void Update()
    {
        
        if( _collider.radius > _maxRadius )
        {
            _collider.enabled = false;
        }
        else
        {
            _collider.radius += Time.deltaTime * _speedExplosion;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IDamageable>(out var damageable))
        {
            
        }
    }
}
