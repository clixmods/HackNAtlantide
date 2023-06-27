using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class EnemyHealth : Character
{
    [SerializeField] ScriptableValueListGameObject _allEnemyAwake;
    [SerializeField] private bool wakeUpOnDamage = true;
    [SerializeField] private bool destroyOnDeath = true;

    public void SetWakeUpOnDamage(bool value)
    {
        wakeUpOnDamage = value;
    }
    
    public override void Dead()
    {
        base.Dead();

        _allEnemyAwake.RemoveUnique(this.gameObject);
        if(destroyOnDeath)
            Destroy(gameObject);
        
        
    }
    public override void DoDamage(float damage, Vector3 attackLocation)
    {
        base.DoDamage(damage, attackLocation);
        if(wakeUpOnDamage && TryGetComponent<EnemyWakeUpBehaviour>(out EnemyWakeUpBehaviour wakeUpBehaviour))
        {
            wakeUpBehaviour.WakeUp();
        }
    }
}