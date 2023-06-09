using System;
using System.Collections;
using System.Collections.Generic;
using Attack;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;

public class PlayerAttackManager : MonoBehaviour
{
    #region Variables
    [SerializeField] private List<AttackScriptableObject> playerComboAttacks;
    [SerializeField] private AttackScriptableObject dashAttackData;
    private int _currentComboIndex;
    [SerializeField] private InputButtonScriptableObject attackInput;
    [SerializeField] private InputButtonScriptableObject dashAttackInput;
    private bool _inputAttack;
    private bool _inputDashAttack;
    private float _timerCurrentAttack;
    private bool _isInCombo;
    private bool _isInDashAttack;
    public bool IsInDashAttack { get { return _isInDashAttack; } }
    [SerializeField] private Animator animator;
    private PlayerMovement _playerMovement;
    private AttackCollider _attackCollider;
    public bool DamageableWasAttackedAtThisFrame { get; private set; }
    private float _waitTimeBetweencombo = 0f;
    private float _timeToResetCombo;
    private bool _registerInputWhenNotAllowed;
    float currentDamage = 2;
    [SerializeField] private ScriptableEvent _dashAttackEvent;
    #endregion
    private void OnEnable()
    {
        attackInput.OnValueChanged += InputAttack;
        dashAttackInput.OnValueChanged += InputDashAttack;
        _dashAttackEvent.OnEvent += DashAttackAnim;
        _attackCollider.OnCollideWithIDamageable += AttackColliderOnOnCollideWithIDamageable;

    }

    private void OnDisable()
    {
        attackInput.OnValueChanged -= InputAttack;
        dashAttackInput.OnValueChanged -= InputDashAttack;
        _dashAttackEvent.OnEvent -= DashAttackAnim;
        _attackCollider.OnCollideWithIDamageable -= AttackColliderOnOnCollideWithIDamageable;
    }

    private void Update()
    {
        _timeToResetCombo -= Time.deltaTime;
        _waitTimeBetweencombo -= Time.deltaTime;
        if(_registerInputWhenNotAllowed && _waitTimeBetweencombo<0)
        {
            InputAttack(true);
        }
        if(_timeToResetCombo < 0 && !_isInCombo)
        {
            _currentComboIndex = 0;
        }
    }

    void InputAttack(bool value)
    {
        _inputAttack = value;
        if (value && !_isInCombo && _waitTimeBetweencombo < 0f && !_isInDashAttack)
        {
            StopAllCoroutines();
            StartCoroutine(CurrentAttackUpdate(playerComboAttacks[_currentComboIndex]));
            _registerInputWhenNotAllowed = false;
        }
        if(value && _waitTimeBetweencombo > 0 && !_isInDashAttack)
        {
            _registerInputWhenNotAllowed = true;
        }
    }
    void InputDashAttack(bool value)
    {
        _inputDashAttack = value;
        if(value && !_isInDashAttack && !_isInCombo)
        {
            _playerMovement.DashOfDashAttack(true);
        }
    }
    private void Awake()
    {
        _currentComboIndex = 0;
        animator.GetComponent<Animator>();
        _playerMovement = GetComponentInParent<PlayerMovement>();
        _attackCollider = GetComponentInChildren<AttackCollider>();

    }
    private void LateUpdate()
    {
        DamageableWasAttackedAtThisFrame = false;
    }
    #region attackCombo
    IEnumerator CurrentAttackUpdate(AttackScriptableObject attack)
    {
        canGiveDamage = true;
        _attackCollider.enabled = true;
        currentDamage = attack.Damage;
        _isInCombo = true;
        bool _hasPressedInputAttack = false;
        _inputAttack = false;
        _timerCurrentAttack = 0;
        switch(attack.Index)
        {
            case 1:
                _playerMovement.Attack(true, 0.2f, 2f);
                break;
            case 2:
                _playerMovement.Attack(true, 0.2f, 2f);
                break;
            case 3:
                _playerMovement.Attack(true, 0.3f, 5f);
                break;
        }
        
            
        animator.CrossFade(attack.AnimName, 0f);
        //tant qu'il est dans l'attaque et que sa durée n'est pas fini
        while (!IsAttackFinished(attack))
        {
            _timerCurrentAttack += Time.deltaTime;

            //attend la fin de l'attaque et check si il appuie sur l'input attack
            if (_inputAttack && !_hasPressedInputAttack && attack.CanAccelerate)
            {
                _hasPressedInputAttack = true;
                _currentComboIndex++;
            }
            
            //check si il peut accelere l'attack
            if (_hasPressedInputAttack && CanAccelerate(attack))
            {
                AccelerateAnimationAttack(attack);
            }
            
            yield return null;
        }


        //Attack Fini
        _isInCombo = false;
        //currentDamage = 0;
        _attackCollider.enabled = false;

        //resetAnimatorSpeed
        animator.speed = 1;

       
        canGiveDamage = false;

        //si il a essayer d'attaquer pendant l'attaque precedente, il lance le prochain.
        if (_hasPressedInputAttack && _currentComboIndex % playerComboAttacks.Count != 0 )
        {
            StartCoroutine(CurrentAttackUpdate(playerComboAttacks[_currentComboIndex % playerComboAttacks.Count]));
        }
        else
        {
           animator.CrossFade("Idle", 0.25f);
            switch(attack.Index)
            {
                case 1:
                    _timeToResetCombo = 0.2f;
                    _currentComboIndex = 1;
                        break;
                case 2:
                    _timeToResetCombo = 0.1f;
                    _currentComboIndex = 2;
                    //_waitTimeBetweencombo = 0.1f;
                    break;
                case 3:
                    _waitTimeBetweencombo = 0.2f;
                    _timeToResetCombo = 0;
                    break;

            }
            
        }
    }

    private bool IsAttackFinished(AttackScriptableObject attack)
    {
        return _timerCurrentAttack > animator.GetCurrentAnimatorStateInfo(0).length;
    }
    private bool CanAccelerate(AttackScriptableObject attack)
    {
        return _timerCurrentAttack > attack.TimeToAccelerate;
    }

    private void AccelerateAnimationAttack(AttackScriptableObject attack)
    {
        //accelerateAnimator
        animator.speed = attack.Acceleration;
    }
    #endregion

    #region dashAttack
    IEnumerator DashAttack(AttackScriptableObject dashAttack)
    {
        canGiveDamage = true;
        _attackCollider.enabled = true;
        _isInDashAttack = true;
        currentDamage = dashAttack.Damage;
        animator.CrossFade(dashAttack.AnimName, 0f);

        yield return new WaitForSeconds(0.5f);

        animator.CrossFade("Idle", 0f);
        _isInDashAttack = false;
        currentDamage = 0;
        _attackCollider.enabled = false;
        canGiveDamage = false;
    }
    void DashAttackAnim()
    {
        StartCoroutine(DashAttack(dashAttackData));
    }
    #endregion

    #region Animation Event Methods
    private void AttackColliderOnOnCollideWithIDamageable(object sender, EventArgs eventArgs)
    {
        if (eventArgs is AttackDamageableEventArgs mDamageableEventArgs && canGiveDamage)
        {
            DamageableWasAttackedAtThisFrame = true;
            mDamageableEventArgs.idamageable.DoDamage(currentDamage, _attackCollider.gameObject.transform.position);
        }
    }
    
    public void SetDamageActive(int value)
    {
        //canGiveDamage = value == 1;
        //_attackCollider.enabled = canGiveDamage;
    }

    #endregion
    
    public bool canGiveDamage { get; set; }
}
