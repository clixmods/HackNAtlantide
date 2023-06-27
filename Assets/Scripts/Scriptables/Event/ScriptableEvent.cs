using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Event/None")]
public class ScriptableEvent : ScriptableObject
{ 
    public event Action OnEvent;
    [ContextMenu("launchevent")]
    public void LaunchEvent()
    {
        OnEvent?.Invoke();
    }
}
