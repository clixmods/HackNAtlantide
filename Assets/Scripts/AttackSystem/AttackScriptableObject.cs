using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "attackData")]
public class AttackScriptableObject : ScriptableObject
{
    [SerializeField] private int index;
    public float Index => index;

    [SerializeField] private float damage;
    public float Damage => damage;

    [SerializeField] private string animName;
    public string AnimName => animName;

    [SerializeField] private bool canAccelerate;
    public bool CanAccelerate => canAccelerate;

    [SerializeField] private float timeToAccelerate;
    public float TimeToAccelerate => timeToAccelerate;
    
    [SerializeField] private float acceleration;
    public float Acceleration => acceleration;
}
