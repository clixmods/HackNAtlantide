using System;
using System.Collections;
using System.Collections.Generic;
using Attack;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerCombat : MonoBehaviour,ICombat
{
    [Header("Scripts Refs")]
    private PlayerMovement _playerMovement;

    private PlayerAnimations _playerAnimations;
    
    Animator _animator;//You may not need an animator, but if so declare it here    
    
    [Header("Variables")]
    [SerializeField] int noOfClicks; //Determines Which Animation Will Play
    [SerializeField] bool canClick; //Locks ability to click during animation event
    float lastClickedTime;
    float lastComboEnd;
    int comboCounter;
    [SerializeField] private bool stopAnimation;
    
    // TODO - TEMPORARY
    private float damage = 1f;
    
    [SerializeField] private InputButtonScriptableObject _inputAttack;

    private IAttackCollider _attackCollider;
    private int INTAttack = Animator.StringToHash("intAttack");

    private void OnEnable()
    {
        _inputAttack.OnValueChanged += Attack;
    }
    private void OnDisable()
    {
        _inputAttack.OnValueChanged -= Attack;
    }
    
    private void Awake()
    {
        _attackCollider = GetComponentInChildren<IAttackCollider>();
        _attackCollider.OnCollideWithIDamageable += AttackColliderOnOnCollideWithIDamageable;
        _animator = GetComponentInChildren<Animator>();
        _playerMovement = GetComponent<PlayerMovement>();
        _playerAnimations = GetComponent<PlayerAnimations>();
        
        noOfClicks = 0;
        canClick = true;
    }

    private void Update()
    {
        if (stopAnimation)
        {
            _animator.enabled = false;
        }
    }

    private void AttackColliderOnOnCollideWithIDamageable(object sender, EventArgs eventArgs)
    {
        if (eventArgs is DamageableEventArgs mDamageableEventArgs && canAttack)
        {
            mDamageableEventArgs.idamageable.DoDamage(damage);
        }
    }
    
    void Attack(bool value)
    {
        if (value)
        {
            ComboStarter();
        }
    }
    
    void ComboStarter()
    {       
        if (canClick)            
        {            
            noOfClicks++;
        }
        
        if (noOfClicks == 1)
        {            
            _animator.SetInteger(INTAttack, 1); // fait attaque 1
        }           
    }
    
    public void ComboCheck()
    {
        canClick = false;
        
        if(_animator.GetCurrentAnimatorStateInfo(0).IsName("Attaque_Chara_Sword_1") && noOfClicks == 1)
        { //If the first animation is still playing and only 1 click has happened, return to idle
            _animator.SetInteger(INTAttack, 0); // 0 is Idle
            canClick = true;
            noOfClicks = 0;
            _playerAnimations.TimeBeforeIdle = 5f;
        }
        else if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Attaque_Chara_Sword_1") && noOfClicks >= 2)
        { //If the first animation is still playing and at least 2 clicks have happened, continue the combo
            _animator.SetInteger(INTAttack, 2); // 2 is Attack2
            canClick = true;
        }
        else if(_animator.GetCurrentAnimatorStateInfo(0).IsName("Attaque_Chara_Sword_2") && noOfClicks == 2)
        { //If the second animation is still playing and only 2 clicks have happened, return to idle
            _animator.SetInteger(INTAttack, 0); // 0 is Idle
            canClick = true;
            noOfClicks = 0;
            _playerAnimations.TimeBeforeIdle = 5f;
        }
        else if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Attaque_Chara_Sword_2") && noOfClicks >= 3)
        { //If the second animation is still playing and at least 3 clicks have happened, continue the combo
            _animator.SetInteger(INTAttack, 3); // 3 is Attack 3
            canClick = true;
        }
        else if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Attaque_Chara_Sword_3"))
        { //Since this is the third and last animation, return to idle
            _animator.SetInteger(INTAttack, 0); // 0 is Idle
            canClick = true;
            noOfClicks = 0;
            _playerAnimations.TimeBeforeIdle = 5f;
        }
    }

    #region Animation Event Methods

    public void SetDamageActive(int value)
    {
        canAttack = value == 1;
        _attackCollider.enabled = canAttack;
    }

    #endregion
    
    public bool canAttack { get; set; }
}