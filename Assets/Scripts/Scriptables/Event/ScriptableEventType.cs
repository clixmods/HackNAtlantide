using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptableEventType<T> : ScriptableObject
{
    public event Action<T> OnEvent;
    public void LaunchEvent(T value)
    {
        OnEvent?.Invoke(value);
    }
}
