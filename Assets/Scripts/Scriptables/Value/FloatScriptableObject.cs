using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Value/float")]
public class FloatScriptableObject : ValueScriptableObject<float>
{
    [SerializeField] private float _maxValue;
    public override float Value { get => base.Value; set => base.Value = value > _maxValue ? _maxValue : value; }
}
