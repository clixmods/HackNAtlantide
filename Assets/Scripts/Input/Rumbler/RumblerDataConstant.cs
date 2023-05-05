using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "Rumbler", menuName = "RumblerSO/rumbleConstant" )]
public class RumblerDataConstant : ScriptableObject
{
    public float duration;
    public float low;
    public float high;
}
