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
