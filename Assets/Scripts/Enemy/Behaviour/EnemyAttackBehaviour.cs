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

    [SerializeField] protected UnityEvent _onAttack;
    public UnityEvent OnAttack {get { return _onAttack; }}
    private bool _canAttack;
    public bool canAttack
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

    private IAttackCollider _attackCollider;

    //Components

    protected EnemyBehaviour _enemyBehaviour;

    #endregion

    #region MonoBehaviour
    private void Awake()
    {
        _enemyBehaviour = GetComponent<EnemyBehaviour>();
        _currentPriority = _minPriority;
        _attackCollider = GetComponentInChildren<AttackCollider>();
        if (_attackCollider != null)
        {
            _attackCollider.OnCollideWithIDamageable += AttackColliderOnOnCollideWithIDamageable;
        }
        else
        {
            Debug.LogError("No attackCollider find, this enemy can't attack.", gameObject);
        }
    }

    private void AttackColliderOnOnCollideWithIDamageable(object sender, EventArgs eventArgs)
    {
        if (eventArgs is AttackDamageableEventArgs mDamageableEventArgs && canAttack)
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
        canAttack = value == 1;
        _attackCollider.enabled = canAttack;

    }
    #endregion
}
