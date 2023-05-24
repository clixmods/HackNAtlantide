using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    sleeping,
    idle,
    moving,
    attacking
}
public class EnemyBehaviour : MonoBehaviour
{
    bool _isAwake;
    bool _canSleepAfterAwake;
    NavMeshAgent _agent;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Move(Vector3 direction)
    {
        _agent.Move(direction);
    }
    private void FaceTarget(Vector3 target)
    {
        transform.rotation = Quaternion.LookRotation(target);
    }
    private void WakeUp()
    {

    }
    private void Attack()
    {

    }
    private void Sleep()
    {

    }
}
