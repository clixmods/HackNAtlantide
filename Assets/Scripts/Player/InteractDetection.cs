using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.PlayerSettings;

[RequireComponent(typeof(SphereCollider))]
public class InteractDetection : MonoBehaviour
{
    [SerializeField] InputButtonScriptableObject _interact;
    bool _canInteract = false;
    List<IInteractable> interactable = new List<IInteractable>();
    SphereCollider _sphereCollider;
    [SerializeField] float _maxDistanceInteraction;

    // Start is called before the first frame update
    private void Start()
    {
        _sphereCollider = GetComponent<SphereCollider>();
        _sphereCollider.radius = _maxDistanceInteraction;
        _sphereCollider.isTrigger = true;

    }
    void OnEnable()
    {
        _interact.OnValueChanged += InteractInput;
    }
    private void OnDisable()
    {
        _interact.OnValueChanged -= InteractInput;
    }
    void InteractInput(bool value)
    {
        if(value)
        {
            Collider[] cols = Physics.OverlapSphere(transform.position, _maxDistanceInteraction);
            for (int i = 0; i < cols.Length; i++)
            {
                if (cols[i].TryGetComponent<IInteractable>(out IInteractable interactObject))
                {
                    interactable.Add(interactObject);
                }
            }
            //calculate closest Interactable
            float closestSqrDistance = Mathf.Infinity;
            IInteractable closestObject = null;
            for (int i = 0; i < interactable.Count; i++)
            {
                float distance = (interactable[i].transform.position - transform.position).sqrMagnitude;
                if (closestSqrDistance > distance)
                {
                    closestObject = interactable[i];
                    closestSqrDistance = distance;
                }
            }
            if(closestObject!=null)
            {
                closestObject.Interact();
            }
        }

    }
    
}
