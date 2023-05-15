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

    private float _timeBeforeIdle = 5f;
    
    private static readonly int Blend = Animator.StringToHash("Blend");

    private void Awake()
    {
        _playerMovement = GetComponentInParent<PlayerMovement>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        _animator.SetFloat(Blend, Mathf.Clamp(_playerMovement.MoveAmount.magnitude / 0.06f, 0, 1));

        if (_playerMovement.MoveAmount.magnitude / 0.06f <= 0.1f)
        {
            _timeBeforeIdle -= Time.deltaTime;
        }
        else if (_playerMovement.MoveAmount.magnitude / 0.06f >= 0.1f)
        {
            _timeBeforeIdle = 5f;
        }

        if (_timeBeforeIdle <= 0f)
        {
            StartCoroutine(IdleCoroutine());
        }
    }

    public void Dash()
    {
        StartCoroutine(DashCoroutine());
    }
    
    IEnumerator IdleCoroutine()
    {
        _animator.SetTrigger("Idle");
        Debug.Log("triggered");
        _timeBeforeIdle = 5f;
        yield break;
    }

    IEnumerator DashCoroutine()
    {
        _animator.SetBool("dash", true);
        Debug.Log("triggered");

        yield return new WaitForSeconds(.5f);
        _animator.SetBool("dash", false);
        Debug.Log("triggered");
    }
}