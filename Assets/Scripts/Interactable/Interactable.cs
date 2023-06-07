using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Interactable : MonoBehaviour, IInteractable
{
    public UnityEvent OnInteract;
    public UnityEvent OnResetInteract;
    public UnityEvent OnBecameClosestObject;
    public UnityEvent OnBecameNotClosestObject;
    bool _isClosetstInteractable;
    public bool IsClosestInteractable 
    { 
        get { return _isClosetstInteractable; } 
        set 
        { 
            _isClosetstInteractable = value; 
            if (_isClosetstInteractable) 
            { 
                OnBecameClosestObject?.Invoke();
            } 
            else
            {
                OnBecameNotClosestObject?.Invoke();
                Debug.Log("notClosestObject");
            }
        } 
    }

    public void LaunchOnInteract()
    { OnInteract?.Invoke(); }
    public void LaunchOnResetInteract()
    { OnResetInteract?.Invoke(); }
    public abstract void CancelInteract();

    public abstract bool Interact();

    public abstract void ResetInteract();

    public abstract void ResetTransform();
}
