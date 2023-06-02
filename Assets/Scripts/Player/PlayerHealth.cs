using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : Character
{
    [SerializeField] PlayerMovementStateScriptableObject _movementState;
    [SerializeField] PostProcessWeightTransition _postProcessWeightTransition;
    [SerializeField] private ScriptableValueFloat healthValue;
    public UnityEvent HealthGain;
    public UnityEvent HealthGainFull;
    public UnityEvent MaxHealthIncrease;
    
    public override float health
    {
        get => healthValue.Value;
        protected set
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
    
    public bool IsFullHealth => Math.Abs(health - maxHealth) < 0.05f;


    bool _isInvincible = false;
    public bool IsInvincible { get { return _isInvincible; } set { _isInvincible = value; } }
    private void Start()
    {
        _postProcessWeightTransition.SetWeightVolume(0);
    }

    private void OnEnable()
    {
        healthValue.OnMaxValueChanged += OnMaxValueChanged;
    }
    private void OnDisable()
    {
        healthValue.OnMaxValueChanged -= OnMaxValueChanged;
    }
    private void OnMaxValueChanged(float maxValue)
    {
        MaxHealthIncrease?.Invoke();
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
        //healthValue.OnMaxValueChanged += MaxValueChanged;
        //healthValue.OnValueChanged += ValueChanged;
        return;
        base.InitHealth();
    }

    private void ValueChanged(float value)
    {
        //OnChangeHealth?.Invoke(health,maxHealth);
    }

    private void MaxValueChanged(float maxValue)
    {
       // throw new NotImplementedException();
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
        if (IsFullHealth)
        {
            return;
        }
        base.AddHealth(amount);
     
        HealthGain?.Invoke();
        if (IsFullHealth)
        {
            HealthGainFull?.Invoke();
        }
    }
    
    IEnumerator Hit()
    {
        _postProcessWeightTransition.SetWeightVolume(1,0.1f);
        float timescale = GameStateManager.Instance.GameStateOverride.timeScale;
        _isInvincible = true;
        Time.timeScale = 0.2f;
        yield return new WaitForSecondsRealtime(0.15f);
        if(!GameStateManager.Instance.pauseStateObject.activeSelf)
        {
            Time.timeScale = timescale;
        }
        _postProcessWeightTransition.SetWeightVolume(0,0.1f);
        yield return new WaitForSecondsRealtime(0.15f);
        _isInvincible = false;
    }

}
