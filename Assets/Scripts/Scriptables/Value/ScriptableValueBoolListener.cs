using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScriptableValueBoolListener : ScriptableValueListener<bool>
{
    public UnityEvent ValueIsTrue;
    public UnityEvent ValueIsFalse;
    protected override void Awake()
    {
        LaunchEvent.AddListener(ValueChanged);
        base.Awake();
    }

    private void ValueChanged(bool obj)
    {
        if (obj)
        {
            ValueIsTrue?.Invoke();
        }
        else
        {
            ValueIsFalse?.Invoke();
        }
    }
}
