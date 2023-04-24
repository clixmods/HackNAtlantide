using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Value/int")]
public class IntScriptableObject : ValueScriptableObject<int>
{
    [SerializeField] private int _maxValue;
    public override int Value { get => base.Value; set => base.Value = value > _maxValue ? _maxValue : value; }
}
