using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigGolemLaserAttack : EnemyAttackBehaviour
{
    int ChargeAnimID = Animator.StringToHash("Ray_Attack_Big_Golem");
    int WalkAnimID = Animator.StringToHash("Walk_Big_Golem");
    [SerializeField] float followSpeed;
    [SerializeField] float attackDuration;
    [SerializeField] GameObject head;
    [SerializeField] GameObject animMesh;
    [SerializeField] GameObject laserMesh;
    bool hasToUpdate = false;
    Quaternion currentRotation;

    public override void Attack()
    {
        StartCoroutine(AttackBehaviour());
        LaunchAttackEvent();
        Priority += CoolDown;
    }
    IEnumerator AttackBehaviour()
    {
        OnAttackStarted();
        _enemyBehaviour.Animator.CrossFade(ChargeAnimID, 0f);
        yield return new WaitForEndOfFrame();
        while(_enemyBehaviour.Animator.GetCurrentAnimatorStateInfo(0).IsName("Ray_Attack_Big_Golem"))
        {
            Debug.Log("inRayAttackAnim");
            yield return null;
        }
        animMesh.SetActive(false);
        laserMesh.SetActive(true);
        float time = attackDuration;
        while (time > 0)
        {
            //_enemyBehaviour.FaceTarget(PlayerInstanceScriptableObject.Player.transform.position);
            //RotateHead
            Vector3 playerPos = PlayerInstanceScriptableObject.Player.transform.position + Vector3.up * 0.3f;
            Quaternion _targetRotation = Quaternion.LookRotation(playerPos - head.transform.position, Vector3.up);
            head.transform.rotation = Quaternion.Slerp(head.transform.rotation, _targetRotation, followSpeed * Time.deltaTime);
            time -= Time.deltaTime;
            yield return null;
        }
        animMesh.SetActive(true);
        laserMesh.SetActive(false);
        OnAttackFinished();
        _enemyBehaviour.IsAttacking = false;
        //_enemyBehaviour.Animator.CrossFade(WalkAnimID, 0.2f);
    }
    public override bool CanAttack()
    {
        return _enemyBehaviour.DistanceWithPlayer > MinDistanceToAttack && _enemyBehaviour.DistanceWithPlayer < MaxDistanceToAttack;
    }
}
