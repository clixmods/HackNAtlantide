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
    
    Animator _animator;//You may not need an animator, but if so declare it here    
      
    int noOfClicks; //Determines Which Animation Will Play
    bool canClick; //Locks ability to click during animation event
    
    float lastClickedTime;
    float lastComboEnd;
    int comboCounter;
    
    public static readonly int AttackAnim = Animator.StringToHash("Attack");
    [SerializeField] private InputButtonScriptableObject _inputAttack;
    [SerializeField] private InputButtonScriptableObject _inputAttackDash;

    private IAttackCollider _attackCollider;
    
    private void OnEnable()
    {
        _inputAttack.OnValueChanged += Attack;
        _inputAttackDash.OnValueChanged += AttackDash;
    }
    private void OnDisable()
    {
        _inputAttack.OnValueChanged -= Attack;
        _inputAttackDash.OnValueChanged -= AttackDash;
    }
    
    private void Awake()
    {
        _attackCollider = GetComponentInChildren<IAttackCollider>();
        _attackCollider.OnCollideWithIDamageable += AttackColliderOnOnCollideWithIDamageable; 
    }

    void Start()
    {
        _animator = GetComponent<Animator>();
        _playerMovement = GetComponent<PlayerMovement>();
    }
    
    private void AttackColliderOnOnCollideWithIDamageable(object sender, EventArgs eventArgs)
    {
        if (eventArgs is DamageableEventArgs mDamageableEventArgs && canAttack)
        {
            mDamageableEventArgs.idamageable.DoDamage(combo[comboCounter].damage);
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
            _animator.SetInteger("intAttack", 31);
        }           
    }
    
    public void ComboCheck() {
       
        canClick = false;
        
        if(_animator.GetCurrentAnimatorStateInfo(0).IsName("Attack 1") && noOfClicks == 1 )
        {//If the first animation is still playing and only 1 click has happened, return to idle
            _animator.SetInteger("intAttack", 1); // 1 is Idle
            canClick = true;
            noOfClicks = 0;
        }
        else if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Attack 1") &&  noOfClicks >= 2)
        {//If the first animation is still playing and at least 2 clicks have happened, continue the combo           
            _animator.SetInteger("intAttack", 2); // 2 is Attack2
            canClick = true;
        }
        else if(_animator.GetCurrentAnimatorStateInfo(0).IsName("Attack 2") && noOfClicks == 2)
        {  //If the second animation is still playing and only 2 clicks have happened, return to idle          
            _animator.SetInteger("intAttack", 1); // 1 is Idle
            canClick = true;
            noOfClicks = 0;
        }
        else if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Attack 2") && noOfClicks >= 3)
        {  //If the second animation is still playing and at least 3 clicks have happened, continue the combo          
            _animator.SetInteger("intAttack", 3); // 3 is Attack 3
            canClick = true;            
        }
        else if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Attack 3"))
        { //Since this is the third and last animation, return to idle           
            _animator.SetInteger("intAttack", 1); // 1 is Idle
            canClick = true;
            noOfClicks = 0;
        }       
    }
    
    void Attack(bool value)
    {
        if (value)
        {
            if (Time.time - lastComboEnd > 0.5f && comboCounter <= combo.Count)
            {
                CancelInvoke("EndCombo");

                if (Time.time - lastClickedTime >= 0.2f)
                {
                    anim.runtimeAnimatorController = combo[comboCounter].animatorOV;
                    _attackCollider.enabled = false;
                    anim.Play(AttackAnim, 0, 0);
                    comboCounter++;
                    lastClickedTime = Time.time;

                    if (comboCounter + 1 > combo.Count)
                    {
                        comboCounter = 0;
                    }
                }
            }
            ExitAttack();
        }
    }
    
    void AttackDash(bool value)
    {
        if (value)
        {
            if (Time.time - lastClickedTime >= 0.2f)
            {
                _attackCollider.enabled = false;
                anim.Play(AttackAnim, 0, 0);
                _playerMovement.Dash(true);
                lastClickedTime = Time.time;
            }
        }
    }

    void ExitAttack()
    {
        if(_animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && _animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            Invoke("EndCombo", 1);
        }
    }

    void EndCombo()
    {
        comboCounter = 0;
        lastComboEnd = Time.time;
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