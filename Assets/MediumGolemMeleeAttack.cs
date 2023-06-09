using Attack;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MediumGolemMeleeAttack : EnemyAttackBehaviour
{
    int AttackAnimID = Animator.StringToHash("Attack_Golem_M");
    public override void Attack()
    {
        StartCoroutine(AttackBehaviour());
        LaunchAttackEvent();
        Priority += CoolDown;
        if (!_enemyBehaviour.Animator.GetCurrentAnimatorStateInfo(0).IsName("Attack_Golem_M"))
        {
            _enemyBehaviour.Animator.CrossFadeInFixedTime(AttackAnimID, 0f);
        }

    }
    IEnumerator AttackBehaviour()
    {
        OnAttackStarted();
        float time = CoolDown / 2f;
        while (time > 0)
        {
            if (FacePlayer)
            {
                _enemyBehaviour.FaceTarget(PlayerInstanceScriptableObject.Player.transform.position);
            }
            time -= Time.deltaTime;
            yield return null;
        }
        OnAttackFinished();
    }
    public override bool CanAttack()
    {
        return _enemyBehaviour.DistanceWithPlayer > MinDistanceToAttack && _enemyBehaviour.DistanceWithPlayer < MaxDistanceToAttack;
    }
}
