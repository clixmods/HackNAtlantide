using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTrigger : MonoBehaviour
{
    [SerializeField] private float damagePerSeconds;
    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.layer == 6)
        {
            other.gameObject.GetComponentInChildren<IDamageable>().DoDamage(damagePerSeconds*Time.fixedDeltaTime);
        }
    }
}
