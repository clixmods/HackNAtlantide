using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    ICombat _iCombat;
    public float damage;

    AnimationEvent _damageActiveAnimatorEvent;

    private void Start()
    {
        _iCombat = GetComponentInParent<ICombat>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(_iCombat.canAttack && other.TryGetComponent<IDamageable>(out var damageable))
        {
            damageable.TakeDamage(damage);
        }
    }
}