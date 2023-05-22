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
    [SerializeField] private ScriptableValueFloat healthValue;
    public override float health
    {
        get => healthValue.Value; 
        set
        {
            base.health = value;
            healthValue.Value = value; 
        }
    }
    public override float maxHealth 
    {
        get
        {
            return healthValue.MaxValue;
        } 
        set
        {
            base.maxHealth = value;
            healthValue.MaxValue = value; 
        }
    } 


    bool _isInvincible = false;
    public bool IsInvincible { get { return _isInvincible; } set { _isInvincible = value; } }
    private void Start()
    {
        _postProcessWeightTransition.SetWeightVolume(0);
    }

    protected override void InitHealth()
    {
        // Scriptable Value Init the health
        if (healthValue.Value <= 0)
        {
            healthValue.Value = healthValue.DefaultValue;
        }
        health = healthValue.Value;
        maxHealth = healthValue.MaxValue;
        return;
        base.InitHealth();
    }

    public override void Dead()
    {
        GameStateManager.Instance.deadStateObject.SetActive(true);
        base.Dead();
        GetComponentInChildren<Animator>().CrossFade("Mort_Chara_Sword",0.01f);
    }
    public override void DoDamage(float damage , Vector3 attackLocation)
    {
        if(_movementState.MovementState != MovementState.dashing && !_isInvincible)
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
