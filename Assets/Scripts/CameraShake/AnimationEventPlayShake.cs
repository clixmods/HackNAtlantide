using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventPlayShake : MonoBehaviour
{
    public void PlayShake(CameraShakeScriptableObject shake)
    {
        shake.Shake();
    }
    public void PlayShakeDistance(CameraShakeScriptableObject shake)
    {
        shake.ShakeByDistance(Vector3.Distance(PlayerInstanceScriptableObject.Player.transform.position,transform.position));
    }
}
