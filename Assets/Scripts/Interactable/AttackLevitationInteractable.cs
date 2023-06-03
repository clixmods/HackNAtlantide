using System;
using System.Collections;
using Attack;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(InputHelper))]
[RequireComponent(typeof(IAttackCollider))]
[RequireComponent(typeof(SphereCollider))]
public class AttackLevitationInteractable : Interactable
{
    private Rigidbody _rigidBody;
    private MeshRenderer _meshRenderer;
    private BoxCollider _boxCollider;
    /// <summary>
    /// The object has been interacted ? True when the object start to attack a target
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
    
    private bool _isCharging;
    private bool _isAttacking;
    private Transform _transformDestination;
    
    [Header("Settings")]
    [SerializeField] private float timeToBeCharged = 0.75f;
    [SerializeField] private float projectionSpeedMultiplier = 50f;
    [SerializeField] private float useStaminaAmount = 1f;
    [SerializeField] private GameObject _meshDestroy;
    [SerializeField] private float damageAmount = 1f;
    private IAttackCollider _attackCollider;
    private InputHelper _inputHelper;
    private UIChargeInputHelper _uiChargeInputHelper;

    public UnityEvent LaunchAttack;
    
    //Explosion
    private SphereCollider _colliderExplosion;
    [SerializeField] private float _speedExplosion;
    [SerializeField] private float _maxRadius;
    private bool _explosion;

    private int _layerBase;
    [SerializeField] float _timeToDestroyIfNoHit = 3f;


    // Mesh destroy
    private Rigidbody[] _rigidbodiesFromMeshDestroy;
    
