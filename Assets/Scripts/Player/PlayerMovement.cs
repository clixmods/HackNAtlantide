using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private Camera _camera;
    //Input
    [SerializeField] private InputVectorScriptableObject _moveInput;
    [SerializeField] private InputButtonScriptableObject _dashInput;
    private Vector3 _moveDirection;

    [SerializeField] private float _moveSpeed;
    private float _speed;
    private Vector3 _smoothMoveVelocity;
    private Vector3 _moveAmount;
    [SerializeField] private float _smoothTimeAcceleration;
    [SerializeField] private float _smoothTimeAccelerationDash;
    [SerializeField] private float _dashTime;
    [SerializeField] private float _dashSpeed;
    [SerializeField] private float _dashReloadTime;
    private bool _canDash;
    private bool _isDashing;
    private float _rotationSpeed;
    Quaternion _targetRotation;

    private Collider[] _buffer = new Collider[8];
    new CapsuleCollider collider;
    [SerializeField] private LayerMask _layerEnvironnement;


    // Start is called before the first frame update
    void Awake()
    {
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
    }
    private void OnDisable()
    {
        _moveInput.OnValueChanged -= MoveInput;
        _dashInput.OnValueChanged += Dash;
    }
    void MoveInput(Vector2 direction)
    {
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

        //calculated direction based of his movedirection of the precedent frame, different smooth time if he is dashing.
        _moveAmount = Vector3.SmoothDamp(_moveAmount, targetmoveAmount, ref _smoothMoveVelocity, _isDashing?_smoothTimeAccelerationDash:_smoothTimeAcceleration);

        //Move the object with the RigidBody
        MoveSweepTestRecurs(_moveAmount, 3);

        //Rotate the player by his direction
        LookAtDirection(5, targetmoveAmount);

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

        velocity -= displacement;
        velocity -= hit.normal * Vector3.Dot(velocity, hit.normal);


        //Force Player To StayOnGround
        /*RaycastHit groundHit;
        float halfHeight = (collider.height * 0.5f);
        Vector3 bottom = collider.bounds.center + (Vector3.down * halfHeight);

        Debug.DrawRay(bottom, Vector3.down);
        if (Physics.Raycast(bottom, Vector3.down, out groundHit,100f, _layerEnvironnement))
        {
            Vector3 direction = groundHit.collider.transform.position - bottom;
            _rigidbody.MovePosition(_rigidbody.position + direction * (1 - Physics.defaultContactOffset));
        }*/

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
        if (direction.magnitude > 0.001f)
        {
            _targetRotation = Quaternion.LookRotation(direction, transform.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, _targetRotation, speed * Time.deltaTime);
        }
    }

    private void Dash(bool value)
    {
        if (_canDash)
        {
            _speed = _dashSpeed;
            _isDashing = true;
            _canDash = false;
            StartCoroutine(CancelDash());
        }
    }

    //sets the speeds value to dash
    IEnumerator CancelDash()
    {
        yield return new WaitForSeconds(_dashTime);
        _speed = _moveSpeed;
        _isDashing = false;
        StartCoroutine(ReloadDash());
    }

    //allows player to dash again
    IEnumerator ReloadDash()
    {
        yield return new WaitForSeconds(_dashReloadTime);
        _canDash = true;
    }
}
