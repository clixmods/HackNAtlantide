using Attack;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BulletBehaviour : MonoBehaviour
{
    [SerializeField] float _speed;
    Vector3 direction;
    public Vector3 Direction { get { return direction; } set { direction = value; } }
    [SerializeField] float _damage;
    public float Damage { get { return _damage; } set { _damage = value; } }
    [SerializeField] AttackCollider _attackCollider;
    public UnityEvent OnHit;

    private void OnEnable()
    {
        _attackCollider.OnCollideWithIDamageable += AttackColliderOnOnCollideWithIDamageable;
    }
    private void Update()
    {
        transform.position += direction*_speed*Time.deltaTime;
	transform.forward = direction;
    }
    private void AttackColliderOnOnCollideWithIDamageable(object sender, EventArgs eventArgs)
    {
        if (eventArgs is AttackDamageableEventArgs mDamageableEventArgs)
        {
            mDamageableEventArgs.idamageable.DoDamage(_damage);
            OnHit?.Invoke();
            Destroy(gameObject);
        }
    }
}
