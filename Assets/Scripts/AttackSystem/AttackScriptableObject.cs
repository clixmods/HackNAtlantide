using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "attackData")]
public class AttackScriptableObject : ScriptableObject
{
    [SerializeField] private float damage;
    public float Damage => damage;
    [SerializeField] private string animName;
    public string AnimName => animName;
    [SerializeField] private float attackDuration;
    public float AttackDuration => attackDuration;
    [SerializeField] private bool canRegisterInput;
    public bool CanRegisterInput => canRegisterInput;
    [SerializeField] private float timeToAllowCutAnim;
    public float TimeToAllowCutAnim => timeToAllowCutAnim;
    [SerializeField] private float timeToAccelerate;
    public float TimeToAccelerate => timeToAccelerate;
    
    [SerializeField] private float acceleration;
    public float Acceleration => acceleration;
    [SerializeField] private float speed;
    public float Speed => speed;
}
