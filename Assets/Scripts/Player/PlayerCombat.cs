using System;
using System.Collections;
using System.Collections.Generic;
using Attack;
using UnityEngine;

public class PlayerCombat : MonoBehaviour,ICombat
{
    private PlayerMovement _playerMovement;
    public List<AttackSO> combo;
    float lastClickedTime;
    float lastComboEnd;
    int comboCounter;
    
    public static readonly int AttackAnim = Animator.StringToHash("Attack");
    Animator anim;
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

    private void AttackColliderOnOnCollideWithIDamageable(object sender, EventArgs eventArgs)
    {
        if (eventArgs is DamageableEventArgs mDamageableEventArgs && canAttack)
        {
            mDamageableEventArgs.idamageable.DoDamage( combo[comboCounter].damage);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        _playerMovement = GetComponent<PlayerMovement>();
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
        if(anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
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