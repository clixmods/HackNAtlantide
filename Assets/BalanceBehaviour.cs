using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BalanceBehaviour : MonoBehaviour
{
    public float rightWeight;
    public float leftWeight;

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

    void BalanceLeft()
    {
        //QuaternionSlerp... point a to point b
    }
    void BalanceRight()
    {
        //QuaternionSlerp... point a to point b
    }
}