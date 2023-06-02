using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionPhysics : MonoBehaviour
{
    [SerializeField] float explosionForce;
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            Vector3 direction = (rb.position - transform.position);
            rb.AddForce(explosionForce * direction/direction.sqrMagnitude, ForceMode.Impulse);
        }
    }
}
