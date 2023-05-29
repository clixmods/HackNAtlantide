using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CameraShakeManager : MonoBehaviour
{
    public static CameraShakeManager instance;
    [SerializeField] CinemachineImpulseSource impulseSourceExplosionLong;
    [SerializeField] CinemachineImpulseSource impulseSourceExplosionShort;
    [SerializeField] CinemachineImpulseSource impulseSourceBump;
    [SerializeField] CinemachineImpulseSource impulseSourceRumble;
    [SerializeField] CinemachineImpulseSource impulseSourceRecoil;
    float currentpriority = float.MaxValue;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
        }
        instance = this;
        DontDestroyOnLoad(this);
    }
    public void Shake(ShakeType shakeType, float duration, float magnitude, bool additive, float priority)
    {
        if(currentpriority < priority)
        {
            return;
        }
        currentpriority = priority;
        StopCoroutine(ResetPriority(duration));
        StartCoroutine(ResetPriority(duration));
        if(!additive)
        {
            StopAllShakes();
        }
        switch (shakeType)
        {
            case ShakeType.ExplosionLong:
                ShakeBySource(impulseSourceExplosionLong, duration, magnitude);
                break;
            case ShakeType.ExplosionShort:
                ShakeBySource(impulseSourceExplosionShort, duration, magnitude);
                break;
            case ShakeType.Recoil:
                ShakeBySource(impulseSourceRecoil, duration, magnitude);
                break;
            case ShakeType.Bump:
                ShakeBySource(impulseSourceBump, duration, magnitude);
                break;
            case ShakeType.Rumble:
                ShakeBySource(impulseSourceRumble, duration, magnitude);
                break;
        }
    }
    IEnumerator ResetPriority(float duration)
    {
        yield return new WaitForSeconds(duration);
        currentpriority = float.MaxValue;
    }

    void ShakeBySource(CinemachineImpulseSource cinemachineImpulseSource, float duration, float magnitude)
    {
        cinemachineImpulseSource.m_ImpulseDefinition.m_ImpulseDuration = duration;
        cinemachineImpulseSource.GenerateImpulse(magnitude);
    }

    public void StopAllShakes()
    {

    }
}
