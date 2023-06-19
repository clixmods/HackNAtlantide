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
    int MovingBackAnimID = Animator.StringToHash("Walk_Big_Golem");
    int ChargingAnimID = Animator.StringToHash("Ray_Attack_Big_Golem");
    int AttackRunAnimID = Animator.StringToHash("Run_Big_Golem");
    int AttackRunCloseToPlayerAnimID = Animator.StringToHash("Transition_Run_To_Charge_Big_Golem");
    int WalkAnimID = Animator.StringToHash("Walk_Big_Golem");


    [SerializeField] float chargeDistance = 10;
    [SerializeField] float attackBlockDistance = 3;
    [SerializeField] float chargeSpeed = 10;
    [SerializeField] float AttackMovementSpeed = 20;
    [SerializeField] float chargeTime = 1.5f;
    float initialSpeed;


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
        _enemyBehaviour.Animator.CrossFadeInFixedTime(MovingBackAnimID, 0.2f);

    }
    public IEnumerator MoveToChargePosition()
    {
        OnStartMovingBack?.Invoke();
        initialSpeed = _enemyBehaviour.Agent.speed;
        _enemyBehaviour.Agent.speed = chargeSpeed;
        NavMeshHit destination;
        NavMesh.SamplePosition(chargePoint.position, out destination, 20f, NavMesh.AllAreas);
        while ((_enemyBehaviour.transform.position - destination.position).sqrMagnitude > 1.5f)
        {
            if(NavMesh.SamplePosition(chargePoint.position, out destination, 20f, NavMesh.AllAreas))
            {
                _enemyBehaviour.Agent.SetDestination(destination.position);
                _enemyBehaviour.FaceTarget(destination.position);
                Debug.Log("move to charge point");
            }
            yield return null;
        }
        StartCoroutine(Charge());
    }
    public IEnumerator Charge()
    {
        _enemyBehaviour.Agent.enabled = false;
        OnStartChargingAttack?.Invoke();
        _enemyBehaviour.Animator.CrossFadeInFixedTime(ChargingAnimID, 0.1f);
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
        //Setup collider
        _attackColliderAttackRun.enabled = true;
        _attackColliderAttackRun.OnCollideWithIDamageable += AttackColliderOnOnCollideWithIDamageable;
        OnAttackStarted();

        //setup agent
        _enemyBehaviour.Agent.isStopped = false;
        _enemyBehaviour.Agent.speed = AttackMovementSpeed;
        
        //animation AttackRun
        _enemyBehaviour.Animator.CrossFadeInFixedTime(AttackRunAnimID, 0.1f);

        //calculates direction to attack
        Vector3 directionToPlayer = -(transform.position - PlayerInstanceScriptableObject.Player.transform.position);
        directionToPlayer = new Vector3(directionToPlayer.x, transform.position.y, directionToPlayer.z);

        Vector3 directionToMove = directionToPlayer + directionToPlayer.normalized * 5f;
        Vector3 PointToStop = transform.position + directionToMove;
        if(NavMesh.SamplePosition(PointToStop, out NavMeshHit hitPoint, 10f, 1))
        {
            PointToStop = hitPoint.position;
        }

        //Run in the direction until at finish point
        while ((transform.position - PointToStop).sqrMagnitude > 5f)
        {
            //move to direction
            //transform.position += directionToMove.normalized * AttackMovementSpeed * Time.deltaTime;
            _enemyBehaviour.Agent.SetDestination(PointToStop);

            if(!_enemyBehaviour.Animator.GetCurrentAnimatorStateInfo(0).IsName("Transition_Run_To_Charge_Big_Golem") 
                && !_enemyBehaviour.Animator.GetCurrentAnimatorStateInfo(0).IsName("Charge_Attack_Big_Golem")
                && (transform.position - PlayerInstanceScriptableObject.Player.transform.position).sqrMagnitude < attackBlockDistance * attackBlockDistance)
            {
                _enemyBehaviour.Animator.CrossFadeInFixedTime(AttackRunCloseToPlayerAnimID, 0f);
            }
            yield return null;
            
        }
        //arrived at end position so set variables back to default
        _rigidBody.velocity = Vector3.zero;
        _enemyBehaviour.Agent.speed = initialSpeed;
        _enemyBehaviour.Animator.CrossFadeInFixedTime(WalkAnimID, 1f);

        OnAttackFinished();
        _enemyBehaviour.IsAttacking = false;
        yield return null;

        _attackColliderAttackRun.OnCollideWithIDamageable -= AttackColliderOnOnCollideWithIDamageable;
        _attackColliderAttackRun.enabled = false;
    }

    public override bool CanAttack()
    {
        Vector3 desiredPosition = PlayerInstanceScriptableObject.Player.transform.position + (transform.position - PlayerInstanceScriptableObject.Player.transform.position).normalized * chargeDistance;
        desiredPosition = new Vector3(desiredPosition.x, transform.position.y, desiredPosition.z);
        bool canMoveBack = NavMesh.SamplePosition(desiredPosition, out chargePoint, 10f, NavMesh.AllAreas);
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
        Gizmos.DrawSphere(chargePoint.position, 1f);
    }
}
