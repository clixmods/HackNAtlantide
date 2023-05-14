using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    Transform transform { get; }
    public bool Interact();
    public void CancelInteract();
    public void ResetInteract();
}
