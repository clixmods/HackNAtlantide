using Attack;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class BigGolemJumpAttack : EnemyAttackBehaviour
{
    int JumpAnimID = Animator.StringToHash("Jump_Big_Golem");
    //int MidAirAnimID = Animator.StringToHash("MidAir_Golem");
    int AttaqueAnimID = Animator.StringToHash("Fall_Big_Golem");
    [SerializeField] float damageOnExplosion = 1f;

    [SerializeField] float _jumpForce;
    [SerializeField] float jumpHorizontalSpeed;
    //bezier trajectory
    [SerializeField] BezierCurveGenerate bezierCurve;
    [SerializeField] Transform startPosition;
    [SerializeField] Transform highPoint1;
    [SerializeField] Transform highPoint2;
    [SerializeField] Transform endPoint;
    [SerializeField] float jumpTime = 2f;
    [SerializeField] float jumpAnimTime = 0.5f;

    [SerializeField] float explosionTime = 0.5f;
    [SerializeField] float explosionRadius = 5f;
    [SerializeField] GameObject explosionObject;
    [SerializeField] UnityAction OnExplosionStart;
    [SerializeField] private AttackCollider _attackColliderExplosion;
    [SerializeField] ParticleSystem ExplosionFx;
    SphereCollider explosionCollider;
    [SerializeField] GameObject groundCrackDecal;
    [SerializeField] CameraShakeScriptableObject attackLandingShake;

    public override void Attack()
    {
        CalculatePath();
        StartCoroutine(AttackBehaviour());
        LaunchAttackEvent();
        Priority += CoolDown;
        _enemyBehaviour.Agent.enabled = false;
    }

    public void GoToNewPositionWithJump(Transform destinationTransform)
    {
        CalculatePathTo(destinationTransform.position);
        StartCoroutine(AttackBehaviour());
        LaunchAttackEvent();
        Priority += CoolDown;
        _enemyBehaviour.Agent.enabled = false;
    }
    private void Start()
    {

        explosionCollider = explosionObject.GetComponent<SphereCollider>();
        explosionCollider.radius = 0f;
        ExplosionFx.transform.localScale = Vector3.zero;

        //listenToEventExplosionDamage

    }

    IEnumerator AttackBehaviour()
    {
        OnAttackStarted();
        // joue le jump anim et attend le bon moment pour sauter
        _enemyBehaviour.Animator.CrossFadeInFixedTime(JumpAnimID, 0f);
        yield return new WaitForSeconds(jumpAnimTime);

        //se deplace sur la courbe bezier et joue les anims qui faut
        float timeToJump = 0f;
        while (timeToJump < jumpTime)
        {
            if (FacePlayer)
            {
                _enemyBehaviour.FaceTarget(PlayerInstanceScriptableObject.Player.transform.position);
            }
            timeToJump += Time.deltaTime;
            if (timeToJump > 2f)
                timeToJump = 2f;

            float interpolationPosition = timeToJump / jumpTime;
            transform.position = bezierCurve.GetPointBezierCurve(interpolationPosition);

            //Joue l'attack animation a partir de 60% du saut
            if (!_enemyBehaviour.Animator.GetCurrentAnimatorStateInfo(0).IsName("Landing_Big_Golem") && interpolationPosition > 0.6f)
            {
                _enemyBehaviour.Animator.CrossFadeInFixedTime(AttaqueAnimID, 0f);
            }

            yield return null;
        }
        _enemyBehaviour.Agent.enabled = true;
        attackLandingShake.ShakeByDistance(_enemyBehaviour.DistanceWithPlayer / 10f);
        OnAttackFinished();

        //launchExplosion
        StartCoroutine(ExplosionAttack());
    }

    IEnumerator ExplosionAttack()
    {
        _attackColliderExplosion.enabled = true;
        _attackColliderExplosion.OnCollideWithIDamageable += AttackColliderOnOnCollideWithIDamageableExplosion;

        Instantiate(groundCrackDecal, transform.position + Vector3.up * 2, Quaternion.identity);


        OnExplosionStart?.Invoke();

        ExplosionFx.Play();

        float time = 0f;
        while (time < explosionTime)
        {
            time += Time.deltaTime;
            explosionCollider.radius = Mathf.Lerp(0, explosionRadius, time / explosionTime);
            ExplosionFx.transform.localScale = explosionCollider.radius * Vector3.one;
            yield return null;
        }
        explosionCollider.radius = 0f;
        ExplosionFx.transform.localScale = Vector3.zero;

        _attackColliderExplosion.OnCollideWithIDamageable -= AttackColliderOnOnCollideWithIDamageableExplosion;
        _attackColliderExplosion.enabled = false;
        _enemyBehaviour.IsAttacking = false;
    }

    private void AttackColliderOnOnCollideWithIDamageableExplosion(object sender, EventArgs eventArgs)
    {
        if (eventArgs is AttackDamageableEventArgs mDamageableEventArgs)
        {
            float damage = Mathf.Lerp(damageOnExplosion, 0, Mathf.Clamp(((transform.position - mDamageableEventArgs.idamageable.transform.position).magnitude / explosionRadius), 0, 1));
            //ZEBI
            mDamageableEventArgs.idamageable.DoDamage(damage);
        }
    }
    void CalculatePath()
    {
        startPosition.parent = null;
        startPosition.position = transform.position;
        endPoint.position = PlayerInstanceScriptableObject.Player.transform.position + new Vector3(Random.value, 0, Random.value) * 2;
        highPoint1.position = transform.position + (endPoint.position - transform.position) / 4f + Vector3.up * 10f;
        highPoint2.position = transform.position + 3 * (endPoint.position - transform.position) / 4f + Vector3.up * 10f;

    }

    private void CalculatePathTo(Vector3 position)
    {
        startPosition.parent = null;
        startPosition.position = transform.position;
        endPoint.position = position + new Vector3(Random.value, 0, Random.value) * 2;
        highPoint1.position = transform.position + (endPoint.position - transform.position) / 4f + Vector3.up * 10f;
        highPoint2.position = transform.position + 3 * (endPoint.position - transform.position) / 4f + Vector3.up * 10f;
    }

    public override bool CanAttack()
    {
        return _enemyBehaviour.DistanceWithPlayer > MinDistanceToAttack
            && _enemyBehaviour.DistanceWithPlayer < MaxDistanceToAttack
            && NavMesh.SamplePosition(PlayerInstanceScriptableObject.Player.transform.position, out NavMeshHit hit, 1f, 1);
    }
}
