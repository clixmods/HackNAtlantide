using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDamageKnockback : MonoBehaviour
{
    private IDamageable _iDamageable;
    private Animator _animator;
    private EnemyController _enemyController;

    private void OnEnable()
    {
        _iDamageable = GetComponent<IDamageable>();
        _iDamageable.OnDamage += KnockBack;
        _animator = GetComponent<Animator>();
        _enemyController = GetComponent<EnemyController>();
    }

    private void KnockBack(object sender, EventArgs eventArgs)
    {
        _animator.enabled = false;
        _animator.Play("Enemy_Walking");
        if( eventArgs is DoDamageEventArgs doDamageEventArgs )
        {
            Vector3 direction = (transform.position - doDamageEventArgs.attackPosition).normalized;
            _enemyController.ForceDiffMove += direction * 100f;
        }
        _animator.enabled = true;
        Debug.Log("onDamageKBAnim is going trought");
    }

    private void OnDisable()
    {
        _iDamageable.OnDamage -= KnockBack;
    }
}
