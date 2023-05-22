using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Value/float")]
public class ScriptableValueFloat : ScriptableValue<float>
{
    [SerializeField] private float _maxValue;
    public override float Value { get => base.Value; set => base.Value = value > _maxValue ? _maxValue : value; }

    public float MaxValue
    {
        get => _maxValue;
        set => _maxValue = value;
    } 
}
