using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.UIElements;

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
    public bool IsAwake { get { return _isAwake; } set { _isAwake = value; } }
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
    public float DistanceWithPlayer { get {return _distanceWithPlayer; } }
    bool _movecoroutineIsPlayed = false;
    public bool MovecoroutineIsPlayed { get { return _movecoroutineIsPlayed; } }
    Rigidbody _rigidBody;
    [SerializeField] bool _calculateDistanceByNavMeshPath = true;
    [SerializeField] ScriptableEventBool _canEnemyTargetPlayer;
    private bool canTargetPlayer;
    public UnityEvent onAwake;

    [SerializeField] protected Focusable _focusable;
    [SerializeField] protected bool _onlyFocusWhenAwake = true;

    #endregion

    #region MonoBehaviour

    private void Awake()
    {

        _focusable = GetComponent<Focusable>();
        if (_onlyFocusWhenAwake)
        {
            _focusable.IsTargetable = false;
        }
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _rigidBody = GetComponent<Rigidbody>();
        canTargetPlayer = true;
    }
    private void OnEnable()
    {
        _canEnemyTargetPlayer.OnEvent += CanTargetPlayer;
    }
    private void OnDisable()
    {
        _canEnemyTargetPlayer.OnEvent -= CanTargetPlayer;
    }
    void CanTargetPlayer(bool value)
    {
        canTargetPlayer = value;
        if (!value)
            Agent.SetDestination(transform.position);
    }
    #endregion
    private void Update()
    {
        if(_isAwake && Agent.enabled)
        {
            _distanceWithPlayer = GetPathLength();
        }
        else
        {
            _distanceWithPlayer = float.MaxValue;
        }
        if(_isAwake)
        {
            Animator.SetFloat("Walk_Speed", _agent.velocity.magnitude / _agent.speed);
            
        }
        //_rigidBody.isKinematic = _distanceWithPlayer > 2.5f;
    }
    public virtual void Move(Vector3 target)
    {
        if(_agent.enabled && canTargetPlayer)
        {
            if (_isAwake && _canMove)
            {
                _agent.SetDestination(target);
            }
            else
            {
                _agent.SetDestination(transform.position);
            }
        }
    }
    public IEnumerator MoveToPlayer()
    {
        _movecoroutineIsPlayed = true;
        _canMove = true;
        while (_canMove && _isAwake && !_returnToStartPos && _agent.enabled && canTargetPlayer)
        {
            Move(PlayerInstanceScriptableObject.Player.transform.position);
            yield return null;
        }
        _movecoroutineIsPlayed = false;

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
            //trie Chaque Attack de l'ennemie par priorit�
            _ennemyAttacks = SortAttacksByPriority();

            //Selectione l'attaque disponible la plus prioritaire
            for (int i = 0; i < _ennemyAttacks.Count; i++)
            {
                if (_ennemyAttacks[i].CanAttack() && canTargetPlayer)
                {
                    _ennemyAttacks[i].Attack();
                    _currentAttack = _ennemyAttacks[i];
                    _isAttacking = true;
                    _state = EnemyState.Attacking;
                    _agent.updateRotation = false;
                    break;
                }
            }

            yield return null;
        }
        _canMove = false;
        if(_agent.enabled)
        {
            _agent.SetDestination(transform.position);
        }
        //attends le coolDown de l'attaque qui est jou� pour commencer a rechercher une nouvelle attaque
        yield return new WaitForSeconds(_currentAttack.CoolDown);
        _currentAttack.StartCoroutine(_currentAttack.RechargePriority());

        _isAttacking = false;
        _agent.updateRotation = true;
        _currentAttack = null;

        //recommence a attaquer
        StartCoroutine(Attack());
        if(!_movecoroutineIsPlayed)
        {
            StartCoroutine(MoveToPlayer());
        }
    }

    //A Tester
    //trie la liste d'attaque par priorit� croissante
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
    public virtual void WakeUp()
    {
        StartCoroutine(WakeUpCoroutine());
    }
    public virtual IEnumerator WakeUpCoroutine()
    {
        yield return new WaitForSeconds(2f);
        _isAwake = true;
        onAwake?.Invoke();
        _focusable.IsTargetable = !GetComponent<Character>().IsInvulnerable;
        StartCoroutine(MoveToPlayer());
    }
    public float GetPathLength()
    {
        if(_calculateDistanceByNavMeshPath)
        {
            NavMeshPath pathToPlayer = new NavMeshPath();
            _agent.CalculatePath(PlayerInstanceScriptableObject.Player.transform.position, pathToPlayer);
            float lng = 0.0f;

            if ((pathToPlayer.status == NavMeshPathStatus.PathComplete))
            {
                for (int i = 1; i < pathToPlayer.corners.Length; ++i)
                {
                    lng += Vector3.Distance(pathToPlayer.corners[i - 1], pathToPlayer.corners[i]);
                }
            }
            else
            {
                lng = float.MaxValue;
            }

            return lng;
        }
        else
        {
            return Vector3.Distance(PlayerInstanceScriptableObject.Player.transform.position, transform.position);
        }
        
    }
}
