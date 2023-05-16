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
        GameStateManager.Instance.deadStateObject.SetActive(true);
        base.Dead();
        GetComponentInChildren<Animator>().CrossFade("Dead_Chara_Sword",0.01f);
        
        // TODO - Make the dead function
    }
    public override void DoDamage(float damage , Vector3 attackLocation)
    {
        if(_movementState.MovementState != MovementState.dashing)
        {
            base.DoDamage(damage,  attackLocation);
        }
        else
        {
            Debug.Log("dashing no damage");
        }
        
    }
}
