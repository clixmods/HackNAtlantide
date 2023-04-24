using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTypeScriptableObject<T> : ScriptableObject
{
    public event Action<T> OnEvent;
    public void LaunchEvent(T value)
    {
        OnEvent?.Invoke(value);
    }
}
