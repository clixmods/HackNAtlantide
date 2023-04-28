using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    private float attackRadius = 2f;
    private float _lookRadius = 10f;
    
    bool playerInLookRadius, playerInAttackRadius;
    [SerializeField] LayerMask groundLayer, playerLayer;

    NavMeshAgent _agent;

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        playerInLookRadius = Physics.CheckSphere(transform.position, _lookRadius, playerLayer);
        playerInAttackRadius = Physics.CheckSphere(transform.position, attackRadius, playerLayer);
        
        if (playerInLookRadius && !playerInAttackRadius) Chase();
        if (playerInLookRadius && playerInAttackRadius) Attack();
    }
    
    void Chase()
    {
        _agent.SetDestination(PlayerInstanceScriptableObject.Player.transform.position);
    }
    
    private void Attack()
    {
        var player = PlayerInstanceScriptableObject.Player.transform.position;
        
        if(player != null)
        {
            Debug.Log("attacking!");
        }
    }
    
    /*private void OnTriggerEnter(Collider other)
    {
        var player = PlayerInstanceScriptableObject.Player.transform.position;
        
        print("ATTACK");

        if(player != null)
        {
            print("HIT!");
        }
    }*/

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _lookRadius);

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}