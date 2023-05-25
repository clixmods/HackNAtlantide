using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallGolemPrimaryAttack : EnemyAttackBehaviour
{
    int AttackAnimID = Animator.StringToHash("Attack");
    public override void Attack()
    {
        StartCoroutine(AttackBehaviour());
        _onAttack?.Invoke();
        Priority += CoolDown*2;

        Debug.Log("attack");
        //_enemyBehaviour.Animator.SetTrigger(AttackAnimID);
    }
    IEnumerator AttackBehaviour()
    {
        float time = CoolDown/2f;
        while(time > 0)
        {
            if(FacePlayer)
            {
                _enemyBehaviour.FaceTarget(PlayerInstanceScriptableObject.Player.transform.position);
            }
            time -= Time.deltaTime;
            yield return null;
        }
    }

    public override bool CanAttack()
    {
        return _enemyBehaviour.DistanceWithPlayer > MinDistanceToAttack && _enemyBehaviour.DistanceWithPlayer < MaxDistanceToAttack;
    }
}
