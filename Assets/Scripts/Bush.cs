using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bush : MonoBehaviour
{
    BoxCollider BoxCollider;
    private void Start()
    {
        BoxCollider = GetComponent<BoxCollider>();
    }
    private void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.TryGetComponent<PlayerMovement>(out PlayerMovement playerMovement))
        {
            if(playerMovement.IsDashing)
            {
               BoxCollider.isTrigger = true;
            }
            
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent<PlayerMovement>(out PlayerMovement playerMovement))
        {
            BoxCollider.isTrigger = false;
        }
    }
}
