using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InteractDetection : MonoBehaviour
{
    [SerializeField] InputButtonScriptableObject _interact;
    List<IInteractable> interactable = new List<IInteractable>();
    float _maxDistanceInteraction;
    [SerializeField] PlayerDetectionScriptableObject _playerDetectionScriptableObject;

    void OnEnable()
    {
        _interact.OnValueChanged += InteractInput;
    }
    private void OnDisable()
    {
        _interact.OnValueChanged -= InteractInput;
    }
    private void Awake()
    {
        _maxDistanceInteraction = _playerDetectionScriptableObject.MaxDistance;
    }
    private void Update()
    {
        _playerDetectionScriptableObject.PlayerPosition = transform.position;
    }
    void InteractInput(bool value)
    {
        if(value)
        {
            Collider[] cols = Physics.OverlapSphere(transform.position, _maxDistanceInteraction);
            for (int i = 0; i < cols.Length; i++)
            {
                if (cols[i].gameObject.TryGetComponent<IInteractable>(out var interactObject))
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