    #region Monobehaviour
    private void Awake()
    {
        // Interactable Object setup
        _meshRenderer = GetComponent<MeshRenderer>();
        _boxCollider = GetComponent<BoxCollider>();
        _rigidBody = GetComponent<Rigidbody>();
        _layerBase = gameObject.layer;
        // Initial transform value
        _initialPosition = transform.position;
        _initialRotation = transform.rotation;
        _inputHelper = GetComponent<InputHelper>();
        _hasInteract = false;
        // Register Events
        Focus.OnFocusSwitch += SetDestination;
        Focus.OnFocusDisable += RemoveTarget;
        //Attack collider setup
        _attackCollider = GetComponent<IAttackCollider>();
        _attackCollider.OnCollideWithIDamageable += AttackColliderOnOnCollideWithIDamageable;

        _colliderExplosion = GetComponent<SphereCollider>();
        _colliderExplosion.radius = 0.1f;
        _colliderExplosion.isTrigger = true;
        if (_meshDestroy != null)
        {
            _rigidbodiesFromMeshDestroy = _meshDestroy.GetComponentsInChildren<Rigidbody>();
        }
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
    // We need to use late update, sometimes, the position targeted glitch because nav agent is bullshit
    private void LateUpdate()
    {
        if (_isAttacking)
        {
            _inputHelper.enabled = false;
            Vector3 direction;
            if (_transformDestination == null)
            {
                _isAttacking = false;
                _attackCollider.enabled = false;
                _attackCollider.gameObject.layer = _layerBase;
                ResetInteract();
                return;
            }
            direction = (_transformDestination.position - _rigidBody.worldCenterOfMass).normalized ;
            transform.LookAt(_transformDestination);
            _rigidBody.velocity = direction * projectionSpeedMultiplier ;
        }
        else if(_hasInteract )
        {
            if (_playerStamina.Value > _playerStamina.MaxStamina - 1)
            {
                _playerStamina.UseStamina(_playerStamina.SpeedToRecharge*Time.deltaTime);
            }
            transform.position = _playerDetectionScriptableObject.PlayerPosition + Vector3.up * 4;
        }
      
    }
    
    #endregion
    
    private void AttackColliderOnOnCollideWithIDamageable(object sender, EventArgs e)
    {
        if (e is AttackDamageableEventArgs mDamageableEventArgs)
        {
            mDamageableEventArgs.idamageable.DoDamage(damageAmount, transform.position);
            DestroyInteractable();
        }
    }
    private void SetDestination(IFocusable target = null)
    {
        if (_isAttacking)
            return; 
        
        if (Focus.FocusIsEnable)
        {
            _transformDestination = target.focusableTransform;
        }
        else
        {
            _transformDestination = null;
        }

    }

    public void DestroyInteractable()
    {
        if (_meshDestroy != null)
        {
            _meshDestroy.transform.position = transform.position;
            _meshDestroy.transform.rotation = transform.rotation;
            _meshDestroy.SetActive(true);
           
            foreach (Rigidbody rb in _rigidbodiesFromMeshDestroy)
            {
                rb.AddForce(Random.onUnitSphere * 5f, ForceMode.Impulse);
            }
        }
        Destroy(_meshRenderer);
        Destroy(_boxCollider);

        _rigidBody.isKinematic = true;
        StartCoroutine(Explosion());
    }

    private void RemoveTarget()
    {
        StartCoroutine(RemoveTargetAfterSeconds(1));
        
    }

    private IEnumerator RemoveTargetAfterSeconds(float value)
    {
        yield return new WaitForSeconds(value);
        if(!Focus.FocusIsEnable)
            _transformDestination = null;
    }

    IEnumerator ChargeObject()
    {
        _isCharging = true;
        float timeElapsed = 0;
        Vector3 startPosition = transform.position  ;
        _inputHelper.enabled = true;
        _uiChargeInputHelper.SetFillValue(1);
        _rigidBody.AddTorque(Random.onUnitSphere * 20);
        while (timeElapsed < timeToBeCharged )
        {
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
            _playerStamina.UseStamina((Time.deltaTime / timeToBeCharged) * useStaminaAmount );
            yield return null;
        }
        _uiChargeInputHelper.SetFillValue(1);
        _inputHelper.enabled = false;
        _hasInteract = true;
        _rigidBody.useGravity = false;
        _isCharging = false;
    }
    IEnumerator Explosion()
    {
        _explosion = true;
        while (_colliderExplosion.radius < _maxRadius)
        {
            _colliderExplosion.radius += Time.deltaTime * _speedExplosion;
            yield return null;
        }
        _colliderExplosion.enabled = false;
        Destroy(gameObject);
    }
    IEnumerator WaitForDestroy()
    {
        yield return new WaitForSeconds(_timeToDestroyIfNoHit);
        if(!_explosion && _isAttacking)
            DestroyInteractable();
    }
    #region IInteractable
    public override bool Interact()
    {
        if (_isAttacking)
        {
            return false;
        }
        if (!_hasInteract && _playerStamina.CanUseStamina(useStaminaAmount))
        {
            LaunchOnInteract();
            StartCoroutine(ChargeObject());
            return true;
        }
        return false;
    }
    public override void CancelInteract()
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
            if (_transformDestination == null)
            {
                _attackCollider.enabled = false;
                _attackCollider.gameObject.layer = _layerBase;
                ResetInteract();
                return;
            }
            // // A target is defined so start the attack
            // Focus.OnFocusSwitch -= SetDestination;
            // Focus.OnFocusNoTarget -= RemoveTarget;
            _isAttacking = true;
            LaunchAttack?.Invoke();
            _attackCollider.enabled = true;
            _inputHelper.enabled = false;

            StartCoroutine(WaitForDestroy());
            return;
        }

        ResetInteract();
    }

    public override void ResetInteract()
    {
        if (_hasInteract && !_isAttacking)
        {
            _attackCollider.enabled = false;
            _attackCollider.gameObject.layer = _layerBase;
        }
        _inputHelper.enabled = false;
        _uiChargeInputHelper.SetFillValue(1);
        _isCharging = false;
        StopCoroutine(ChargeObject());
        StopCoroutine(WaitForDestroy());
        _hasInteract = false;
        // Renable gravity
        _rigidBody.useGravity = true;
        _rigidBody.velocity = Vector3.zero;
        LaunchOnResetInteract();
    }
    public override void ResetTransform()
    {
        transform.position = _initialPosition;
        transform.rotation = _initialRotation;
    }

    #endregion

   
}
