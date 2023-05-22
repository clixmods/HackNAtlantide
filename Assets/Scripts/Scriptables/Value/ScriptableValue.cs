using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptableValue<T> : ScriptableObject
{
    [SerializeField] private T _value;
    public virtual T Value { get { return _value; } set { this._value = value; OnValueChanged?.Invoke(this._value); } }

    public Action<T> OnValueChanged;

    private void OnValidate()
    {
        OnValueChanged?.Invoke(Value);
    }

 
}
