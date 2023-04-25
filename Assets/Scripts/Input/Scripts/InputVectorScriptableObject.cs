using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/InputData/Vector")]
public class InputVectorScriptableObject : ScriptableObject
{
    public event Action<Vector2> OnValueChanged;

    public void ChangeValue(Vector2 value)
    {
        OnValueChanged?.Invoke(value);
    }
}
