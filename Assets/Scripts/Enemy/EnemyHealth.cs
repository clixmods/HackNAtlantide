using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class EnemyHealth : Character
{
    [SerializeField] EnemyController _enemyController;
    public override void Dead()
    {
        base.Dead();
        Destroy(transform.gameObject);
    }
}
