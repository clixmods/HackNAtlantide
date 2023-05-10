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
    private static readonly int Blend = Animator.StringToHash("Blend");

    private void Awake()
    {
        PlayerMovement = GetComponentInParent<PlayerMovement>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        _animator.SetFloat(Blend, Mathf.Clamp(PlayerMovement.MoveAmount.magnitude / 0.06f, 0, 1));
        Debug.Log(PlayerMovement.MoveAmount.magnitude/ 0.06f);
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