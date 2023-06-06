using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BalanceTrigger : MonoBehaviour
{
    [SerializeField] private BalanceBehaviour balanceBehaviour;
    public bool isRight;
    public List<Rigidbody> rbList;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Rigidbody rb) && !rbList.Contains(rb))
        {
            rbList.Add(other.gameObject.GetComponent<Rigidbody>());
            if (isRight)
            {
                balanceBehaviour.rightWeight += rb.mass;
            }
            else
            {
                balanceBehaviour.leftWeight += rb.mass;
            }
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Rigidbody rb) && rbList.Contains(rb))
        {
            rbList.Remove(other.gameObject.GetComponent<Rigidbody>());
            if (isRight)
                balanceBehaviour.rightWeight -= rb.mass;
            else
                balanceBehaviour.leftWeight -= rb.mass;
        }
    }
}
