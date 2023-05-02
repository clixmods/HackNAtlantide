using System;
using System.Collections;
using System.Collections.Generic;
using Attack;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(InputHelper))]
[RequireComponent(typeof(IAttackCollider))]
public class AttackLevitationInteractable : MonoBehaviour, IInteractable
{
    private Rigidbody _rigidBody;
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
    private bool _isAttacking;
    private Transform transformDestination;
    [Header("Settings")]
    [SerializeField] private float timeToBeCharged = 0.75f;
    [SerializeField] private float projectionSpeedMultiplier = 50f;
    [SerializeField] private float useStaminaAmount = 1f;
    [SerializeField] private GameObject _meshDestroy;
    [SerializeField] private float damageAmount = 1f;
    private IAttackCollider _attackCollider;
    private UIChargeInputHelper _inputHelper;
    #region Monobehaviour
    private void Awake()
    {
        _initialPosition = transform.position;
        _initialRotation = transform.rotation;
        _rigidBody = GetComponent<Rigidbody>();
        _hasInteract = false;
        Focus.OnFocusSwitch += SetDestination;
        Focus.OnFocusNoTarget += RemoveTarget;
        _attackCollider = GetComponent<IAttackCollider>();
        _attackCollider.OnCollideWithIDamageable += AttackColliderOnOnCollideWithIDamageable;
        
    }
    private void OnDestroy()
    {
        _attackCollider.OnCollideWithIDamageable -= AttackColliderOnOnCollideWithIDamageable;
        Focus.OnFocusSwitch -= SetDestination;
        Focus.OnFocusNoTarget -= RemoveTarget;
    }
    private void Start()
    {
        _attackCollider.enabled = false;
        _inputHelper = (UIChargeInputHelper)(GetComponent<InputHelper>().UIInputHelper);
    }
   
   
    // We need to use late update, sometimes, the position targeted glitch because nav agent is bullshit
    private void LateUpdate()
    {
        // if (_inputHelper == null)
        // {
        //     _inputHelper = (UIChargeInputHelper)(GetComponent<InputHelper>().UIInputHelper);
        // }
       // inputHelper.enabled = _playerDetectionScriptableObject.IsInRange(transform.position)&&!_hasInteract;
        
        if (_isAttacking)
        {
            Vector3 direction;
            if (transformDestination == null)
            {
                direction = transform.forward.normalized ;
            }
            else
            {
                direction =(transformDestination.position - _rigidBody.worldCenterOfMass).normalized ;
            }
            transform.LookAt(transformDestination);
           
            
            _rigidBody.velocity = transform.forward * projectionSpeedMultiplier ;
        }
    }
    
  

        #endregion
        
        private void AttackColliderOnOnCollideWithIDamageable(object sender, EventArgs e)
        {
            if (e is DamageableEventArgs mDamageableEventArgs)
            {
                mDamageableEventArgs.idamageable.DoDamage(damageAmount);
                DestroyInteractable();
            }
        }
        private void SetDestination(ITargetable target)
        {
            transformDestination = target.targetableTransform;
        }
        
        private void DestroyInteractable()
        {
            if (_meshDestroy != null)
            {
                _meshDestroy.transform.position = transform.position;
                _meshDestroy.transform.rotation = transform.rotation;
                _meshDestroy.SetActive(true);
                Rigidbody[] childrb = _meshDestroy.GetComponentsInChildren<Rigidbody>();
                foreach(Rigidbody rb in childrb)
                {
                    rb.AddExplosionForce(Random.value, rb.position + Random.onUnitSphere, Random.value);
                }
            }
            this.gameObject.SetActive(false);
        }
        
        private void RemoveTarget()
        {
            transformDestination = null;
        }
    IEnumerator ChargeObject()
    {
        _isCharging = true;
        float timeElapsed = 0;
        Vector3 startPosition = transform.position;
        Vector3 destinationPosition = startPosition + Vector3.up * 5;
        _inputHelper.SetFillValue(1);
        while (timeElapsed < timeToBeCharged )
        {
            if(!_isCharging)
                yield break;
            
            _playerStamina.UseStamina(_playerStamina.SpeedToRecharge*Time.deltaTime);
            var t = timeElapsed / timeToBeCharged;
            _inputHelper.SetFillValue(t);
            transform.position = Vector3.Lerp(startPosition, destinationPosition, t);
            timeElapsed += Time.deltaTime;
            yield return null;
            //_rigidBody.AddTorque(Random.onUnitSphere * 20);
        }
        _inputHelper.SetFillValue(1);
        _hasInteract = true;
        _attackCollider.enabled = true;
        _playerStamina.UseStamina(useStaminaAmount);
        _rigidBody.useGravity = false;
        
        _isCharging = false;
    }
    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (transformDestination != null)
        {
            Gizmos.DrawLine(transform.position, transformDestination.position );
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
        _inputHelper.SetFillValue(1);
        if (_isAttacking)
        {
            return;
        }
            // If object is charged, go start the attack
        if (_hasInteract && !_isAttacking)
        {
            // Check if the transform destination is null, to cancel the attack
            if (transformDestination == null)
            {
                _isCharging = false;
                StopCoroutine(ChargeObject());
                _hasInteract = false;
                // Renable gravity
                _rigidBody.useGravity = true;
                return;
            }
            // A target is defined so start the attack
            Focus.OnFocusSwitch -= SetDestination;
            _isAttacking = true;
            return;
        }
        Debug.Log("Cancel Interact" , gameObject);
        _isCharging = false;
        StopCoroutine(ChargeObject());
        _hasInteract = false;
        // Renable gravity
        _rigidBody.useGravity = true;
    }
    #endregion

    private void ResetToInitialPositionAndRotation()
    {
        transform.position = _initialPosition;
        transform.rotation = _initialRotation;
    }
}
