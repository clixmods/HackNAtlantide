using System;
using System.Collections;
using System.Collections.Generic;
using Attack;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;

public class PlayerAttackManager : MonoBehaviour
{
    [SerializeField] private List<AttackScriptableObject> playerComboAttacks;
    private int _currentComboIndex;
    [SerializeField] private InputScriptableObject<bool> attackInput;
    private bool _inputAttack;
    private float _timerCurrentAttack;
    private bool _isInCombo;
    [SerializeField] private Animator animator;
    private IAttackCollider _attackCollider;
    public bool DamageableWasAttackedAtThisFrame { get; private set; }
    private float _waitTimeBetweencombo = 0f;
    private void OnEnable()
    {
        attackInput.OnValueChanged += InputAttack;
    }

    private void OnDisable()
    {
        attackInput.OnValueChanged -= InputAttack;
    }

    private void Update()
    {
        _waitTimeBetweencombo -= Time.deltaTime;
    }

    void InputAttack(bool value)
    {
        _inputAttack = value;
        if (value && !_isInCombo && _waitTimeBetweencombo < 0f)
        {
            StopAllCoroutines();
            StartCoroutine(CurrentAttackUpdate(playerComboAttacks[0],false));
        }
    }
    private void Awake()
    {
        _currentComboIndex = 0;
        animator.GetComponent<Animator>();
        
        _attackCollider = GetComponentInChildren<IAttackCollider>();
        _attackCollider.OnCollideWithIDamageable += AttackColliderOnOnCollideWithIDamageable;
    }
    private void LateUpdate()
    {
        DamageableWasAttackedAtThisFrame = false;
    }
    // ReSharper disable Unity.PerformanceAnalysis
    IEnumerator CurrentAttackUpdate(AttackScriptableObject attack, bool fromCombo)
    {
        _isInCombo = true;
        bool _hasPressedInputAttack = false;
        _inputAttack = false;
        _timerCurrentAttack = 0;
            
        animator.CrossFade(attack.AnimName, 0f);
        //tant qu'il est dans l'attaque et que sa durée n'est pas fini
        while (!IsAttackFinished(attack))
        {
            //attend la fin de l'attaque et check si il appuie sur l'input attack
            _timerCurrentAttack += Time.deltaTime; //* attack.Speed;
            if (_inputAttack && !_hasPressedInputAttack && attack.CanRegisterInput)
            {
                _hasPressedInputAttack = true;
                _currentComboIndex++;
            }
            
            //check si il peut accelere l'attack
            if (_hasPressedInputAttack && CanAccelerate(attack))
            {
                AccelerateAnimationAttack(attack);
            }
            //check si il peut cut l'attack
            if (_hasPressedInputAttack && CanCut(attack))
            {
                CutAnimationAttack();
            }
            
            yield return null;
        }
        Debug.Log("finAttack");
        _isInCombo = false;
        
        //si il a essayer d'attaquer pendant l'attaque precedente, il lance le prochain.
        if (_hasPressedInputAttack && _currentComboIndex % playerComboAttacks.Count != 0 )
        {
            StartCoroutine(CurrentAttackUpdate(playerComboAttacks[_currentComboIndex % playerComboAttacks.Count], true));
        }
        else
        {
           animator.CrossFade("Idle", 0.25f);
           //StartCoroutine(ResetComboIndex(0.5f));
           _currentComboIndex = 0;
           _waitTimeBetweencombo = 0.25f;
        }

        Debug.Log(animator.speed);
        //resetAnimatorSpeed
        animator.speed = 1;
    }

    IEnumerator ResetComboIndex(float time)
    {
        yield return new WaitForSeconds(time);
        _currentComboIndex = 0;
    }
    private bool IsAttackFinished(AttackScriptableObject attack)
    {
        return _timerCurrentAttack > animator.GetCurrentAnimatorStateInfo(0).length;
    }
    private bool CanAccelerate(AttackScriptableObject attack)
    {
        return _timerCurrentAttack > attack.TimeToAccelerate;
    }
    private bool CanCut(AttackScriptableObject attack)
    {
        return _timerCurrentAttack > attack.TimeToAllowCutAnim;
    }

    private void AccelerateAnimationAttack(AttackScriptableObject attack)
    {
        //accelerateAnimator
        animator.speed = attack.Acceleration;
    }
    private void CutAnimationAttack()
    {
        _timerCurrentAttack = 100;
    }
    
    #region Animation Event Methods
    private void AttackColliderOnOnCollideWithIDamageable(object sender, EventArgs eventArgs)
    {
        if (eventArgs is AttackDamageableEventArgs mDamageableEventArgs && canGiveDamage)
        {
            DamageableWasAttackedAtThisFrame = true;
            mDamageableEventArgs.idamageable.DoDamage(playerComboAttacks[_currentComboIndex].Damage, _attackCollider.gameObject.transform.position);
            
        }
    }
    
    public void SetDamageActive(int value)
    {
        canGiveDamage = value == 1;
        _attackCollider.enabled = canGiveDamage;

        //_trailSwordDistortion.emitting = canGiveDamage;
    }

    #endregion
    
    public bool canGiveDamage { get; set; }
}
