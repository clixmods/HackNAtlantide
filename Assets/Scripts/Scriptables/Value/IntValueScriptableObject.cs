using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Value/int")]
public class IntValueScriptableObject : ValueScriptableObject<int>
{
    [SerializeField] private int _maxValue = Int32.MaxValue;
    public override int Value { get => base.Value; set => base.Value = value > _maxValue ? _maxValue : value; }
}
