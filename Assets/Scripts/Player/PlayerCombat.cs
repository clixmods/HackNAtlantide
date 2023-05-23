using System;
using System.Collections;
using System.Collections.Generic;
using Attack;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerCombat : MonoBehaviour,ICombat
{
    [Header("Scripts Refs"), SerializeField]
    private PlayerMovement _playerMovement;

    private PlayerAnimations _playerAnimations;
    
    Animator _animator;//You may not need an animator, but if so declare it here    
    
    [Header("Variables")]
    [SerializeField] int noOfClicks; //Determines Which Animation Will Play
    private float _lastClickedTime = 0f;
    private float comboDelay = .3f;
    [SerializeField] private bool stopAnimation;
    
    // TODO - TEMPORARY
    private float damage = 1f;
    
    [SerializeField] private InputButtonScriptableObject _inputAttack;
    [SerializeField] private InputButtonScriptableObject _inputDashAttack;

    private IAttackCollider _attackCollider;
    [SerializeField] TrailRenderer _trailSwordDistortion;

    [SerializeField] private ScriptableEvent _dashAttackEvent;

    private void OnEnable()
    {
        _inputAttack.OnValueChanged += Attack;
        _inputDashAttack.OnValueChanged += DashAttack;
        _dashAttackEvent.OnEvent += DashAttackAnim;
    }
    private void OnDisable()
    {
        _inputAttack.OnValueChanged -= Attack;
        _inputDashAttack.OnValueChanged -= DashAttack;
        _dashAttackEvent.OnEvent -= DashAttackAnim;
    }
    
    private void Awake()
    {
        _attackCollider = GetComponentInChildren<IAttackCollider>();
        _attackCollider.OnCollideWithIDamageable += AttackColliderOnOnCollideWithIDamageable;
        _animator = GetComponentInChildren<Animator>();
        _playerMovement = GetComponentInParent<PlayerMovement>();
        _playerAnimations = GetComponent<PlayerAnimations>();
        
        noOfClicks = 0;
    }

    private void Update()
    {
        if (stopAnimation)
        {
            _animator.enabled = false;
        }
        if (Time.time - _lastClickedTime > comboDelay)
        {
            noOfClicks = 0;
        }
        if (noOfClicks > 0)
        {
            _animator.SetBool("IdleReturn", false);
        }
        else if (noOfClicks <= 0)
        {
            _animator.SetBool("IdleReturn", true);
        }
    }

    private void AttackColliderOnOnCollideWithIDamageable(object sender, EventArgs eventArgs)
    {
        if (eventArgs is AttackDamageableEventArgs mDamageableEventArgs && canAttack)
        {
            mDamageableEventArgs.idamageable.DoDamage(damage, _attackCollider.gameObject.transform.position);
        }
    }
    
    void Attack(bool value)
    {
        if (value)
        {
            _lastClickedTime = Time.time;
            noOfClicks++;

            if (noOfClicks == 1)
            {
                _animator.SetTrigger("Attack1");
            }

            noOfClicks = Mathf.Clamp(noOfClicks, 0, 3);
        }
    }

    public void ComboAttack1Transition()
    {
        if (noOfClicks < 2) return;
        _animator.SetTrigger("Attack2");
    }
    
    public void ComboAttack2Transition()
    {
        if (noOfClicks < 3) return;
        _animator.SetTrigger("Attack3");
    }
    
    void DashAttack(bool value)
    {
        if (value)
        {
            StartCoroutine(DashAttackCoroutine());
        }
    }

    void DashAttackAnim()
    {
        StartCoroutine(DashAttackBoolean());
    }

    IEnumerator DashAttackBoolean()
    {
        _animator.SetBool("dashAttack", true);
        yield return new WaitForSeconds(.5f);
        _animator.SetBool("dashAttack", false);
    }

    IEnumerator DashAttackCoroutine()
    {
        _playerMovement.DashOfDashAttack(true);
        yield return new WaitForSeconds(.4f);
        _playerAnimations.TimeBeforeIdle = 5f;
        noOfClicks = 0;
        // canClick = true;
    }

    #region Animation Event Methods

    public void SetDamageActive(int value)
    {
        canAttack = value == 1;
        _attackCollider.enabled = canAttack;

        _trailSwordDistortion.emitting = canAttack;
    }

    #endregion
    
    public bool canAttack { get; set; }
}