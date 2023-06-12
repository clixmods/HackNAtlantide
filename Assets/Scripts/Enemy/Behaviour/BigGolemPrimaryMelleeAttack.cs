using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigGolemPrimaryMelleeAttack : EnemyAttackBehaviour
{
    int AttackAnimID = Animator.StringToHash("Attaque_1_Golem");
    public override void Attack()
    {
        StartCoroutine(AttackBehaviour());
        LaunchAttackEvent();
        Priority += CoolDown;
        if (!_enemyBehaviour.Animator.GetCurrentAnimatorStateInfo(0).IsName("Attaque_1_Golem"))
        {
            _enemyBehaviour.Animator.CrossFadeInFixedTime(AttackAnimID, 0f);
        }

    }
    IEnumerator AttackBehaviour()
    {
        OnAttackStarted();
        float time = CoolDown;
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
