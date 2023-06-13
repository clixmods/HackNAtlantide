using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    #region properties
    //General
    [Header("GENERAL")]
    [Space(5)]
    private Rigidbody _rigidbody;
    private Camera _camera;
    [SerializeField] private PlayerMovementStateScriptableObject _playerMovementState;
    [SerializeField] private PlayerInstanceScriptableObject _playerInstanceSO;
    [SerializeField] private LayerMask _layerToIgnore;
    public LayerMask LayerToIgnore { get { return _layerToIgnore; } set { _layerToIgnore = value; } }

    //Input
    [Header("INPUT")]
    [Space(5)]
    [SerializeField] private InputVectorScriptableObject _moveInput;
    [SerializeField] private InputButtonScriptableObject _dashInput;
    [SerializeField] private InputButtonScriptableObject _dashAttackInput;
    private Vector3 _moveDirection;

    //Walk
    [Header("WALK")]
    [Space(5)]

    [SerializeField] private float _moveSpeed;
    private float _speed;
    private Vector3 _smoothMoveVelocity;
    private Vector3 _dashDirection;
    private Vector3 _attackDirection;
    public Vector3 MoveAmount { get { return _moveAmount; } }
    private Vector3 _moveAmount;
    [SerializeField] private float _smoothTimeAcceleration;
    [SerializeField] private float _smoothTimeAccelerationDash;

    //rotation look at
    [Header("LOOK")]
    [Space(5)]

    [SerializeField] private float _rotationSpeed;
    Quaternion _targetRotation;

    // Dash & Dash Attack
    [Header("DASH")]
    [Space(5)]

    [SerializeField] private float _dashTime;
    [SerializeField] private float _dashSpeed;
    [SerializeField] private float _dashReloadTime;
    private bool _canDash;
    private bool _isDashing;
    public bool IsDashing { get { return _isDashing; } }
    
    [Header("DASH ATTACK")]
    [Space(5)]
    [SerializeField] private float _dashAttackTime;
    [SerializeField] private float _dashAttackSpeed;
    [SerializeField] private float _dashAttackReloadTime;

    [SerializeField] ScriptableEventFloat dashScriptableEvent;
    [SerializeField] ScriptableEventFloat dashAttackScriptableEvent;
    [SerializeField] PlayerStaminaScriptableObject _playerStamina;
    private bool _canDashAttack;
    private bool _isDashingAttack;
    private bool _isAttacking;
    public bool IsDashingAttack { get { return _isDashingAttack; } }
    
    //Collision Detection
    private Collider[] _buffer = new Collider[8];
    new CapsuleCollider collider;

    //LockForCombat
    Transform _transformLock = null;
    Transform _transformLockTempForDash;

    [Header("FeedBack")]
    [SerializeField] RumblerDataConstant _dashRumble;
    [SerializeField] ParticleSystem _dashFX;
    [SerializeField] TrailRenderer _dashTrail;
    [SerializeField] ParticleSystem _dustWalk;
    
    [SerializeField] ScriptableEvent _dashEvent;
    [SerializeField] ScriptableEvent _dashAttackEvent;

    #endregion
    //CheatENgine ToRemove
    public bool fly;

    public UnityEvent OnTeleport;

    public UnityEvent OnDash;

    public UnityEvent OnDashCancel;
    // Start is called before the first frame update
    void Awake()
    {
        PlayerInstanceScriptableObject.Player = this.gameObject;
        _rigidbody = GetComponent<Rigidbody>();
        _camera = CameraUtility.Camera;
        collider = GetComponent<CapsuleCollider>();
        _speed = _moveSpeed;
        _canDash = true;
        _canDashAttack = true;
    }
    void OnEnable()
    {
        _moveInput.OnValueChanged += MoveInput;
        _dashInput.OnValueChanged += Dash;
        _dashAttackInput.OnValueChanged += DashOfDashAttack;
        Focus.OnFocusEnable += FocusEnable;
        Focus.OnFocusSwitch += FocusSwitch;
        Focus.OnFocusDisable += FocusUnLock;
        Focus.OnFocusNoTarget += FocusUnLock;
    }

    private void OnDisable()
    {
        _moveInput.OnValueChanged -= MoveInput;
        _dashInput.OnValueChanged -= Dash;
        _dashAttackInput.OnValueChanged -= DashOfDashAttack;
        Focus.OnFocusEnable -= FocusEnable;
        Focus.OnFocusSwitch -= FocusSwitch;
        Focus.OnFocusDisable -= FocusUnLock;
        Focus.OnFocusNoTarget -= FocusUnLock;
    }
    
    void MoveInput(Vector2 direction)
    {
        //Projects the camera forward on 2D horizontal plane
        
        Vector3 camForwardOnPlane = new Vector3(_camera.transform.forward.x, 0, _camera.transform.forward.z).normalized;
        Vector3 camRightOnPlane = new Vector3(_camera.transform.right.x, 0, _camera.transform.right.z).normalized;
        _moveDirection = direction.x * camRightOnPlane + direction.y * camForwardOnPlane;
    }
    
    public void MoveTo(Vector3 Target)
    {
        Vector3 direction = ((Target - transform.position).normalized);
        _moveDirection = new Vector3(direction.x, 0, direction.z);
    }
    
    public void CancelMove()
    {
        _moveDirection = Vector3.zero;
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
    }
    
    private void Move()
    {
        Vector3 targetmoveAmount = Vector3.zero;
        _rigidbody.velocity = new Vector3(0,_rigidbody.velocity.y,0);
        //direction in which player wants to move

        if(_isDashing)
        {
            targetmoveAmount = _dashDirection * (_speed * Time.fixedDeltaTime);
            //calculated direction based of his movedirection of the precedent frame
            _moveAmount = Vector3.SmoothDamp(_moveAmount, targetmoveAmount, ref _smoothMoveVelocity, _smoothTimeAccelerationDash);

            _playerMovementState.MovementState = MovementState.dashing;
        }
        else if (_isDashingAttack)
        {
            targetmoveAmount = _dashDirection * (_speed * Time.fixedDeltaTime);
            //calculated direction based of his movedirection of the precedent frame
            _moveAmount = Vector3.SmoothDamp(_moveAmount, targetmoveAmount, ref _smoothMoveVelocity, _smoothTimeAccelerationDash);

            _playerMovementState.MovementState = MovementState.dashingAttack;
        }
        else if(_isAttacking)
        {
            targetmoveAmount = _attackDirection * (_speed * Time.fixedDeltaTime);
            //calculated direction based of his movedirection of the precedent frame
            _moveAmount = Vector3.SmoothDamp(_moveAmount, targetmoveAmount, ref _smoothMoveVelocity, _smoothTimeAccelerationDash);

            _playerMovementState.MovementState = MovementState.attacking;
        }
        else
        {
            targetmoveAmount = _moveDirection * (_speed * Time.fixedDeltaTime);
            //calculated direction based of his movedirection of the precedent frame
            _moveAmount = Vector3.SmoothDamp(_moveAmount, targetmoveAmount, ref _smoothMoveVelocity, _smoothTimeAcceleration);
            if(_moveAmount.sqrMagnitude > 0.0005f)
            {
                _playerMovementState.MovementState =  MovementState.running;
                if(!_dustWalk.isEmitting)
                _dustWalk.Play();
            }
            else
            {
                _playerMovementState.MovementState = MovementState.idle;
                _dustWalk.Stop();
            }
        }

        //Move the object with the RigidBody
        MoveSweepTestRecurs(_moveAmount, 3);

        //Rotate the player by his direction
        LookAtDirection(_isDashing?_rotationSpeed*20:_rotationSpeed, targetmoveAmount);
        //Keep the player on ground
        StayGrounded();

        //Extract the rb from any collider
        ExtractFromColliders();
    }
    
    private void MoveSweepTestRecurs(Vector3 velocity, int recurs)
    {
        float distance = velocity.magnitude;
        Vector3 displacement = velocity;
        if (_rigidbody.SweepTest(velocity.normalized, out RaycastHit hit, distance, QueryTriggerInteraction.Ignore))
        {
            //ClampDistance with contact offset;
            distance = Mathf.Max(0f, hit.distance - Physics.defaultContactOffset);
            displacement = velocity.normalized * distance;
            if (hit.collider.TryGetComponent<Rigidbody>(out Rigidbody hitRB))
            {
                hitRB.AddForceAtPosition(-hit.normal * 5000f, hit.point);
            }
        }
        _rigidbody.MovePosition(_rigidbody.position + displacement);

        velocity -= displacement;
        velocity -= hit.normal * Vector3.Dot(velocity, hit.normal);

        //recursivity
        if ((--recurs != 0) && (velocity != Vector3.zero))
        {
            MoveSweepTestRecurs(velocity, recurs);
        }
    }
    
    private void ExtractFromColliders()
    {
        float halfHeight = (collider.height * .5f) - collider.radius;

        Vector3 bottom = collider.bounds.center + (Vector3.down * halfHeight);
        Vector3 top = collider.bounds.center + (Vector3.up * halfHeight);

        int amount = Physics.OverlapCapsuleNonAlloc(bottom, top, collider.radius, _buffer);
        for (int i = 0; i < amount; i++)
        {
            if (_buffer[i] == collider)
            {
                continue;
            }

            if (((1 << _buffer[i].gameObject.layer) &_layerToIgnore) == 0 && 
                Physics.ComputePenetration(collider, _rigidbody.position, _rigidbody.rotation,
                _buffer[i], _buffer[i].transform.position, _buffer[i].transform.rotation,
                                       out Vector3 direction, out float distance))
            {
                _rigidbody.MovePosition(_rigidbody.position + (direction * (Physics.defaultContactOffset + (distance))));
            }
        }
    }

    public void LookAtDirection(float speed, Vector3 direction)
    {
        if(_followTarget && _transformLock != null)
        {
            _targetRotation = Quaternion.LookRotation((new Vector3(_transformLock.position.x,transform.position.y, _transformLock.position.z)-transform.position), Vector3.up);
        }
        else if (direction.magnitude > 0.001f)
        {
            _targetRotation = Quaternion.LookRotation(direction, Vector3.up);
        }
        transform.rotation = Quaternion.Slerp(transform.rotation, _targetRotation, speed * Time.deltaTime);
    }

    public void Dash(bool value)
    {
        if (value && _canDash && _playerStamina.CanUseStamina(1)&& _moveDirection.sqrMagnitude > 0.1f && !_isAttacking)
        {
            _dashDirection = _moveDirection;
            _playerStamina.UseStamina(1);
            _speed = _dashSpeed;
            _isDashing = true;
            _canDash = false;
            _transformLockTempForDash = _transformLock;
            _transformLock = null;
            //ignore ennemie collision
            Physics.IgnoreLayerCollision(6, 11);
            Physics.IgnoreLayerCollision(this.gameObject.layer, 16);
            StartCoroutine(CancelDash());

            //FeedBack
            DashFeedBack(true);
            OnDash?.Invoke();
            
            _dashEvent.LaunchEvent();
        }
    }
    
    public void DashOfDashAttack(bool value)
    {
        if (value && _canDashAttack && _playerStamina.CanUseStamina(1f)&& _moveDirection.sqrMagnitude > 0.1f)
        {
            _dashDirection = _moveDirection;
            _dashAttackEvent.LaunchEvent();
            _playerStamina.UseStamina(1f);
            _speed = _dashAttackSpeed;
            _isDashingAttack = true;
            _canDashAttack = false;
            _transformLockTempForDash = _transformLock;
            _transformLock = null;
            //ignore ennemie collision
            Physics.IgnoreLayerCollision(6, 11);
            //ignore bushCollision
            Physics.IgnoreLayerCollision(this.gameObject.layer, 16);
            StartCoroutine(CancelDashAttack());

            //FeedBack
            DashAttackFeedBack(true);

            _dashAttackEvent.LaunchEvent();
        }
    }

    public void Attack(bool value, float timeToLock, float playerSpeed)
    {
        if (value)
        {
            _attackDirection = transform.forward;
            _speed = playerSpeed;
            _isAttacking = true;
            StartCoroutine(CancelAttack(timeToLock));
        }
    }
    //sets the speeds value to dash
    IEnumerator CancelDash()
    {
        yield return new WaitForSeconds(_dashTime);
        _speed = _moveSpeed;

        StartCoroutine(ReloadDash());

        StartCoroutine(CancelDashFeedback());
    }
    
    //sets the speeds value to dash attack
    IEnumerator CancelDashAttack()
    {
        yield return new WaitForSeconds(_dashAttackTime);

        _speed = _moveSpeed;
        
        _transformLock = _transformLockTempForDash;
        

        StartCoroutine(ReloadDashAttack());

        StartCoroutine(CancelDashAttackFeedback());
    }
    
    IEnumerator CancelDashFeedback()
    {
        yield return new WaitForSeconds(0.2f);
        _transformLock = _transformLockTempForDash;
        //ignore ennemie collision
        Physics.IgnoreLayerCollision(6, 11, false);
        Physics.IgnoreLayerCollision(this.gameObject.layer, 16, false);
        OnDashCancel?.Invoke();
        DashFeedBack(false);
        yield return new WaitForSeconds(0.3f);
        _isDashing = false;

        
    }
    IEnumerator CancelAttack(float time)
    {
        yield return new WaitForSeconds(time);
        _isAttacking = false;
        _speed = _moveSpeed;
    }
    public void SetTransformLock()
    {
        _transformLock = _transformLockTempForDash;
    }

    IEnumerator CancelDashAttackFeedback()
    {
        yield return new WaitForSeconds(0.2f);
        _transformLock = _transformLockTempForDash;
        _isDashingAttack = false;

        //ignore ennemie collision
        Physics.IgnoreLayerCollision(6, 11, false);
        Physics.IgnoreLayerCollision(this.gameObject.layer, 16, false);
        DashAttackFeedBack(false);
    }

    //allows player to dash again
    IEnumerator ReloadDash()
    {
        yield return new WaitForSeconds(_dashReloadTime);
        _canDash = true;
    }
    
    //allows player to dash attack again
    IEnumerator ReloadDashAttack()
    {
        yield return new WaitForSeconds(_dashAttackReloadTime);
        _canDashAttack = true;
    }
    
    private void StayGrounded()
    {
        if(!fly)
            _rigidbody.AddForce(Vector3.down * 1000000 * Time.fixedDeltaTime);
        /*float distance = 0f;
        Vector3 displacement = Vector3.zero;
        float magnitudeCheck = IsDashing || IsDashingAttack ? 0.6f : 0.3f;
        RaycastHit hit;
        if (Physics.Raycast(_rigidbody.position, Vector3.down, magnitudeCheck))
        {
            //ClampDistance with contact offset;
            distance = Mathf.Max(0f, hit.distance - Physics.defaultContactOffset);
            displacement = Vector3.down * distance;
        }
        _rigidbody.MovePosition(_rigidbody.position + displacement);*/
    }
    
    public void Teleport(Transform transformPoint)
    {
        Teleport(transformPoint.position);
    }
    public void Teleport(Vector3 position)
    {
        if (Vector3.Distance(position , transform.position) > 5)
        {
            OnTeleport?.Invoke();
        }
        transform.position = position;
       
    }
    public void TeleportWorldSpawn(Vector3 position)
    {
        if (position != Vector3.zero)
        {
            Teleport(position);
        }
    }
    
    public  void SetParentToNull()
    {
        transform.SetParent(null);
    }

    #region Focus Events / Behavior

    private bool _followTarget;
    private void FocusEnable()
    {
        _followTarget = true;
    }
    public void FocusSwitch(IFocusable focusable)
    {
        
        _transformLock = focusable.focusableTransform;
    }
    public void FocusUnLock()
    {
        _followTarget = false;
    }

    #endregion
    void DashFeedBack(bool enable)
    {
        if(enable)
        {
            Rumbler.instance.RumbleConstant(_dashRumble);
            _dashFX.Play();
            _dashTrail.emitting = true;
        }
        else
        {
            _dashTrail.emitting = false;
        }
    }
    
    void DashAttackFeedBack(bool enable)
    {
        if(enable)
        {
            Rumbler.instance.RumbleConstant(_dashRumble);
            _dashFX.Play();
            _dashTrail.emitting = true;
        }
        else
        {
            _dashTrail.emitting = false;
        }
    }
}