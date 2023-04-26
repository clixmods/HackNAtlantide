using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InputHelper))]
public class AttackLevitationInteractable : MonoBehaviour, IInteractable
{
    Rigidbody _rigidBody;
    InputHelper inputHelper;
    bool _hasInteract;
    [SerializeField] PlayerDetectionScriptableObject _playerDetectionScriptableObject;
    private Vector3 _initialPosition;
    private Quaternion _initialRotation;
    private float _chargePercentage = 0;
    private bool _isCharging;
    private void Awake()
    {
        _initialPosition = transform.position;
        _initialRotation = transform.rotation;
        _rigidBody = GetComponent<Rigidbody>();
        inputHelper = GetComponent<InputHelper>();
        _hasInteract = false;
        
    }
    private void FixedUpdate()
    {
        inputHelper.enabled = _playerDetectionScriptableObject.IsInRange(transform.position)&&!_hasInteract? true:false;
        if (_hasInteract)
        {
            Vector3 direction = (Focus.Target.position - transform.position).normalized ;
            _rigidBody.useGravity = false;
            _rigidBody.velocity = direction*10  ;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_hasInteract)
        {
            //Destroy(gameObject);
        }
    }

    public void Interact()
    {
        if (!_hasInteract)
            StartCoroutine(ChargeObject());
    }

    IEnumerator ChargeObject()
    {
        _isCharging = true;
        float timeElapsed = 0;
        float timeToBeCharged = 2;
        while (timeElapsed < timeToBeCharged )
        {
            if(!_isCharging)
                yield break;
            var t = timeElapsed / timeToBeCharged;
            transform.position = Vector3.Lerp(_initialPosition, _initialPosition + Vector3.up*5, t);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        _hasInteract = true;
        _isCharging = false;
    }

    private void OnDrawGizmos()
    {
        if (Focus.Target != null)
        {
            Vector3 direction = (Focus.Target.position - transform.position).normalized;
            Gizmos.DrawLine(transform.position, transform.position + (Focus.Target.position - transform.position) );
        }
        
    }

    public void CancelInteract()
    {
        if (_hasInteract)
        {
            return;
        }
        Debug.Log("Cancel Interact" , gameObject);
        _isCharging = false;
        StopCoroutine(ChargeObject());
        _hasInteract = false;
        transform.position = _initialPosition;
        transform.rotation = _initialRotation;
    }
}
