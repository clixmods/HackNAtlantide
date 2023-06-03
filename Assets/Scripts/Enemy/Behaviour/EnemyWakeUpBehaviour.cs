using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class EnemyWakeUpBehaviour : MonoBehaviour
{
    [SerializeField] private bool _wakeUpByDistance = true;
    [SerializeField] float _distanceToWakeUp;
    [SerializeField] bool _distanceByNavMesh = true;
    [SerializeField] float _distanceToSleep;
    [SerializeField] private bool _goBackToStartPosToSleep = true;
    public UnityEvent OnAwake;
    public UnityEvent OnSleep;
    private bool _isAwake;
    private Vector3 _startPos;
    
    EnemyBehaviour _enemyBehaviour;
    [SerializeField] ScriptableValueListGameObject _allEnemyAwake;

    private void Awake()
    {
        _startPos = transform.position;
        _enemyBehaviour = GetComponent<EnemyBehaviour>();
    }
    private void Update()
    {
        if(_isAwake && !_enemyBehaviour.IsAttacking)
        {
            _enemyBehaviour.ReturnToStartPos = _enemyBehaviour.DistanceWithPlayer > _distanceToSleep;
            if (_enemyBehaviour.ReturnToStartPos)
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
                {
                    WakeUp();
                }
            }
        }
    }

    //appele le wakeup de l'ennemie behaviour et met a jour le bool isAwake
    public void WakeUp()
    {
        _isAwake = true;
        _enemyBehaviour.WakeUp();
        OnAwake?.Invoke();
        _allEnemyAwake.AddUnique(this.gameObject);
    }

    public void ReturnToStartPos()
    {
        _goBackToStartPosToSleep = true;
        _enemyBehaviour.StopCoroutine(_enemyBehaviour.MoveToPlayer());
        _enemyBehaviour.Move(_startPos);
    }

    public void Sleep()
    {
        _goBackToStartPosToSleep = false;
        _isAwake = false;
        OnSleep?.Invoke();
        _allEnemyAwake.RemoveUnique(this.gameObject);
    }
}
