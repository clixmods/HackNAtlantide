using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimations : MonoBehaviour
{
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
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