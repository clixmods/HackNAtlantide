using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    //General
    [Header("GENERAL")]
    [Space(5)]

    private Animator _animator;
    private Rigidbody _rigidbody;
    private Camera _camera;
    [SerializeField] private PlayerMovementStateScriptableObject _playerMovementState;
    [SerializeField] private PlayerInstanceScriptableObject _playerInstanceSO;


    //Input
    [Header("INPUT")]
    [Space(5)]
    [SerializeField] private InputVectorScriptableObject _moveInput;
    [SerializeField] private InputButtonScriptableObject _dashInput;
    private Vector3 _moveDirection;


    //Walk
    [Header("WALK")]
    [Space(5)]

    [SerializeField] private float _moveSpeed;
    private float _speed;
    private Vector3 _smoothMoveVelocity;
    private Vector3 _moveAmount;
    [SerializeField] private float _smoothTimeAcceleration;
    [SerializeField] private float _smoothTimeAccelerationDash;


    //rotation lookat
    [Header("LOOK")]
    [Space(5)]

    [SerializeField] private float _rotationSpeed;
    Quaternion _targetRotation;


    //Dash
    [Header("DASH")]
    [Space(5)]

    [SerializeField] private float _dashTime;
    [SerializeField] private float _dashSpeed;
    [SerializeField] private float _dashReloadTime;
    [SerializeField] EventFloatScriptableObject _dashEvent;
    [SerializeField] PlayerStaminaScriptableObject _playerStamina;
    private bool _canDash;
    private bool _isDashing;
    
    //Collision Detection
    private Collider[] _buffer = new Collider[8];
    new CapsuleCollider collider;

    //LockForCombat
    Transform _transformLock = null;
    Transform _transformLockTempForDash;

    [Header("FeedBack")]
    [SerializeField] RumblerDataConstant _dashRumble;

    // Start is called before the first frame update
    void Awake()
    {
        _animator = GetComponent<Animator>();
        PlayerInstanceScriptableObject.Player = this.gameObject;
        _rigidbody = GetComponent<Rigidbody>();
        _camera = Camera.main;
        collider = GetComponent<CapsuleCollider>();
        _speed = _moveSpeed;
        _canDash = true;
    }
    void OnEnable()
    {
        _moveInput.OnValueChanged += MoveInput;
        _dashInput.OnValueChanged += Dash;
        Focus.OnFocusEnable += FocusEnable;
        Focus.OnFocusSwitch += FocusSwitch;
        Focus.OnFocusDisable += FocusUnLock;
    }

   

    private void OnDisable()
    {
        _moveInput.OnValueChanged -= MoveInput;
        _dashInput.OnValueChanged += Dash;
        Focus.OnFocusEnable -= FocusEnable;
        Focus.OnFocusSwitch -= FocusSwitch;
        Focus.OnFocusDisable -= FocusUnLock;
    }
    void MoveInput(Vector2 direction)
    {
        _animator.SetFloat("RunningSpeed", Mathf.Abs(direction.x) + Mathf.Abs(direction.y));
        //Projects the camera forward on 2D horizontal plane
        Vector3 camForwardOnPlane = new Vector3(_camera.transform.forward.x, 0, _camera.transform.forward.z).normalized;
        Vector3 camRightOnPlane = new Vector3(_camera.transform.right.x, 0, _camera.transform.right.z).normalized;
        _moveDirection = direction.x * camRightOnPlane + direction.y * camForwardOnPlane;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
    }
    private void Move()
    {
        //direction in which player wants to move
        Vector3 targetmoveAmount = _moveDirection * _speed * Time.fixedDeltaTime;

        if(_isDashing)
        {
            //calculated direction based of his movedirection of the precedent frame
            _moveAmount = Vector3.SmoothDamp(_moveAmount, targetmoveAmount, ref _smoothMoveVelocity, _smoothTimeAccelerationDash);

            _playerMovementState.MovementState = MovementState.dashing;
        }
        else
        {
            //calculated direction based of his movedirection of the precedent frame
            _moveAmount = Vector3.SmoothDamp(_moveAmount, targetmoveAmount, ref _smoothMoveVelocity, _smoothTimeAcceleration);

            _playerMovementState.MovementState = _moveAmount.sqrMagnitude > 0.0005f ? MovementState.running : MovementState.idle;
        }
        

        //Move the object with the RigidBody
        MoveSweepTestRecurs(_moveAmount, 3);

        //Rotate the player by his direction
        LookAtDirection(_isDashing?_rotationSpeed*20:_rotationSpeed, targetmoveAmount);

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
        }
        _rigidbody.MovePosition(_rigidbody.position + displacement);

        //Keep the player on ground
        StayGrounded();

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

            if (Physics.ComputePenetration(collider, _rigidbody.position, _rigidbody.rotation,
                                       _buffer[i], _buffer[i].transform.position, _buffer[i].transform.rotation,
                                       out Vector3 direction, out float distance))
            {
                _rigidbody.MovePosition(_rigidbody.position + (direction * (distance)));
            }
        }

        amount = Physics.OverlapCapsuleNonAlloc(bottom, top, collider.radius, _buffer);
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

    private void Dash(bool value)
    {
        if (_canDash && _playerStamina.CanUseStamina()&& _moveDirection.sqrMagnitude > 0.1f)
        {
            _playerStamina.UseStamina(1);
            _speed = _dashSpeed;
            _isDashing = true;
            _canDash = false;
            _transformLockTempForDash = _transformLock;
            _transformLock = null;
            StartCoroutine(CancelDash());

            //FeedBack
            Rumbler.instance.RumbleConstant(_dashRumble);
        }
    }

    //sets the speeds value to dash
    IEnumerator CancelDash()
    {
        yield return new WaitForSeconds(_dashTime);
        _speed = _moveSpeed;
        _isDashing = false;
        _transformLock = _transformLockTempForDash;
        StartCoroutine(ReloadDash());
    }

    //allows player to dash again
    IEnumerator ReloadDash()
    {
        yield return new WaitForSeconds(_dashReloadTime);
        _canDash = true;
    }
    private void StayGrounded()
    {
        float distance = 0f;
        Vector3 displacement = Vector3.zero;
        if (_rigidbody.SweepTest(Vector3.down*0.1f, out RaycastHit hit, 0.5f, QueryTriggerInteraction.Ignore))
        {
            //ClampDistance with contact offset;
            distance = Mathf.Max(0f, hit.distance - Physics.defaultContactOffset);
            displacement = Vector3.down * distance;
        }
        _rigidbody.MovePosition(_rigidbody.position + displacement);
    }
    public void Teleport(Transform transformPoint)
    {
        transform.position = transformPoint.position;
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
    public void FocusSwitch(Transform transform)
    {
        _transformLock = transform;
    }
    public void FocusUnLock()
    {
        _followTarget = false;
    }

    #endregion
}
