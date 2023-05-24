using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallGolemPrimaryAttack : EnemyAttackBehaviour
{
    int AttackAnimID = Animator.StringToHash("Attack");
    public override void Attack()
    {
        StartCoroutine(AttackBehaviour());
        OnAttack?.Invoke();
        Priority += CoolDown*2;

        Debug.Log("attack");
        //EnemyBehaviour.Animator.SetTrigger(AttackAnimID);
        //EnemyBehaviour.Agent.isStopped = true;
    }
    IEnumerator AttackBehaviour()
    {
        float time = CoolDown/2f;
        while(time > 0)
        {
            if(FacePlayer)
            {
                EnemyBehaviour.FacePlayer();
            }
            time -= Time.deltaTime;
            yield return null;
        }
    }

    public override bool CanAttack()
    {
        float distanceWithPlayer = EnemyBehaviour.DistanceWithPlayer();

        return distanceWithPlayer > MinDistanceToAttack && distanceWithPlayer < MaxDistanceToAttack;
    }
}
