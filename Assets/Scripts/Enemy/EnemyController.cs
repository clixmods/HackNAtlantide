using System;
using System.Collections;
using Attack;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class EnemyController : MonoBehaviour, ICombat
{
    public float damage;

    [SerializeField] private float attackRadius = 1f;
    [SerializeField] private float lookRadius = 10f;

    private bool _hasFinishAttack;
    
    bool _playerInLookRadius, _playerInAttackRadius;
    [SerializeField] LayerMask groundLayer, playerLayer;

    NavMeshAgent _agent;

    private Animator _animator;
    private IAttackCollider _attackCollider;
    private bool _isAttacking;
    public bool IsAttacking { get { return _isAttacking; } }

    private bool _canAttack;
    private bool _hasFinishWaitAttack = true;
    private static readonly int IsAwakeNameID = Animator.StringToHash("IsAwake");
    public event Action _attackEvent;
    Character character;

    public UnityEvent OnAwake;
    public UnityEvent OnSleep;

    #region Properties

    public bool canAttack
    {
        get { return _canAttack; }
        set
        {
            _canAttack = value;
            _isAttacking = canAttack;
            if (_isAttacking)
            {
                _attackEvent?.Invoke();
            };

        }
    }
    public bool IsAwake => _playerInLookRadius;
    

    #endregion
    private void Awake()
    {
        character = GetComponent<Character>();
        _attackCollider = GetComponentInChildren<IAttackCollider>();
        if (_attackCollider != null)
        {
            _attackCollider.OnCollideWithIDamageable += AttackColliderOnOnCollideWithIDamageable;
        }
        else
        {
            Debug.LogError("No attackCollider find, this enemy can't attack.", gameObject);
        }

        _hasFinishAttack = true;
    }

    private void AttackColliderOnOnCollideWithIDamageable(object sender, EventArgs eventArgs)
    {
        if( eventArgs is AttackDamageableEventArgs mDamageableEventArgs && canAttack)
        {
            mDamageableEventArgs.idamageable.DoDamage(damage);
        }
    }

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        _playerInLookRadius = PlayerIsInLookRadius();
        _playerInAttackRadius = Physics.CheckSphere(transform.position, attackRadius, playerLayer);

        if (_playerInLookRadius) _animator.SetBool(IsAwakeNameID, true);
        if (!_playerInLookRadius) _animator.SetBool(IsAwakeNameID, false);
        if (_playerInLookRadius && !_playerInAttackRadius && !_isAttacking && _hasFinishAttack) Chase();
        if (_playerInLookRadius && (_playerInAttackRadius || _isAttacking) && _hasFinishWaitAttack) StartCoroutine(Attack());
        
        //for animation
        character.CurrentSpeed = _agent.velocity.magnitude;
    }

    private bool PlayerIsInLookRadius()
    {
        bool value = Physics.CheckSphere(transform.position, lookRadius, playerLayer);
        if (_playerInLookRadius != value)
        {
            if (value) // Awake
            {
                Debug.Log("Awake", gameObject);
                OnAwake?.Invoke();
            }
            else // Sleep
            {
                Debug.Log("Sleep", gameObject);
                OnSleep?.Invoke();
            }
        }
            
        return value;
    }
    
    void Chase()
    {
        _agent.isStopped = false;
        _agent.SetDestination(PlayerInstanceScriptableObject.Player.transform.position);
    }
    
    IEnumerator Attack()
    {
        _hasFinishWaitAttack = false;
        _hasFinishAttack = false;
        if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Attaque_1_Golem"))
        {
            _animator.SetTrigger("Attack");
            _agent.isStopped = true;
        }
        FaceTarget();

        yield return new WaitForSeconds(1f);

        FaceTarget();
        _hasFinishAttack = true;
        _animator.SetBool(IsAwakeNameID, false);

        yield return new WaitForSeconds(1f);

        _hasFinishWaitAttack = true;
        _animator.SetBool(IsAwakeNameID, true);

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

    
}