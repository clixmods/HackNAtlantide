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
    public override void Update()
    {
        if (_isAwake && Agent.enabled)
        {
            _distanceWithPlayer = GetPathLength();
        }
        else
        {
            _distanceWithPlayer = float.MaxValue;
        }
        if (_isAwake)
        {
            float walkSpeed = Mathf.Lerp(0, 1, Agent.velocity.magnitude / 3);
            Animator.SetFloat("Walk_Speed", walkSpeed);
        }
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
        onAwake?.Invoke();
        _focusable.IsTargetable = !GetComponent<Character>().IsInvulnerable;
        StartCoroutine(MoveToPlayer());
        Animator.CrossFadeInFixedTime(MoveAnimID, 0f);
    }
}
