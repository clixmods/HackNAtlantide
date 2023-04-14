using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CreateInputHelper))]
public class InteractBehaviour : MonoBehaviour, IInteractable
{
    [SerializeField] Rigidbody _rigidBody;
    CreateInputHelper inputHelper;
    bool _hasInteract;
    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
        inputHelper = GetComponent<CreateInputHelper>();
        _hasInteract = false;
    }
    public void Interact()
    {
        if (!_hasInteract)
        {
            _rigidBody.AddForce((new Vector3(0.5f - Random.value, 0.5f - Random.value, 0.5f - Random.value)+Vector3.up).normalized * 30, ForceMode.Impulse);
            inputHelper.enabled = false;
            _hasInteract = true;
        }
        
    }
}
