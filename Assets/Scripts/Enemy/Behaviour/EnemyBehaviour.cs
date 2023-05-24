using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    Sleeping,
    Idle,
    Moving,
    Attacking,
    Dead,
}
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyBehaviour : MonoBehaviour
{
    #region properties
    //states
    bool _isAwake;
    [SerializeField] bool _canSleepAfterAwake;
    bool _isAttacking;
    bool _canMove;
    EnemyState _state = EnemyState.Sleeping;

    //RequiredComponents
    private NavMeshAgent _agent;
    public NavMeshAgent Agent { get { return _agent; } }

    private Animator _animator;
    public Animator Animator { get { return _animator; } }

    //Attack data
    [SerializeField] List<EnemyAttackBehaviour> _allEnnemyAttacks;
    private List<EnemyAttackBehaviour> _ennemyAttacks;
    EnemyAttackBehaviour _currentAttack;
    [SerializeField] private float _rotationSpeed;
    #endregion

    #region MonoBehaviour
    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        WakeUp();
    }
    #endregion

    private void Move(Vector3 target)
    {
        _agent.SetDestination(target);
    }
    IEnumerator MoveToPlayer()
    {
        _canMove = true;
        while(_canMove)
        {
            Move(PlayerInstanceScriptableObject.Player.transform.position);
            FacePlayer();
            yield return null;
        }
        
    }
    public void FacePlayer()
    {
        Vector3 playerFlatPos = new Vector3(PlayerInstanceScriptableObject.Player.transform.position.x, 0,PlayerInstanceScriptableObject.Player.transform.position.z);
        Vector3 flatPos = new Vector3(transform.position.x, 0, transform.position.z);
        Quaternion _targetRotation = Quaternion.LookRotation((playerFlatPos - transform.position), Vector3.up);

        transform.rotation = Quaternion.Slerp(transform.rotation, _targetRotation, _rotationSpeed * Time.deltaTime) ;
    }
    

    //A Tester
    public IEnumerator Attack()
    {
        while(!_isAttacking)
        {
            //trie Chaque Attack de l'ennemie par priorité
            _ennemyAttacks = SortAttacksByPriority();

            //Selectione l'attaque disponible la plus prioritaire
            for (int i = 0; i < _ennemyAttacks.Count; i++)
            {
                if (_ennemyAttacks[i].CanAttack())
                {
                    _ennemyAttacks[i].Attack();
                    _currentAttack = _ennemyAttacks[i];
                    _isAttacking = true;
                    _state = EnemyState.Attacking;
                    break;
                }
            }

            yield return null;
        }
        _canMove = false;
        _agent.SetDestination(transform.position);
        Debug.Log("Current CoolDown is : " + _currentAttack.CoolDown);
        //attends le coolDown de l'attaque qui est joué pour commencer a rechercher une nouvelle attaque
        yield return new WaitForSeconds(_currentAttack.CoolDown);
        _currentAttack.StartCoroutine(_currentAttack.RechargePriority());

        _isAttacking = false;

        _currentAttack = null;

        //recommence a attaquer
        StartCoroutine(Attack());
        StartCoroutine(MoveToPlayer());
    }

    //A Tester
    //trie la liste d'attaque par priorité croissante
    private List<EnemyAttackBehaviour> SortAttacksByPriority()
    {
        //initialize une liste temporaire
        List<EnemyAttackBehaviour> allEnemyAttacksInOrder = _allEnnemyAttacks;

        //trie la liste
        _allEnnemyAttacks.Sort((a1, a2) => a1.Priority.CompareTo(a2.Priority));

        return allEnemyAttacksInOrder;
    }
    private void Sleep()
    {
        _isAwake = false;
        _state = EnemyState.Sleeping;
    }
    private void WakeUp()
    {
        _isAwake = true;
        StartCoroutine(MoveToPlayer());
    }
    public float DistanceWithPlayer()
    {
        return Vector3.Distance(transform.position, PlayerInstanceScriptableObject.Player.transform.position); 
    }
}
