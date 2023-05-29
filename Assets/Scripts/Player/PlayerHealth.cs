using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PlayerHealth : Character
{
    [SerializeField] PlayerMovementStateScriptableObject _movementState;
    [SerializeField] PostProcessWeightTransition _postProcessWeightTransition;
    [SerializeField] private ScriptableValueFloat healthValue;
    public UnityEvent HealthGain;
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
            StartCoroutine(Hit());
        }
        
    }

    public override void AddHealth(float amount)
    {
        base.AddHealth(amount);
        HealthGain?.Invoke();
    }


    IEnumerator Hit()
    {
        _postProcessWeightTransition.SetWeightVolume(1,0.1f);
        float timescale = GameStateManager.Instance.GameStateOverride.timeScale;
        _isInvincible = true;
        Time.timeScale = 0.2f;
        yield return new WaitForSecondsRealtime(0.15f);
        Time.timeScale = timescale;
        _postProcessWeightTransition.SetWeightVolume(0,0.1f);
        yield return new WaitForSecondsRealtime(0.15f);
        _isInvincible = false;
    }

}
