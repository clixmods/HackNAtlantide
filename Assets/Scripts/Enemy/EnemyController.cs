using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class EnemyController : MonoBehaviour,ICombat
{
    public float damage;

    [SerializeField] private float attackRadius = 1f;
    [SerializeField] private float lookRadius = 10f;
    
    bool _playerInLookRadius, _playerInAttackRadius;
    [SerializeField] LayerMask groundLayer, playerLayer;

    NavMeshAgent _agent;

    private Animator _animator;

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        _playerInLookRadius = Physics.CheckSphere(transform.position, lookRadius, playerLayer);
        _playerInAttackRadius = Physics.CheckSphere(transform.position, attackRadius, playerLayer);
        
        if (_playerInLookRadius && !_playerInAttackRadius) Chase();
        if (_playerInLookRadius && _playerInAttackRadius) Attack();
    }
    
    void Chase()
    {
        _agent.SetDestination(PlayerInstanceScriptableObject.Player.transform.position);
    }
    
    private void Attack()
    {
        FaceTarget();
        if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Enemy_attack"))
        {
            _animator.SetTrigger("Attack");
            _agent.SetDestination(transform.position);
        }
    }
    
    private void FaceTarget()
    {
        Vector3 direction = (PlayerInstanceScriptableObject.Player.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5);
    }

    public void SetDamageActive(int value)
    {
        canAttack = value == 1;
        Debug.Log(canAttack);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        var position = transform.position;
        Gizmos.DrawWireSphere(position, lookRadius);

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(position, attackRadius);
    }

    public bool canAttack { get; set; }
}