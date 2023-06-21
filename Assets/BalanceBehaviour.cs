using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BalanceBehaviour : MonoBehaviour
{
    public float rightWeight;
    public float leftWeight;
    [SerializeField] float leftRotation;
    [SerializeField] float rightRotation;
    [SerializeField] float rotationSpeed;
    float acceleration;
    float currentRotation;
    float startRotationY;
    Rigidbody _rigidbody;

    float xRotation;
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        xRotation = _rigidbody.rotation.eulerAngles.x - 360;
        startRotationY = transform.rotation.eulerAngles.y;
    }

    private void Update()
    {
        if (leftWeight > rightWeight)
        {
            BalanceLeft();
        }
        else
        {
            BalanceRight();
        }
    }


    void BalanceRight()
    {
        xRotation -= Time.deltaTime * (rotationSpeed * Mathf.Pow((rightWeight / (leftWeight + rightWeight)),2));
        xRotation = Mathf.Clamp(xRotation, rightRotation, leftRotation);
        _rigidbody.rotation = Quaternion.Euler(xRotation, startRotationY, 0);
    }
    void BalanceLeft()
    {
        xRotation += Time.deltaTime * (rotationSpeed * Mathf.Pow((leftWeight / (leftWeight + rightWeight)), 2));
        xRotation = Mathf.Clamp(xRotation, rightRotation, leftRotation);
        _rigidbody.rotation = Quaternion.Euler(xRotation, startRotationY, 0);
    }
}