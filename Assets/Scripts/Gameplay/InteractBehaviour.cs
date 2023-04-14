using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO : Mettre RequireComponent pour le show/hide Icon
public class InteractBehaviour : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject _iconeInteract;

    
    public void Interact()
    {
        Debug.Log("interact");
    }

    public void ShowIcon()
    {
        _iconeInteract.SetActive(true);
    }
    public void HideIcon()
    {
        _iconeInteract.SetActive(false);
    }
}
