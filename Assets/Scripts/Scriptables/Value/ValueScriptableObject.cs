using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValueScriptableObject<T> : ScriptableObject
{
    [SerializeField] private T _value;
    public virtual T Value { get { return _value; } set { this._value = value; OnValueChanged.Invoke(value); } }

    public Action<T> OnValueChanged;
    
}
