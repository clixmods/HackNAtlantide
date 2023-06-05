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

    float xRotation;
    private void Start()
    {
        xRotation = transform.rotation.eulerAngles.x - 360;
        startRotationY = transform.rotation.eulerAngles.y;
    }

    private void Update()
    {
        if (leftWeight > rightWeight)
        {
            BalanceLeft();
        }
        else if( leftWeight < rightWeight)
        {
            BalanceRight();
        }
    }


    void BalanceRight()
    {
        xRotation -= Time.deltaTime * rotationSpeed;
        xRotation = Mathf.Clamp(xRotation, rightRotation, leftRotation);
        transform.rotation = Quaternion.Euler(xRotation, startRotationY, 0);
    }
    void BalanceLeft()
    {
        xRotation += Time.deltaTime * rotationSpeed;
        xRotation = Mathf.Clamp(xRotation, rightRotation, leftRotation);
        transform.rotation = Quaternion.Euler(xRotation, startRotationY, 0);
    }
}