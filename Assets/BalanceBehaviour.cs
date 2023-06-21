using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    /*[SerializeField] Transform cubeWeightLeft;
    [SerializeField] Transform cubeWeightRight;
    float startCubeWeightPosYLeft;
    float startCubeWeightPosYRight;
    [SerializeField] float maxYSizeCubeFeedback = 10;*/
    [SerializeField] TextMeshProUGUI _leftWeightText;
    [SerializeField] TextMeshProUGUI _rightWeightText;
    float xRotation;


    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        xRotation = _rigidbody.rotation.eulerAngles.x - 360;
        startRotationY = transform.rotation.eulerAngles.y;
        /*startCubeWeightPosYLeft = cubeWeightLeft.localPosition.y;
        startCubeWeightPosYRight = cubeWeightRight.localPosition.y;*/
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
        /*cubeWeightLeft.transform.localScale = new Vector3(cubeWeightLeft.transform.localScale.x, Mathf.Lerp(0,maxYSizeCubeFeedback, (leftWeight / (leftWeight + rightWeight)))/2f, cubeWeightLeft.transform.localScale.z);
        cubeWeightLeft.transform.localPosition = new Vector3(cubeWeightLeft.transform.localPosition.x, Mathf.Lerp(startCubeWeightPosYLeft + maxYSizeCubeFeedback/4f, startCubeWeightPosYLeft - maxYSizeCubeFeedback / 4f, (leftWeight / (leftWeight + rightWeight))), cubeWeightLeft.transform.localPosition.z);*/
    }
    private void LateUpdate()
    {
        float currentleftweight = leftWeight;
        _leftWeightText.text = "";
        while (currentleftweight >=2)
        {
            currentleftweight -= 21;
            _leftWeightText.text += "I";
        }
        float currentrightweight = rightWeight;
        _rightWeightText.text = "";
        while (currentrightweight >= 2)
        {
            currentrightweight -= 21;
            _rightWeightText.text += "I";
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