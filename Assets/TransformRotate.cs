using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformRotate : MonoBehaviour
{
    [SerializeField] private float speed = 1;
    [SerializeField] private Vector3 rotationAmount;
   // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotationAmount * (speed * Time.deltaTime));
    }
}
