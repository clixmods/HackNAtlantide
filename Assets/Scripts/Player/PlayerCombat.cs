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
    private float comboDelay = .4f;
    [SerializeField] private bool stopAnimation;
    
    // TODO - TEMPORARY
    private float _damage = 1f;
    
    [SerializeField] private InputButtonScriptableObject _inputAttack;
    [SerializeField] private InputButtonScriptableObject _inputDashAttack;

    private IAttackCollider _attackCollider;
    [SerializeField] TrailRenderer _trailSwordDistortion;

    [SerializeField] private ScriptableEvent _dashAttackEvent;
    
    public bool IsDashingAttack => _animator.GetBool("dashAttack");
    public bool DamageableWasAttackedAtThisFrame { get; private set; }
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
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Attaque_Chara_Sword_1"))
        {
            _damage = 1f;
        }
        else if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Attaque_Chara_Sword_2"))
        {
            _damage = 1.5f;
        }
        else if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Attaque_Chara_Sword_3") || _animator.GetCurrentAnimatorStateInfo(0).IsName("Dash_Attack_Chara") || _animator.GetCurrentAnimatorStateInfo(0).IsName("Dash_Attack_Chara_2"))
        {
            _damage = 2f;
        }
        
        if (stopAnimation)
        {
            _animator.enabled = false;
        }
        if (Time.time - _lastClickedTime > comboDelay)
        {
            noOfClicks = 0;
        }
    }

    private void LateUpdate()
    {
        DamageableWasAttackedAtThisFrame = false;
    }

    private void AttackColliderOnOnCollideWithIDamageable(object sender, EventArgs eventArgs)
    {
        if (eventArgs is AttackDamageableEventArgs mDamageableEventArgs && canGiveDamage)
        {
            DamageableWasAttackedAtThisFrame = true;
            mDamageableEventArgs.idamageable.DoDamage(_damage, _attackCollider.gameObject.transform.position);
            
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

    public void ResetNoOfClicks()
    {
      
        noOfClicks = 0;
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
        yield return new WaitForSeconds(.25f);
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
        canGiveDamage = value == 1;
        _attackCollider.enabled = canGiveDamage;

        _trailSwordDistortion.emitting = canGiveDamage;
    }

    #endregion
    
    public bool canGiveDamage { get; set; }
}