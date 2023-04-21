using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="InputData/button")]
public class InputButtonScriptableObject : ScriptableObject
{
    public event Action<bool> OnValueChanged;
    public void ChangeValue(bool value)
    {
        OnValueChanged?.Invoke(value);
    }

}
