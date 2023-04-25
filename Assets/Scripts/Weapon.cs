using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public  float damage;

    BoxCollider triggerBox;

    private void Start()
    {
        triggerBox= GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        var damageable = other.gameObject.GetComponent<EnemyController>(); // getcomponent IDamageable
        if(damageable != null)
        {
            //damageable.takedamage(damage);
        }
    }

    public void EnableTriggerBox()
    {
        triggerBox.enabled = true;
    }

    public void DisableTriggerBox()
    {
        triggerBox.enabled = false;
    }
}