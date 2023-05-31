using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimations : MonoBehaviour
{
    [Header("SCRIPTS REFS")]
    private PlayerMovement _playerMovement;
    private Animator _animator;

    [field: Header("VARIABLES")]
    public float TimeBeforeIdle
    {
        get => _timeBeforeIdle;
        set => _timeBeforeIdle = value;
    }

    private float _timeBeforeIdle = 15f;

    private void Awake()
    {
        _playerMovement = GetComponentInParent<PlayerMovement>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        float Blend;
        Blend = Mathf.Clamp(_playerMovement.MoveAmount.magnitude / 0.06f, 0f, 1f);
        if (Blend < 0.1f)
        {
            Debug.Log("Set Blend to zero");
            Blend *= 2f;
        }
        if (Blend < 0.01f)
        {
            Debug.Log("Set Blend to zero");
            Blend = 0f;
        }
        _animator.SetFloat("Blend", Blend);
        if (_playerMovement.MoveAmount.magnitude / 0.06f <= 0.1f)
        {
            _animator.ResetTrigger("Idle");
            _timeBeforeIdle -= Time.deltaTime;
        }
        else if (_playerMovement.MoveAmount.magnitude / 0.06f >= 0.1f)
        {
            _animator.SetTrigger("Idle");
            _timeBeforeIdle = 15f;
        }

        if (_timeBeforeIdle <= 0f)
        {
            StartCoroutine(IdleCoroutine());
        }
    }

    public void SetTriggerIdle()
    {
        _animator.CrossFade("Idle",0.01f);
    }

    public void ResetIdle2()
    {
        _timeBeforeIdle = 15f;
    }

    public void Dash()
    {
        StartCoroutine(DashCoroutine());
    }
    
    IEnumerator IdleCoroutine()
    {
        _animator.CrossFade("Idle_Chara_Sword_2",0.01f);
        _timeBeforeIdle = 15f;
        yield break;
    }

    IEnumerator DashCoroutine()
    {
        _animator.CrossFade("Roulade_Chara_Sword", 0f);
        _animator.SetBool("dash", true);

        yield return new WaitForSeconds(.5f);
        _animator.SetBool("dash", false);
    }
    public void DashFinish()
    {
        Debug.Log("dashfinish");
        GetComponentInParent<PlayerMovement>().SetTransformLock();
    }
}