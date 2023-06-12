using Attack;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.UI;

public class BigGolemRunAttack : EnemyAttackBehaviour
{
    int MovingBackAnimID = Animator.StringToHash("");
    int ChargingAnimID = Animator.StringToHash("");
    int AttackRunAnimID = Animator.StringToHash("");
    int AttackRunCloseToPlayerAnimID = Animator.StringToHash("");


    [SerializeField] float chargeDistance = 10;
    [SerializeField] float attackBlockDistance = 3;
    [SerializeField] float chargeSpeed = 10;
    [SerializeField] float AttackMovementSpeed = 20;
    [SerializeField] float chargeTime = 1.5f;


    public UnityEvent OnStartMovingBack;
    public UnityEvent OnStartChargingAttack;
    public UnityEvent OnStartAttackRun;
    public UnityEvent OnAttackFinish;

    [SerializeField] private AttackCollider _attackColliderAttackRun;
    Rigidbody _rigidBody;

    private NavMeshHit chargePoint;
    private void OnEnable()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }
    public override void Attack()
    {
        
        StopAllCoroutines();
        StartCoroutine(MoveToChargePosition());

        //LaunchAttackEvent();
        Priority += CoolDown;
        if (!_enemyBehaviour.Animator.GetCurrentAnimatorStateInfo(0).IsName(""))
        {
            //_enemyBehaviour.Animator.CrossFadeInFixedTime(MovingBackAnimID, 0f);
        }

    }
    public IEnumerator MoveToChargePosition()
    {
        OnStartMovingBack?.Invoke();
        _enemyBehaviour.Agent.enabled = false;

        while ((_enemyBehaviour.transform.position - chargePoint.position).sqrMagnitude > 1.5f)
        {
            transform.position += (chargePoint.position - transform.position).normalized * chargeSpeed * Time.deltaTime;
            _enemyBehaviour.FaceTarget(chargePoint.position);
            yield return null;
        }
        StartCoroutine(Charge());
    }
    public IEnumerator Charge()
    { 
        OnStartChargingAttack?.Invoke();
        //_enemyBehaviour.Animator.CrossFadeInFixedTime(ChargingAnimID, 0.2f);
        float timeToCharge = chargeTime;

        while(timeToCharge > 0)
        {
            timeToCharge -= Time.deltaTime;
            _enemyBehaviour.FaceTarget(PlayerInstanceScriptableObject.Player.transform.position);
            yield return null;
        }

        OnStartChargingAttack?.Invoke();
        _enemyBehaviour.Agent.enabled = true;
        StartCoroutine(AttackBehaviour());

    }
    IEnumerator AttackBehaviour()
    {
        _attackColliderAttackRun.enabled = true;
        _attackColliderAttackRun.OnCollideWithIDamageable += AttackColliderOnOnCollideWithIDamageable;
        OnAttackStarted();
        _enemyBehaviour.Agent.isStopped = false;
        float initialSpeed = _enemyBehaviour.Agent.speed;
        
        
        //_enemyBehaviour.Animator.CrossFadeInFixedTime(AttackRunAnimID, 0.2f);

        Vector3 directionToPlayer = -(transform.position - PlayerInstanceScriptableObject.Player.transform.position);
        directionToPlayer = new Vector3(directionToPlayer.x, transform.position.y, directionToPlayer.z);

        Vector3 directionToMove = directionToPlayer + directionToPlayer.normalized * 10f;
        Vector3 PointToStop = transform.position + directionToMove;
        if(NavMesh.SamplePosition(PointToStop, out NavMeshHit hitPoint, 10f, 1))
        {
            PointToStop = hitPoint.position;
        }

        while ((transform.position - PointToStop).sqrMagnitude > 5f)
        {
            transform.position += directionToMove.normalized * AttackMovementSpeed * Time.deltaTime;

            if((transform.position - PlayerInstanceScriptableObject.Player.transform.position).sqrMagnitude < attackBlockDistance*attackBlockDistance)
            {
                //_enemyBehaviour.Animator.CrossFadeInFixedTime(AttackRunCloseToPlayerAnimID, 0.2f);
            }
            yield return null;
            
        }
        _rigidBody.velocity = Vector3.zero;
        _enemyBehaviour.Agent.speed = initialSpeed;
        OnAttackFinished();
        yield return null;
        _attackColliderAttackRun.OnCollideWithIDamageable -= AttackColliderOnOnCollideWithIDamageable;
        _attackColliderAttackRun.enabled = false;
    }

    public override bool CanAttack()
    {
        Vector3 desiredPosition = PlayerInstanceScriptableObject.Player.transform.position + (transform.position - PlayerInstanceScriptableObject.Player.transform.position).normalized * chargeDistance;
        desiredPosition = new Vector3(desiredPosition.x, transform.position.y, desiredPosition.z);
        bool canMoveBack = NavMesh.SamplePosition(desiredPosition, out chargePoint, 3f, 1);
        return _enemyBehaviour.DistanceWithPlayer > MinDistanceToAttack && _enemyBehaviour.DistanceWithPlayer < MaxDistanceToAttack && canMoveBack;
    }
    private void AttackColliderOnOnCollideWithIDamageable(object sender, EventArgs eventArgs)
    {
        if (eventArgs is AttackDamageableEventArgs mDamageableEventArgs)
        {
            mDamageableEventArgs.idamageable.DoDamage(base.Damage);
        }
    }
    private void OnDrawGizmosSelected()
    {
        if(chargePoint.position != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(chargePoint.position, 1f);
            Gizmos.color = Color.yellow;
            Vector3 desiredPosition = PlayerInstanceScriptableObject.Player.transform.position + (transform.position - PlayerInstanceScriptableObject.Player.transform.position).normalized * chargeDistance;
            desiredPosition = new Vector3(desiredPosition.x, transform.position.y, desiredPosition.z);
            Gizmos.DrawLine(transform.position, desiredPosition);
        }
    }
}
