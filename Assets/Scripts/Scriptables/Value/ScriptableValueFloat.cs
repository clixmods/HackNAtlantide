using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Value/float")]
public class ScriptableValueFloat : ScriptableValue<float>
{
    [SerializeField] protected float defaultValue = 0;
    [SerializeField] protected float defaultMaxValue = 0;
    
    
    
    
    
    [SerializeField] private float _maxValue;
    public override float Value { get => base.Value; set => base.Value = value > _maxValue ? _maxValue : value; }
    public float DefaultValue => defaultValue;
    public float DefaultMaxValue => defaultMaxValue;
    public float MaxValue
    {
        get => _maxValue;
        set => _maxValue = value;
    } 
}
