using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class PlayerHealth : Character
{
    [SerializeField] PlayerMovementStateScriptableObject _movementState;
    [SerializeField] PostProcessWeightTransition _postProcessWeightTransition;
    private void Start()
    {
        _postProcessWeightTransition.SetWeightVolume(0);
    }
    public override void Dead()
    {
        GameStateManager.Instance.deadStateObject.SetActive(true);
        base.Dead();
        GetComponentInChildren<Animator>().CrossFade("Mort_Chara_Sword",0.01f);
        
        // TODO - Make the dead function
    }
    public override void DoDamage(float damage , Vector3 attackLocation)
    {
        if(_movementState.MovementState != MovementState.dashing)
        {
            base.DoDamage(damage,  attackLocation);
            StartCoroutine(PostProcessHit());
        }
        
    }
    IEnumerator PostProcessHit()
    {
        _postProcessWeightTransition.SetWeightVolume(1);
        yield return new WaitForSecondsRealtime(0.3f);
        _postProcessWeightTransition.SetWeightVolume(0);
    }
}
