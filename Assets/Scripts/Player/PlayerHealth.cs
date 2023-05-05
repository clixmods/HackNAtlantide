using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class PlayerHealth : Character
{
    [SerializeField] PlayerMovementStateScriptableObject _movementState;
    public override void Dead()
    {
        base.Dead();
        
        // TODO - Make the dead function
    }
    public override void DoDamage(float damage)
    {
        if(_movementState.MovementState != MovementState.dashing)
        {
            base.DoDamage(damage);
        }
        else
        {
            Debug.Log("dashing no damage");
        }
        
    }
}
