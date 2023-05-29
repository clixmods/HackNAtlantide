using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ShakeType
{
    ExplosionLong,
    ExplosionShort,
    Recoil,
    Bump,
    Rumble,
}

[CreateAssetMenu(fileName = "CameraShake/ShakeData")]
public class CameraShakeScriptableObject : ScriptableObject
{
    [SerializeField] float duration;
    public float Duration { get { return duration; } }

    [SerializeField] float magnitude;
    public float Magnitude { get { return magnitude; } }

    [SerializeField] float priority =100;
    public float Priority { get { return priority; } }

    [SerializeField] bool additive = true;
    public bool Additive { get { return additive; } }

    [SerializeField] ShakeType shakeType;
    public ShakeType ShakeType { get { return shakeType; }}

    public void Shake()
    {
        CameraShakeManager.instance.Shake(shakeType, duration, magnitude, additive, priority);
    }
    public void ShakeByDistance(float distance)
    {
        CameraShakeManager.instance.Shake(shakeType, duration, magnitude/(1+distance), additive, priority);
    }
}
