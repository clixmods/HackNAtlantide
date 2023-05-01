using System;
using UnityEngine;


    public abstract class InputScriptableObject<T> : ScriptableObject
    {
        public event Action<T> OnValueChanged;
        public void ChangeValue(T value)
        {
            OnValueChanged?.Invoke(value);
        }
    }
