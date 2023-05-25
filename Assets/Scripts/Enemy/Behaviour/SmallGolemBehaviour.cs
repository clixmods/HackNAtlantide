using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallGolemBehaviour : EnemyBehaviour
{
    int MoveAnimID = Animator.StringToHash("Walk_Golem");
    int IdleAnimID = Animator.StringToHash("Idle_Golem");
    private void Start()
    {
        StartCoroutine(Attack());
    }
    public override void Move(Vector3 target)
    {
        base.Move(target);
        if(!Animator.GetCurrentAnimatorStateInfo(0).IsName("Walk_Golem"))
        {
            //Animator.CrossFadeInFixedTime(MoveAnimID, 0f);
        }

    }
    public override void WakeUp()
    {
        base.WakeUp();
        Animator.CrossFadeInFixedTime("Walk_Golem", 0f);
    }
}
