using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWakeUpBehaviour : MonoBehaviour
{
    [SerializeField] private bool _wakeUpByDistance = true;
    [SerializeField] float _distanceToWakeUp;
    [SerializeField] float _distanceToSleep;
    [SerializeField] private bool _goBackToStartPosToSleep = true;
    private bool _isAwake;
    private Vector3 _startPos;
    
    EnemyBehaviour _enemyBehaviour;

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
            //attends que le joueur soit à une distance minimale et qu'il ne soit pas réveiller
            if (_wakeUpByDistance && _enemyBehaviour.DistanceWithPlayer < _distanceToWakeUp && !_isAwake)
            {
                WakeUp();
            }
        }
    }

    //appele le wakeup de l'ennemie behaviour et met a jour le bool isAwake
    public void WakeUp()
    {
        _isAwake = true;
        _enemyBehaviour.WakeUp();
    }

    public void ReturnToStartPos()
    {
        _enemyBehaviour.StopCoroutine(_enemyBehaviour.MoveToPlayer());
        _enemyBehaviour.Move(_startPos);
    }

}
