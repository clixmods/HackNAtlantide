using System;
using System.Collections;
using Attack;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(InputHelper))]
[RequireComponent(typeof(IAttackCollider))]
[RequireComponent(typeof(SphereCollider))]
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
    private InputHelper _inputHelper;
    private UIChargeInputHelper _uiChargeInputHelper;

    //Explosiopn
    SphereCollider _colliderExplosion;
    [SerializeField] float _speedExplosion;
    [SerializeField] float _maxRadius;
    bool explosion;

    private int _layerBase;
    #region Monobehaviour
    private void Awake()
    {
        _layerBase = gameObject.layer;
        _initialPosition = transform.position;
        _initialRotation = transform.rotation;
        _inputHelper = GetComponent<InputHelper>();
        _rigidBody = GetComponent<Rigidbody>();
        _hasInteract = false;
        Focus.OnFocusSwitch += SetDestination;
        Focus.OnFocusDisable += RemoveTarget;
        _attackCollider = GetComponent<IAttackCollider>();
        _attackCollider.OnCollideWithIDamageable += AttackColliderOnOnCollideWithIDamageable;

        _colliderExplosion = GetComponent<SphereCollider>();
        _colliderExplosion.radius = 0.1f;
        _colliderExplosion.isTrigger = true;

    }
    private void OnDestroy()
    {
        if (_attackCollider != null)
        {
            _attackCollider.OnCollideWithIDamageable -= AttackColliderOnOnCollideWithIDamageable;
        }
        Focus.OnFocusSwitch -= SetDestination;
        Focus.OnFocusDisable -= RemoveTarget;
    }
    private void Start()
    {
        _attackCollider.enabled = false;
        _uiChargeInputHelper = (UIChargeInputHelper)(_inputHelper.UIInputHelper);
        _inputHelper.enabled = false;
    }
    private void Update()
    {
        Explosion();
    }
    void Explosion()
    {
        if (explosion)
        {
            if (_colliderExplosion.radius > _maxRadius)
            {
                _colliderExplosion.enabled = false;
                Destroy(gameObject);
            }
            else
            {
                _colliderExplosion.radius += Time.deltaTime * _speedExplosion;
            }
        }
    }

    // We need to use late update, sometimes, the position targeted glitch because nav agent is bullshit
    private void LateUpdate()
    {
        if (_isAttacking)
        {
            _inputHelper.enabled = false;
            Vector3 direction;
            if (transformDestination == null)
            {
                direction = transform.forward.normalized ;
            }
            else
            {
                direction = (transformDestination.position - _rigidBody.worldCenterOfMass).normalized ;
            }
            transform.LookAt(transformDestination);
            _rigidBody.velocity = direction * projectionSpeedMultiplier ;
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
        private void SetDestination(IFocusable target = null) 
        {
            if (Focus.FocusIsEnable)
            {
                transformDestination = target.focusableTransform;
            }
            else
            {
                transformDestination = null;
            }
            
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
            Destroy(GetComponent<MeshRenderer>());
            Destroy(GetComponent<BoxCollider>());
            
            _rigidBody.isKinematic = true ;
            explosion = true;
        }
        
        private void RemoveTarget()
        {
            transformDestination = null;
        }
    IEnumerator ChargeObject()
    {
        _isCharging = true;
        float timeElapsed = 0;
      
        _inputHelper.enabled = true;
        _uiChargeInputHelper.SetFillValue(1);
        _rigidBody.AddTorque(Random.onUnitSphere * 20);
        while (timeElapsed < timeToBeCharged )
        {
            
            Vector3 startPosition = _playerDetectionScriptableObject.PlayerPosition + Vector3.up * 2 ;
            Vector3 destinationPosition = _playerDetectionScriptableObject.PlayerPosition + Vector3.up * 4;
            if (!_isCharging)
            {
                _hasInteract = false;
                yield break;
            }
                
            
            _inputHelper.enabled = true;
            _playerStamina.UseStamina(_playerStamina.SpeedToRecharge*Time.deltaTime);
            var t = timeElapsed / timeToBeCharged;
            _uiChargeInputHelper.SetFillValue(t);
            transform.position = Vector3.Lerp(startPosition, destinationPosition, t);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        _uiChargeInputHelper.SetFillValue(1);
        _inputHelper.enabled = false;
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
    public bool Interact()
    {
        if (_isAttacking)
        {
            return false;
        }
        if (!_hasInteract && _playerStamina.CanUseStamina(useStaminaAmount))
        {
            StartCoroutine(ChargeObject());
            return true;
        }
        return false;
    }
    public void CancelInteract()
    {
        _inputHelper.enabled = false;
        _uiChargeInputHelper.SetFillValue(1);
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
                _attackCollider.enabled = false;
                _attackCollider.gameObject.layer = _layerBase;
                ResetInteract();
                return;
            }
            // A target is defined so start the attack
            Focus.OnFocusSwitch -= SetDestination;
            Focus.OnFocusNoTarget -= RemoveTarget;
            _isAttacking = true;
            _inputHelper.enabled = false;
            return;
        }

        ResetInteract();
    }

    public void ResetInteract()
    {
        if (_hasInteract && !_isAttacking)
        {
            _attackCollider.enabled = false;
            _attackCollider.gameObject.layer = _layerBase;
        }

        Debug.Log("Cancel Interact" , gameObject);
        _inputHelper.enabled = false;
        _uiChargeInputHelper.SetFillValue(1);
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
