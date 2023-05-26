using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallGolemBehaviour : EnemyBehaviour
{
    int MoveAnimID = Animator.StringToHash("Walk_Golem");
    int IdleAnimID = Animator.StringToHash("Idle_Golem");
    int AwakeAnimID = Animator.StringToHash("WakeUp_Golem");
    private void Start()
    {
        StartCoroutine(Attack());
    }
    public override void Move(Vector3 target)
    {
        base.Move(target);
    }
    public override void WakeUp()
    {
        base.WakeUp();
    }
    public override IEnumerator WakeUpCoroutine()
    {
        Animator.CrossFadeInFixedTime(AwakeAnimID, 0f);
        yield return new WaitForSeconds(1f);
        IsAwake = true;
        StartCoroutine(MoveToPlayer());
        Animator.CrossFadeInFixedTime(MoveAnimID, 0f);
    }
}
