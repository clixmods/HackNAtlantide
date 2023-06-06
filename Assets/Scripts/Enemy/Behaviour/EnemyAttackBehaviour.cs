using Attack;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class EnemyAttackBehaviour : MonoBehaviour, ICombat
{
    #region properties
    [SerializeField] float damage;
    public float Damage { get { return damage; } }

    [SerializeField] private float _minPriority;
    public float MinPriority { get { return _minPriority; } }
    private float _currentPriority;
    public float Priority { get { return _currentPriority; } set { _currentPriority = value; } }

    [SerializeField] private float _coolDown;
    public float CoolDown { get { return _coolDown; } }

    [SerializeField] private float _minDistanceToAttack = 0f;
    public float MinDistanceToAttack { get { return _minDistanceToAttack; } }
    [SerializeField] private float _maxDistanceToAttack = 3f;
    public float MaxDistanceToAttack { get { return _maxDistanceToAttack; } }

    [SerializeField] private bool _facePlayer;
    public bool FacePlayer { get { return _facePlayer; } set { _facePlayer = value; } }

    [SerializeField] public event Action OnAttack;
    public void LaunchAttackEvent()
    {
        OnAttack?.Invoke();
    }
    private bool _canAttack;
    public bool canGiveDamage
    {
        get { return _canAttack; }
        set
        {
            _canAttack = value;
            if (_canAttack)
            {
                OnAttack?.Invoke();
            };

        }
    }

    [SerializeField] private AttackCollider _attackColliderRight;
    public AttackCollider AttackColliderRight { get { return _attackColliderRight; } set { _attackColliderRight = value; } }
    [SerializeField] private AttackCollider _attackColliderLeft;
    public AttackCollider AttackColliderLeft { get { return _attackColliderLeft; } set { _attackColliderLeft = value; } }
    //Components

    protected EnemyBehaviour _enemyBehaviour;

    #endregion

    #region MonoBehaviour
    public virtual void Awake()
    {
        _enemyBehaviour = GetComponent<EnemyBehaviour>();
        _currentPriority = _minPriority;

    }

    public void OnAttackStarted()
    {
        if (_attackColliderRight != null)
        {
            _attackColliderRight.OnCollideWithIDamageable += AttackColliderOnOnCollideWithIDamageable;
        }
        if (_attackColliderLeft != null)
        {
            _attackColliderLeft.OnCollideWithIDamageable += AttackColliderOnOnCollideWithIDamageable;
        }
    }
    public void OnAttackFinished()
    {
        if (_attackColliderRight != null)
        {
            _attackColliderRight.OnCollideWithIDamageable -= AttackColliderOnOnCollideWithIDamageable;
        }
        if (_attackColliderLeft != null)
        {
            _attackColliderLeft.OnCollideWithIDamageable -= AttackColliderOnOnCollideWithIDamageable;
        }
    }

    private void AttackColliderOnOnCollideWithIDamageable(object sender, EventArgs eventArgs)
    {
        if (eventArgs is AttackDamageableEventArgs mDamageableEventArgs && canGiveDamage)
        {
            mDamageableEventArgs.idamageable.DoDamage(damage);
        }
    }
    #endregion
    public abstract bool CanAttack();
    public abstract void Attack();
    public IEnumerator RechargePriority()
    {
        while (_currentPriority > _minPriority)
        {
            _currentPriority -= Time.deltaTime;
            yield return null;
        }
        _currentPriority = _minPriority;
    }
    #region Animation Event Methods
    public void SetDamageActive(int value)
    {
        canGiveDamage = (value == 1|| value == 2);
        if(_attackColliderLeft != null)
            _attackColliderLeft.enabled = value == 2;

        if(_attackColliderRight != null)
            _attackColliderRight.enabled = canGiveDamage;
    }
    #endregion
}
