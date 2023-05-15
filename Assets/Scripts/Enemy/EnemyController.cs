using System;
using System.Collections;
using Attack;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class EnemyController : MonoBehaviour, ICombat
{
    public float damage;

    [SerializeField] private float attackRadius = 1f;
    [SerializeField] private float lookRadius = 10f;
    
    bool _playerInLookRadius, _playerInAttackRadius;
    [SerializeField] LayerMask groundLayer, playerLayer;

    NavMeshAgent _agent;

    private Animator _animator;
    private IAttackCollider _attackCollider;
    private bool _isAttacking;
    private Vector3 forceDiffMove;
    public Vector3 ForceDiffMove { get; set; }
    private float reduceForce = 1f;
    public bool IsAttacking { get { return _isAttacking; } }
    private Vector3 destination;
    private void Awake()
    {
        _attackCollider = GetComponentInChildren<IAttackCollider>();
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
        if( eventArgs is AttackDamageableEventArgs mDamageableEventArgs && canAttack)
        {
            mDamageableEventArgs.idamageable.DoDamage(damage, _attackCollider.gameObject.transform.position);
        }
    }

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        _agent.SetDestination(destination + forceDiffMove);
        Debug.Log(forceDiffMove.magnitude);
        _playerInLookRadius = Physics.CheckSphere(transform.position, lookRadius, playerLayer);
        _playerInAttackRadius = Physics.CheckSphere(transform.position, attackRadius, playerLayer);
        
        if (_playerInLookRadius && !_playerInAttackRadius) Chase();
        if (_playerInLookRadius && _playerInAttackRadius)
        {
            Attack();
        }

        float magnitude = forceDiffMove.magnitude;
        forceDiffMove = (magnitude - Time.deltaTime * reduceForce) * forceDiffMove;
        if (magnitude < 0.001f)
        {
            forceDiffMove = Vector3.zero;
        }
    }
    
    void Chase()
    {
        destination = PlayerInstanceScriptableObject.Player.transform.position;
    }
    
    private void Attack()
    {
        FaceTarget();
        if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Enemy_attack"))
        {
            _animator.SetTrigger("Attack");
            destination = transform.position;
        }
    }
    
    private void FaceTarget()
    {
        Vector3 direction = (PlayerInstanceScriptableObject.Player.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5);
    }
    #region Animation Event Methods

    public void SetDamageActive(int value)
    {
        canAttack = value == 1;
        _attackCollider.enabled = canAttack; 
       
    }
    #endregion
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        var position = transform.position;
        Gizmos.DrawWireSphere(position, lookRadius);

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(position, attackRadius);
    }

    private bool _canAttack;
    public event Action _attackEvent;
    public bool canAttack { 
        get { return _canAttack; } 
        set
        { 
            _canAttack = value;
            _isAttacking = canAttack; 
            if(_isAttacking)
            {
                _attackEvent?.Invoke();
            };
            
        } 
    }
}