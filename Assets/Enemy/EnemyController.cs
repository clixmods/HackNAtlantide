using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float lookRadius;

    private bool isAwake;
    private float timerToGoSleep;

    Transform target;
    NavMeshAgent agent;

    private void Start()
    {
        target = Resources.Load<PlayerInstanceScriptableObject>("PlayerInstance").Player.transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        //Debug.Log(timerToGoSleep);
        float distance = Vector3.Distance(target.position, transform.position);

        if (distance <= lookRadius)
        {
            if(!isAwake)
            {
                timerToGoSleep = 0;
                StartCoroutine(SetDestinationToPlayer());

                if (distance <= agent.stoppingDistance)
                {
                    // Attack the target

                    // Face target
                    FaceTarget();
                }
            }
            else
            {
                timerToGoSleep = 0;
                agent.SetDestination(target.position);

                if (distance <= agent.stoppingDistance)
                {
                    // Attack the target

                    // Face target
                    FaceTarget();
                }
            }
            
        }
        else if(isAwake)
        {
            timerToGoSleep += Time.deltaTime;
        }

        if (timerToGoSleep > 3.5f)
        {
            isAwake = false;
            timerToGoSleep = 0f;
        }
    }

    public IEnumerator SetDestinationToPlayer()
    {
        float distance = Vector3.Distance(target.position, transform.position);
        // anim enemy awake
        Debug.Log("Is Awakening" + "/!\\ IS IN COROUTINE");

        yield return new WaitForSeconds(3.5f); // temps d'animation awake a mettre

        if (timerToGoSleep > 3.5f)
        {
            yield break;
        }

        if (distance <= lookRadius)
        {
            Debug.Log("Start focusing enemy" + "/!\\ IS IN COROUTINE & In Range");
            isAwake = true;
            agent.SetDestination(target.position);
        }
        else if (distance >= lookRadius)
        {
            Debug.Log("Not focusing enemy" + "/!\\ IS IN COROUTINE & Not In Range");
            isAwake = false;
        }
        else
        {
            Debug.Log("TESTTTTTTT");
        }
    }

    private void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);

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