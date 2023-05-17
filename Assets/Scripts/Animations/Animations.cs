using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Animations : MonoBehaviour
{
    [SerializeField] private Animator altarAnimator;
    private static readonly int Activation = Animator.StringToHash("Activation");

    public void AltarAnimationActivation()
    {
        altarAnimator.SetBool(Activation, true);
    }
}