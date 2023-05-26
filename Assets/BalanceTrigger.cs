using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalanceTrigger : MonoBehaviour
{
    [SerializeField] private BalanceBehaviour _balanceBehaviour;
    public bool isRight;
    
    private void OnTriggerEnter(Collider other)
    {
        if (!TryGetComponent(out Rigidbody rb)) return;
        if (isRight)
            _balanceBehaviour.rightWeight += rb.mass;
        else
            _balanceBehaviour.leftWeight += rb.mass;
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (!TryGetComponent(out Rigidbody rb)) return;
        if (isRight)
            _balanceBehaviour.rightWeight -= rb.mass;
        else
            _balanceBehaviour.leftWeight -= rb.mass;
    }
}
