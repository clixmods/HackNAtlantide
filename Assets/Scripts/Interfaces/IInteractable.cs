using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    Transform transform { get; }
    public void Interact();
    public void CancelInteract();
}
