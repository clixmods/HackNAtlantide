using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimations : MonoBehaviour
{
    [Header("SCRIPTS REFS")] private PlayerMovement PlayerMovement;
    private Animator _animator;

    private void Awake()
    {
        PlayerMovement = GetComponentInParent<PlayerMovement>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        _animator.SetFloat("Blend", PlayerMovement.MoveAmount.magnitude);
    }

    public void Dash()
    {
        StartCoroutine(DashCoroutine());
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