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
    private bool _isDashing;

    private Collider[] _buffer = new Collider[8];
    new CapsuleCollider collider;


    // Start is called before the first frame update
    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _camera = Camera.main;
        collider = GetComponent<CapsuleCollider>();
        _speed = _moveSpeed;
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
        ExtractFromColliders();
    }
    private void Move()
    {
        //direction in which player wants to move
        Vector3 targetmoveAmount = _moveDirection * _speed * Time.fixedDeltaTime;

        //calcultaed direction based of his movedirection of the precedent fralme
        _moveAmount = Vector3.SmoothDamp(_moveAmount, targetmoveAmount, ref _smoothMoveVelocity, _isDashing?_smoothTimeAccelerationDash:_smoothTimeAcceleration);

        //Move the object with the RigidBody
        MoveSweepTestRecurs(_moveAmount, 3);
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
                _rigidbody.MovePosition(_rigidbody.position + (direction * (distance + Physics.defaultContactOffset)));
            }
        }

        amount = Physics.OverlapCapsuleNonAlloc(bottom, top, collider.radius, _buffer);
    }

    private void Dash(bool value)
    {
        _speed = _dashSpeed;
        _isDashing = true;
        StartCoroutine(CancelDash());
    }
    
    IEnumerator CancelDash()
    {
        yield return new WaitForSeconds(_dashTime);
        _speed = _moveSpeed;
        _isDashing = false;
    }

}
