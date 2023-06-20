using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BigGolemBehaviour : EnemyBehaviour
{
    int MoveAnimID = Animator.StringToHash("Walk_Big_Golem");
    int AwakeAnimID = Animator.StringToHash("Wake_Up_Big_Golem");
    int walkSpeedId = Animator.StringToHash("Walk_Speed");
    public UnityEvent OnWalk;
    public override void Move(Vector3 target)
    {
        base.Move(target);
    }
    public override void WakeUp()
    {
        base.WakeUp();
    }
    public override void Update()
    {
        base.Update();
        Animator.SetFloat(walkSpeedId, Agent.velocity.magnitude / 10f);
        Debug.Log("distance with player : " + DistanceWithPlayer);
    }
    public override IEnumerator WakeUpCoroutine()
    {
        Animator.CrossFadeInFixedTime(AwakeAnimID, 0f);
        yield return new WaitForSeconds(3f);
        IsAwake = true;
        onAwake?.Invoke();
        _focusable.IsTargetable = !GetComponent<Character>().IsInvulnerable;
        StartCoroutine(MoveToPlayer());
        StartCoroutine(Attack());
        Animator.CrossFadeInFixedTime(MoveAnimID, 0f);
    }
    public override IEnumerator Attack()
    {
        while (!IsAttacking)
        {
            //trie Chaque Attack de l'ennemie par priorit?
            EnnemyAttacks = SortAttacksByPriority();

            //Selectione l'attaque disponible la plus prioritaire
            for (int i = 0; i < EnnemyAttacks.Count; i++)
            {
                if (EnnemyAttacks[i].CanAttack() && canTargetPlayer)
                {
                    EnnemyAttacks[i].Attack();
                    CurrentAttack = EnnemyAttacks[i];
                    IsAttacking = true;
                    _state = EnemyState.Attacking;
                    Agent.updateRotation = false;
                    break;
                }
            }

            yield return null;
        }
        _canMove = false;
        if (Agent.enabled)
        {
            Agent.SetDestination(transform.position);
        }
        //attends le coolDown de l'attaque qui est jou? pour commencer a rechercher une nouvelle attaque
        yield return new WaitForSeconds(CurrentAttack.CoolDown);
        while (IsAttacking)
        {
            Debug.Log("IsAttacking");
            yield return null;
        }
        CurrentAttack.StartCoroutine(CurrentAttack.RechargePriority());
        Debug.Log("attackFinished");
        Agent.updateRotation = true;
        CurrentAttack = null;

        //recommence a attaquer
        StartCoroutine(Attack());
        if (!_movecoroutineIsPlayed)
        {
            StartCoroutine(MoveToPlayer());
        }
    }
    public override IEnumerator MoveToPlayer()
    {
        _movecoroutineIsPlayed = true;
        _canMove = true;
        while (_canMove && _isAwake && !_returnToStartPos && Agent.enabled && canTargetPlayer)
        {
            Move(PlayerInstanceScriptableObject.Player.transform.position);
            FaceTarget(PlayerInstanceScriptableObject.Player.transform.position);
            yield return null;
        }
        _movecoroutineIsPlayed = false;

    }
    public void OnWalkFeedback()
    {
        if(Agent.velocity.magnitude / 10f > 0.1f)
        {
            OnWalk?.Invoke();
        }
    }

}
