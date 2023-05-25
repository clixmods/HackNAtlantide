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
[RequireComponent(typeof(EnemyWakeUpBehaviour))]
public class EnemyBehaviour : MonoBehaviour
{
    #region properties
    //states
    bool _isAwake;
    bool _isAttacking;
    public bool IsAttacking { get { return _isAttacking; } }
    bool _canMove;
    bool _returnToStartPos;
    public bool ReturnToStartPos { get { return _returnToStartPos; } set { _returnToStartPos = value; } }
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
    private float _distanceWithPlayer = 1000000;
    public float DistanceWithPlayer { get { return _distanceWithPlayer; } }
    #endregion

    #region MonoBehaviour
    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
    }
    #endregion
    private void Update()
    {
        _distanceWithPlayer = Vector3.Distance(transform.position, PlayerInstanceScriptableObject.Player.transform.position);
        
    }
    public void Move(Vector3 target)
    {
        if(_isAwake && _canMove)
        {
            _agent.SetDestination(target);
            FaceTarget(target);
        }
        else
        {
            _agent.SetDestination(transform.position);
        }
        
    }
    public IEnumerator MoveToPlayer()
    {
        _canMove = true;
        while (_canMove && _isAwake && !_returnToStartPos)
        {
            Move(PlayerInstanceScriptableObject.Player.transform.position);
            FaceTarget(PlayerInstanceScriptableObject.Player.transform.position);
            yield return null;
        }
        
    }

    public void FaceTarget(Vector3 target)
    {
            Vector3 playerFlatPos = new Vector3(target.x, 0, target.z);
            Vector3 flatPos = new Vector3(transform.position.x, 0, transform.position.z);
            Quaternion _targetRotation = Quaternion.LookRotation((playerFlatPos - flatPos), Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, _targetRotation, _rotationSpeed * Time.deltaTime);
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
    public void Sleep()
    {
        _isAwake = false;
        _state = EnemyState.Sleeping;
    }
    public void WakeUp()
    {
        _isAwake = true;
        StartCoroutine(MoveToPlayer());
    }
}
