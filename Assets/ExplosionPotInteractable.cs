using Attack;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent (typeof(SphereCollider))]
[RequireComponent(typeof(AttackCollider))]
public class ExplosionPotInteractable : MonoBehaviour
{
    [SerializeField] float damageMax;
    [SerializeField] float radiusMax;
    [SerializeField] float explosionSpeed;
    AttackCollider _attackColliderExplosion;
    private void OnEnable()
    {
        _attackColliderExplosion = GetComponent<AttackCollider>();
        transform.localScale = Vector3.zero;
        StartCoroutine(Explosion());
    }
    private void AttackColliderOnOnCollideWithIDamageableExplosion(object sender, EventArgs eventArgs)
    {
        if (eventArgs is AttackDamageableEventArgs mDamageableEventArgs)
        {
            float damage = Mathf.Lerp(damageMax, 0, Mathf.Clamp(((transform.position - mDamageableEventArgs.idamageable.transform.position).magnitude / radiusMax), 0, 1));
            if (damage < 3 && damage > 2.4f) 
            {
                damage = 2.5f;
            }
            else if (damage <= 2.4f && damage > 1.9f)
            {
                damage = 2f;
            }
            else if (damage <= 1.9f && damage > 1.4f)
            {
                damage = 1.5f;
            }
            else if (damage <= 1.4f && damage > 0.9f)
            {
                damage = 1f;
            }
            else if (damage <= 0.9f && damage > 0.4f)
            {
                damage = 0.5f;
            }
            else
            {
                return;
            }
            //ZEBI
            mDamageableEventArgs.idamageable.DoDamage(damage);
        }
    }
    IEnumerator Explosion()
    {
        while (transform.localScale.x < radiusMax)
        {
            _attackColliderExplosion.OnCollideWithIDamageable += AttackColliderOnOnCollideWithIDamageableExplosion;
            transform.localScale += Vector3.one * Time.deltaTime * explosionSpeed;
            yield return null;
            _attackColliderExplosion.OnCollideWithIDamageable -= AttackColliderOnOnCollideWithIDamageableExplosion;
        }
        Destroy(gameObject);
    }
}
