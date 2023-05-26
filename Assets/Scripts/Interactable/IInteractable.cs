using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    Transform transform { get; }
    public abstract bool Interact();
    public abstract void CancelInteract();
    public abstract void ResetInteract();
    public abstract void ResetTransform();
}
