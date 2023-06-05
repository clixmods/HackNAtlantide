using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventPlayAttackFX : MonoBehaviour
{
    [SerializeField] private ParticleSystem FXAttack1;
    [SerializeField] private ParticleSystem FXAttack2;
    [SerializeField] private ParticleSystem FXAttack3;
    public void PlayFX(int value)
    {
        switch(value)
        {
            case 0 :
                //FXAttack1.transform.rotation = transform.rotation;
                FXAttack1.Play();
            break;
            case 1 :
                //FXAttack2.transform.rotation = transform.rotation;
                FXAttack2.Play();
            break;
            case 2 :
                //FXAttack3.transform.rotation = transform.rotation;
                FXAttack3.Play();
            break;
            default :
               // FXAttack1.transform.rotation = transform.rotation;
                FXAttack1.Play();
            break;
        }
    }
}
