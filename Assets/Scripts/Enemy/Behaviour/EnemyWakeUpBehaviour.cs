using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class EnemyWakeUpBehaviour : MonoBehaviour
{
    [SerializeField] private bool _wakeUpByDistance = true;
    [SerializeField] float _distanceToWakeUp;
    [SerializeField] bool _distanceByNavMesh = true;
    [SerializeField] float _distanceToSleep;
    [SerializeField] private bool _goBackToStartPosToSleep = true;
    [SerializeField] private bool _wakeUpAtStart;
    [SerializeField] private bool _canReturnSleep = true;
    public UnityEvent OnAwake;
    public UnityEvent OnSleep;
    private bool _isAwake;
    private Vector3 _startPos;
    [SerializeField] string sleepAnim;
    
    EnemyBehaviour _enemyBehaviour;
    [SerializeField] ScriptableValueListGameObject _allEnemyAwake;

    private void Awake()
    {
        _startPos = transform.position;
        _enemyBehaviour = GetComponent<EnemyBehaviour>();
    }

    private void Start()
    {
        if (_wakeUpAtStart)
        {
            WakeUp();
        }
    }

    private void Update()
    {
        if(_isAwake && _enemyBehaviour != null &&  !_enemyBehaviour.IsAttacking)
        {
            _enemyBehaviour.ReturnToStartPos = _enemyBehaviour.DistanceWithPlayer > _distanceToSleep;
            if ( _canReturnSleep && _enemyBehaviour.ReturnToStartPos)
            {
                ReturnToStartPos();
            }
            else
            {
                if (!_enemyBehaviour.MovecoroutineIsPlayed)
                {
                    StartCoroutine(_enemyBehaviour.MoveToPlayer());
                }
            }
        }
        else
        {
            //attends que le joueur soit � une distance minimale et qu'il ne soit pas r�veiller
            if (_wakeUpByDistance)
            {
                float distance = Vector3.Distance(transform.position, PlayerInstanceScriptableObject.Player.transform.position);
                if (_distanceByNavMesh)
                {
                    if (distance < _distanceToWakeUp * 2)
                    {
                        distance = _enemyBehaviour.GetPathLength();
                    }
                }
                else
                {
                    distance = Vector3.Distance(transform.position, PlayerInstanceScriptableObject.Player.transform.position);
                }
                if (distance < _distanceToWakeUp && !_isAwake)
                //attends que le joueur soit � une distance minimale et qu'il ne soit pas r�veiller
                if (_wakeUpByDistance && _enemyBehaviour.GetPathLength() < _distanceToWakeUp && !_isAwake)
                {
                    WakeUp();
                }
            }
        }
    }
    //appele le wakeup de l'ennemie behaviour et met a jour le bool isAwake
    public void WakeUp()
    {
        if(!_isAwake)
        {
            _isAwake = true;
            _enemyBehaviour.WakeUp();
            OnAwake?.Invoke();
            _allEnemyAwake.AddUnique(this.gameObject);
        }
        
    }

    public void ReturnToStartPos()
    {
        if(Vector3.Distance(transform.position, _startPos) > 0.5f)
        {
            _goBackToStartPosToSleep = true;
            _enemyBehaviour.StopCoroutine(_enemyBehaviour.MoveToPlayer());
            _enemyBehaviour.Move(_startPos);
        }
        else
        {
            if(_goBackToStartPosToSleep)
            {
                Sleep();
            }
        }
    }

    public void Sleep()
    {
        _goBackToStartPosToSleep = false;
        _isAwake = false;
        _enemyBehaviour.IsAwake = false;
        _enemyBehaviour.Animator.CrossFade(sleepAnim, 0.2f);
        OnSleep?.Invoke();
        _allEnemyAwake.RemoveUnique(this.gameObject);
    }
    // on switch scene, we need to remove the enemy of the list
    private void OnDestroy()
    {
        _allEnemyAwake.RemoveUnique(gameObject);
    }
}
