using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class InteractDetection : MonoBehaviour
{
    [SerializeField] InputButtonScriptableObject _interact;
    List<IInteractable> interactable = new List<IInteractable>();
    public List<IInteractable> Interactable { get { DetectInteract(); return interactable; } }
    IInteractable closestObject;
    public Action<List<IInteractable>> onInteractableListValueChanged;
    float _maxDistanceInteraction;
    [SerializeField] PlayerDetectionScriptableObject _playerDetectionScriptableObject;
    [SerializeField] private PlayerStaminaScriptableObject _playerStamina;


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
        transform.localPosition = Vector3.zero;
    }
    private void Update()
    {
        _playerDetectionScriptableObject.PlayerPosition = transform.position;
        if (_currentInteractable != null )
        {
            _currentInteractable.transform.position = transform.position + transform.up * 2;
        }
    }

    private IInteractable _currentInteractable;
    void InteractInput(bool value)
    {
        Debug.Log("Value Input "+value);
        
        DetectInteract();
        ClosestInteractable();
        if (closestObject != null)
        {
            if (closestObject.transform.TryGetComponent<InputHelper>(out var inputHelper))
            {
                inputHelper.enabled = true;
            }
        }
        if(value)
        {
            // A Interactable is currently used so we dont need to use an another one
            if (_currentInteractable != null)
            {
                return;
            }
            if(closestObject != null && _playerStamina.CanUseStamina())
            {
                closestObject.Interact();
                _currentInteractable = closestObject;
            }
        }
        else
        {
            if (_currentInteractable != null)
            {
                _currentInteractable.CancelInteract();
                _currentInteractable = null;
            }
               
                
            // if(closestObject != null)
            // {
            //     if(_currentInteractable != null)
            //         closestObject.CancelInteract();
            // }
        }
    }

    void DetectInteract()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, _maxDistanceInteraction);
        interactable = new List<IInteractable>();
        for (int i = 0; i < cols.Length; i++)
        {
            if (cols[i].gameObject.TryGetComponent<IInteractable>(out var interactObject))
            {
                interactable.Add(interactObject);
            }
        }
        
    }
    IInteractable ClosestInteractable()
    {
        //calculate closest Interactable
        float closestSqrDistance = Mathf.Infinity;
        closestObject = null;
        for (int i = 0; i < interactable.Count; i++)
        {
            if (interactable[i].transform.TryGetComponent<InputHelper>(out var inputHelper))
            {
                inputHelper.enabled = false;
            }
            // if (interactable[i] == null) continue;
            float distance = (interactable[i].transform.position - transform.position).sqrMagnitude;
            if (closestSqrDistance > distance)
            {
                closestObject = interactable[i];
                closestSqrDistance = distance;
            }
        }
        
        return closestObject;
    }
    
}
