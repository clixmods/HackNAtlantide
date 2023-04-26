using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] PlayerCombat _playerCombat;
    public float damage;

    BoxCollider _triggerBox;

    AnimationEvent _damageActiveAnimatorEvent;

    private void Start()
    {
        _triggerBox= GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(_playerCombat.CanGiveDamage && other.TryGetComponent<IDamageable>(out var damageable))
        {
            damageable.TakeDamage(damage);
        }
    }

    public void EnableTriggerBox()
    {
        _triggerBox.enabled = true;
    }

    public void DisableTriggerBox()
    {
        _triggerBox.enabled = false;
    }
}