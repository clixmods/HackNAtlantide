using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MediumGolemBehaviour : EnemyBehaviour
{
    int MoveAnimID = Animator.StringToHash("Walk_Golem");
    int AwakeAnimID = Animator.StringToHash("Wake_Up_Golem_M");
    Rigidbody _rigidbody;
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        StartCoroutine(Attack());
    }
    private void FixedUpdate()
    {
        _rigidbody.AddForce(Vector3.down * 100f);
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
        yield return new WaitForSeconds(3f);
        IsAwake = true;
        StartCoroutine(MoveToPlayer());
        //Animator.CrossFadeInFixedTime(MoveAnimID, 0f);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = (DistanceWithPlayer > 25 && DistanceWithPlayer < 35) ? Color.blue : Color.yellow;
        Debug.DrawLine(transform.position, PlayerInstanceScriptableObject.Player.transform.position);
    }
}
