using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    private float _lookRadius = 10f;

    public bool IsAwake { get { return _isAwake; } }
    private bool _isAwake;
    private float _timerToGoSleep;

    Transform _target;
    NavMeshAgent _agent;

    private void Start()
    {
        _target = PlayerInstanceScriptableObject.Player.transform;
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        //Debug.Log(timerToGoSleep);
        float distance = Vector3.Distance(_target.position, transform.position);

        if (distance <= _lookRadius)
        {
            if(!_isAwake)
            {
                _timerToGoSleep = 0;
                StartCoroutine(SetDestinationToPlayer());

                if (distance <= _agent.stoppingDistance)
                {
                    // Attack the target

                    // Face target
                    FaceTarget();
                }
            }
            else
            {
                _timerToGoSleep = 0;
                _agent.SetDestination(_target.position);

                if (distance <= _agent.stoppingDistance)
                {
                    // Attack the target

                    // Face target
                    FaceTarget();
                }
            }
            
        }
        else if(_isAwake)
        {
            _timerToGoSleep += Time.deltaTime;
        }

        if (_timerToGoSleep > 3f)
        {
            _isAwake = false;
            _timerToGoSleep = 0f;
        }
    }

    public IEnumerator SetDestinationToPlayer()
    {
        float distance = Vector3.Distance(_target.position, transform.position);
        // anim enemy awake
        Debug.Log("Is Awakening" + "/!\\ IS IN COROUTINE");

        yield return new WaitForSeconds(1f); // temps d'animation awake a mettre

        if (_timerToGoSleep > 3f)
        {
            yield break;
        }

        if (distance <= _lookRadius)
        {
            Debug.Log("Start focusing enemy" + "/!\\ IS IN COROUTINE & In Range");
            _isAwake = true;
            _agent.SetDestination(_target.position);
        }
        else if (distance >= _lookRadius)
        {
            Debug.Log("Not focusing enemy" + "/!\\ IS IN COROUTINE & Not In Range");
            _isAwake = false;
        }
        else
        {
            Debug.Log("TESTTTTTTT");
        }
    }

    private void FaceTarget()
    {
        Vector3 direction = (_target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _lookRadius);

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<PlayerMovement>(out var damageable))
        {
            // player.takedamage
            print("HIT!");
        }
    }
}