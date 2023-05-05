using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Rumbler", menuName = "RumblerSO/rumblePulse")]
public class RumblerDataPulse : ScriptableObject
{
    public float duration;
    public float burstTime;
    public float low;
    public float high;
}
