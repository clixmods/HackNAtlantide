using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO : Mettre RequireComponent pour le show/hide Icon
public class InteractBehaviour : MonoBehaviour, IInteractable
{
    [SerializeField] Rigidbody rb;
    public void Interact()
    {
        Debug.Log("Interact");
        rb.AddForce(Vector3.up * 1000,ForceMode.Impulse);
    }
}
