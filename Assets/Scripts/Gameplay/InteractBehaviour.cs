using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InputHelper))]
public class InteractBehaviour : MonoBehaviour, IInteractable
{
    Rigidbody _rigidBody;
    InputHelper inputHelper;
    bool _hasInteract;
    [SerializeField] PlayerDetectionScriptableObject _playerDetectionScriptableObject;
    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
        inputHelper = GetComponent<InputHelper>();
        _hasInteract = false;
    }
    private void Update()
    {
        inputHelper.enabled = _playerDetectionScriptableObject.IsInRange(transform.position)&&!_hasInteract? true:false;
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
