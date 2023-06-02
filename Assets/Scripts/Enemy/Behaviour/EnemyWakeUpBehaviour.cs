using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class EnemyWakeUpBehaviour : MonoBehaviour
{
    [SerializeField] private bool _wakeUpByDistance = true;
    [SerializeField] float _distanceToWakeUp;
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
            if(_enemyBehaviour.Agent.enabled)
            {
                //attends que le joueur soit à une distance minimale et qu'il ne soit pas réveiller
                if (_wakeUpByDistance && _enemyBehaviour.GetPathLength() < _distanceToWakeUp && !_isAwake)
                {
                    WakeUp();
                }
                if (Vector3.Distance(_startPos, transform.position) < 0.3f && _goBackToStartPosToSleep)
                {
                    Sleep();
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
