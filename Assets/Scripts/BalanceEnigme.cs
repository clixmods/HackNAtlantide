using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class BalanceEnigme : MonoBehaviour
{
    [SerializeField] private List<GameObject> targets = new List<GameObject>();
    
    [Header("VARIABLES")]
    [SerializeField] private Transform platform1;
    [SerializeField] private Transform platform2;
    [SerializeField] private Transform startPos1;
    [SerializeField] private Transform endPos1;
    [SerializeField] private Transform startPos2;
    [SerializeField] private Transform endPos2;
    [SerializeField] private float speedUp;
    [SerializeField] private float speedDown;

    private void MoveUp()
    {
        if (platform1.position.y < endPos1.position.y)
        {
            platform1.position += Vector3.up * (Time.deltaTime * speedUp);
            platform2.position += Vector3.down * (Time.deltaTime * speedDown);
        }
    }
    
    private void MoveDown()
    {
        if (platform1.position.y > startPos1.position.y)
        {
            platform1.position += Vector3.down * (Time.deltaTime * speedDown);
            platform2.position += Vector3.up * (Time.deltaTime * speedUp);
        }
    }

    private void Update()
    {
        if (targets.Count >= 5)
        {
            MoveUp();
        }
        else
        {
            MoveDown();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag($"balance")) return;
            targets.Add(other.gameObject);
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag($"balance")) return;
            targets.Remove(other.gameObject);
    }
}