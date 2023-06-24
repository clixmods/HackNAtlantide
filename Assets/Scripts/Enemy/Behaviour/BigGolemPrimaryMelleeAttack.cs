using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class BigGolemPrimaryMelleeAttack : EnemyAttackBehaviour
{

    int walkAnimID = Animator.StringToHash("Walk_Big_Golem");
    int AttackAnimID = Animator.StringToHash("Attack1_Big_Golem");
    [SerializeField] float attackDistance;
    public UnityEvent OnAttackDone;
    public override void Attack()
    {
        StartCoroutine(AttackBehaviour());
        LaunchAttackEvent();
        Priority += CoolDown;

    }
    IEnumerator AttackBehaviour()
    {
        if (!_enemyBehaviour.Animator.GetCurrentAnimatorStateInfo(0).IsName("Walk_Big_Golem"))
        {
            _enemyBehaviour.Animator.CrossFadeInFixedTime(walkAnimID, 0f);
        }
        Vector3 destination = PlayerInstanceScriptableObject.Player.transform.position + (transform.position - PlayerInstanceScriptableObject.Player.transform.position).normalized * attackDistance;
        //Run in the direction until at finish point
        _enemyBehaviour.Agent.speed *= 3f;
        float timeMaxToPosition = 1.5f;
        while ((transform.position - destination).ProjectOntoPlane(Vector3.up).sqrMagnitude > 2f && timeMaxToPosition > 0f)
        {
            timeMaxToPosition -= Time.deltaTime;
            Debug.Log(timeMaxToPosition);
            destination = PlayerInstanceScriptableObject.Player.transform.position + (transform.position - PlayerInstanceScriptableObject.Player.transform.position).normalized * attackDistance;
            _enemyBehaviour.Agent.SetDestination(destination);
            _enemyBehaviour.FaceTarget(destination);
            yield return null;
        }
        if (!_enemyBehaviour.Animator.GetCurrentAnimatorStateInfo(0).IsName("Attack1_Big_Golem"))
        {
            _enemyBehaviour.Animator.CrossFadeInFixedTime(AttackAnimID, 0f);
        }
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
        _enemyBehaviour.Agent.speed /= 3f;
        _enemyBehaviour.IsAttacking = false;
        _enemyBehaviour.Animator.CrossFadeInFixedTime(walkAnimID, 0.2f);
    }

    public override bool CanAttack()
    {
        return _enemyBehaviour.DistanceWithPlayer > MinDistanceToAttack && _enemyBehaviour.DistanceWithPlayer < MaxDistanceToAttack;
    }
    public void AttackDone()
    {
        OnAttackDone?.Invoke();
    }
    public override void CancelAttack()
    {
        StopCoroutine(AttackBehaviour());
        OnAttackFinished();
        _enemyBehaviour.Agent.speed /= 3f;
        _enemyBehaviour.IsAttacking = false;
        _enemyBehaviour.Animator.CrossFadeInFixedTime(walkAnimID, 0.2f);

    }
}
