using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody))]
public class BallInteractable : Interactable
{
    private Rigidbody _rigidbody; 
    [SerializeField] private float forceMultiplier;
    private bool _isInteract;
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        MoveBall();
    }

    private void MoveBall()
    {
        if (_isInteract)
        {
            Vector3 forceVector = (PlayerInstanceScriptableObject.Player.transform.position - transform.position);
            _rigidbody.AddForce(forceVector*forceMultiplier);
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
