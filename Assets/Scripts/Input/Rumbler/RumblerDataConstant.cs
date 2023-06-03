using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "Rumbler", menuName = "RumblerSO/rumbleConstant" )]
public class RumblerDataConstant : ScriptableObject
{
    public float duration = 0.25f;
    public float low = 0.5f;
    public float high = 1f;

    public void Rumble()
    {
        Rumbler.instance.RumbleConstant(duration, low, high);
    }
}
