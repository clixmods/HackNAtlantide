using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.Rendering.DebugUI;

public class PlayerInteractDetection : MonoBehaviour
{
    [SerializeField] InputButtonScriptableObject _interact;
    [SerializeField] InputButtonScriptableObject _release;
    [SerializeField] private InputInfoScriptableObject _releaseInfo;
    List<IInteractable> interactable = new List<IInteractable>();
    public List<IInteractable> Interactable { get { DetectInteract(); return interactable; } }
    IInteractable closestObject;
    public Action<List<IInteractable>> onInteractableListValueChanged;
    private float maxDistanceInteraction => _playerDetectionScriptableObject.MaxDistance;
    [SerializeField] PlayerDetectionScriptableObject _playerDetectionScriptableObject;
    [SerializeField] private PlayerStaminaScriptableObject _playerStamina;

    private IInteractable _currentInteractable;

    public UnityEvent<IInteractable> InteractableSelected;
    public UnityEvent InteractableDeselected;
    

    void OnEnable()
    {
        _interact.OnValueChanged += InteractInput;
        _release.OnValueChanged += ReleaseInput;
    }
    private void OnDisable()
    {
        _interact.OnValueChanged -= InteractInput;
        _release.OnValueChanged -= ReleaseInput;
    }
    private void Awake()
    {
        //maxDistanceInteraction = _playerDetectionScriptableObject.MaxDistance;
        transform.localPosition = Vector3.zero;
    }
    private void Update()
    {
        if (_currentInteractable == null)
        {
             DetectInteract();
             ClosestInteractable();
        }
       
        _playerDetectionScriptableObject.PlayerPosition = transform.position;
        if (_currentInteractable != null )
        {
            _releaseInfo.ShowInputInfo();
            _currentInteractable.transform.position = transform.position + transform.up * 4;
        }
    }
    

 
    void InteractInput(bool value)
    {
        Debug.Log("Value Input "+value);
        // if (_currentInteractable == null)
        // {
        //     DetectInteract();
        //     ClosestInteractable();
        // }
        
        if(value)
        {
            // A Interactable is currently used so we dont need to use an another one
            if (_currentInteractable != null)
            {
                return;
            }
            // No interactableTransform used, so we need to check the closest and use it
            if(closestObject != null && closestObject.Interact())
            {
                
                _currentInteractable = closestObject;
                InteractableSelected?.Invoke(_currentInteractable);
            }
        }
        else
        {
            if (_currentInteractable != null)
            {
                _currentInteractable.CancelInteract();
                _currentInteractable = null;
                InteractableDeselected?.Invoke();
            }
        }
    }
    
    private void ReleaseInput(bool value)
    {
        if(value)
        {
            // A Interactable is currently used so we dont need to use an another one
            if (_currentInteractable != null)
            {
                _releaseInfo.RemoveInputInfo();
                _currentInteractable.ResetInteract();
                _currentInteractable = null;
                InteractableDeselected?.Invoke();
            }
        }
    }

    void DetectInteract()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, maxDistanceInteraction);
        interactable = new List<IInteractable>();
        
        for (int i = 0; i < cols.Length; i++)
        {
            if (cols[i].gameObject.TryGetComponent<IInteractable>(out var interactObject))
            {
                interactable.Add(interactObject);
                if (closestObject != interactObject && interactObject != _currentInteractable && interactObject.transform.TryGetComponent<InputHelper>(out var inputHelper))
                {
                    inputHelper.enabled = false;
                }
            }
        }
        // If the previous ClosestObject is not in the list of interactableTransform, we need to disable the input helper
        if ( closestObject != null && closestObject.transform.TryGetComponent<InputHelper>(out var closestObjectInputHelpernputHelper) )
        {
            if (!interactable.Contains(closestObject))
            {
                closestObjectInputHelpernputHelper.enabled = false;
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
            float distance = (interactable[i].transform.position - transform.position).sqrMagnitude;
            if (closestSqrDistance > distance)
            {
                closestObject = interactable[i];
                closestSqrDistance = distance;
            }
        }

        if (_currentInteractable == null)
        {
            _releaseInfo.RemoveInputInfo();
            if ( closestObject != null && closestObject.transform.TryGetComponent<InputHelper>(out var inputHelper) )
            {
                inputHelper.enabled = true;
            }
        }
        
        return closestObject;
    }
    
}
