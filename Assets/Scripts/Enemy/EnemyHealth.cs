using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class EnemyHealth : Character
{
    [SerializeField] EnemyController _enemyController;
    [SerializeField] ParticleSystem _particleDead;
    public override void Dead()
    {
        base.Dead();
        
        _particleDead.transform.parent = null;
        _particleDead.Play();
        Destroy(gameObject);
    }
}
