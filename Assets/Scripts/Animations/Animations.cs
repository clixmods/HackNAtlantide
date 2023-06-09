using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animations : MonoBehaviour
{
    [SerializeField] private Animator altarAnimator;
    [SerializeField] private GameObject bridgeIntro;
    [SerializeField] private GameObject invisbleWall;
    private static readonly int Activation = Animator.StringToHash("Activation");

    public void AltarAnimationActivation()
    {
        altarAnimator.SetBool(Activation, true);

    }
    public void AltarAnimationDesctivation()
    {
        altarAnimator.SetBool(Activation, false);
    }
    public void AltarActivated()
    {
        bridgeIntro.SetActive(true);
        invisbleWall.SetActive(false);

    }
}