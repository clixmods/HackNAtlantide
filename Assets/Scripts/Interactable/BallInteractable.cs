using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody))]
public class BallInteractable : Interactable
{
    private Rigidbody _rigidbody; 
    [SerializeField] private InputVectorScriptableObject inputVectorScriptableObject;
    [SerializeField] private float forceMultiplier;
    private bool _isInteract;
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        inputVectorScriptableObject.OnValueChanged += InputVectorScriptableObjectOnOnValueChanged;
    }

    private void InputVectorScriptableObjectOnOnValueChanged(Vector2 value)
    {
        if (_isInteract)
        {
            var _camera = CameraUtility.Camera;
            Vector3 direction = new Vector3();
            Vector3 forceVector = new Vector3();
           
            // Generate forceDirection by the camera view
            forceVector = _camera.transform.TransformDirection( value);
            forceVector.y = 0;
            direction.x = forceVector.x * forceMultiplier ;
            direction.z = forceVector.z * forceMultiplier ;
            _rigidbody.velocity = direction;
        }
           
    }

    public override bool Interact()
    {
        _isInteract = true;
        _rigidbody.WakeUp();
        LaunchOnInteract();
        return true;
    }

    public override void CancelInteract()
    {
        _isInteract = false;
        _rigidbody.Sleep();
        LaunchOnResetInteract();
    }

    public override void ResetInteract()
    {
        throw new System.NotImplementedException();
    }

    public override void ResetTransform()
    {
        throw new System.NotImplementedException();
    }
}
