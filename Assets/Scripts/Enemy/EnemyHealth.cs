using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class EnemyHealth : Character
{
    // [SerializeField] EnemyController _enemyController;
    // [SerializeField] ParticleSystem _particleDead;
    // [SerializeField] DeathEnemyEffect _deathEnemyEffect;
    [SerializeField] ScriptableValueListGameObject _allEnemyAwake;
    public override void Dead()
    {
        base.Dead();

        //   _particleDead.transform.parent = null;
        //   _particleDead.Play();
        // _deathEnemyEffect.DeadEffect();
        _allEnemyAwake.RemoveUnique(this.gameObject);
        Destroy(gameObject);
    }
    public override void DoDamage(float damage, Vector3 attackLocation)
    {
        base.DoDamage(damage, attackLocation);
        if(TryGetComponent<EnemyWakeUpBehaviour>(out EnemyWakeUpBehaviour wakeUpBehaviour))
        {
            wakeUpBehaviour.WakeUp();
        }
    }
}