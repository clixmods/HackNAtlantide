using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(InputHelper))]
public class AttackLevitationInteractable : MonoBehaviour, IInteractable
{
    private Rigidbody _rigidBody;
    private InputHelper inputHelper;
    /// <summary>
    /// The object has been interacted ? True when the object start to project to a target
    /// </summary>
    private bool _hasInteract;
    [SerializeField] PlayerDetectionScriptableObject _playerDetectionScriptableObject;
    [SerializeField] private PlayerStaminaScriptableObject _playerStamina;
    /// <summary>
    /// Initial position of the in the Awake event
    /// </summary>
    private Vector3 _initialPosition;
    /// <summary>
    /// Initial rotation of the object in the Awake event
    /// </summary>
    private Quaternion _initialRotation;
    /// <summary>
    /// Charge progression to be projected to the target : 0 = no charged, 1 = charged
    /// </summary>
    private float _chargePercentage = 0;
    private bool _isCharging;
    private Transform transformDestination;
    [Header("Settings")]
    [SerializeField] private float timeToBeCharged = 0.75f;
    [SerializeField] private float projectionSpeedMultiplier = 50f;
    [SerializeField] private float useStaminaAmount = 1f;
    [SerializeField] private GameObject _meshDestroy;
    #region Monobehaviour
    private void Awake()
    {
        _initialPosition = transform.position;
        _initialRotation = transform.rotation;
        _rigidBody = GetComponent<Rigidbody>();
        inputHelper = GetComponent<InputHelper>();
        _hasInteract = false;
        Focus.OnFocusSwitch += SetDestination;
    }

    private void SetDestination(Transform target)
    {
        transformDestination = target;
    }

    private void FixedUpdate()
    {
        inputHelper.enabled = _playerDetectionScriptableObject.IsInRange(transform.position)&&!_hasInteract? true:false;
        if (_hasInteract)
        {
            Vector3 direction;
            if (transformDestination == null)
            {
                direction = transform.forward.normalized ;
            }
            else
            {
                direction = (transformDestination.position - transform.position).normalized ;
            }
            
            _rigidBody.useGravity = false;
            _rigidBody.velocity = direction * projectionSpeedMultiplier  ;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (_hasInteract)
        {
            if (collision.transform.TryGetComponent<IDamageable>(out var damageable))
            {
                damageable.TakeDamage(2f);
            }
            // Todo : implement collision ennemi etc
            DestroyInteractable();
        }
    }
    
    private void DestroyInteractable()
    {
        _meshDestroy.transform.position = transform.position;
        _meshDestroy.transform.rotation = transform.rotation;
        _meshDestroy.SetActive(true);
        Rigidbody[] childrb = _meshDestroy.GetComponentsInChildren<Rigidbody>();
        foreach(Rigidbody rb in childrb)
        {
            rb.AddExplosionForce(Random.value, rb.position + Random.onUnitSphere, Random.value);
        }
        this.gameObject.SetActive(false);
    }

        #endregion
    IEnumerator ChargeObject()
    {
        _isCharging = true;
        float timeElapsed = 0;
        
        while (timeElapsed < timeToBeCharged )
        {
            if(!_isCharging)
                yield break;
            
            _playerStamina.UseStamina(_playerStamina.SpeedToRecharge*Time.deltaTime);
            var t = timeElapsed / timeToBeCharged;
            transform.position = Vector3.Lerp(_initialPosition, _initialPosition + Vector3.up*5, t);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        _hasInteract = true;
        _playerStamina.UseStamina(useStaminaAmount);
        _isCharging = false;
    }

    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (transformDestination != null)
        {
            Gizmos.DrawLine(transform.position, transform.position + (transformDestination.position - transform.position) );
        }
    }
    #endif

    #region IInteractable

    public void Interact()
    {
        if (!_hasInteract && _playerStamina.CanUseStamina())
        {
            StartCoroutine(ChargeObject());
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
        // Reset initial position & rotation
        transform.position = _initialPosition;
        transform.rotation = _initialRotation;
    }

    #endregion
}
