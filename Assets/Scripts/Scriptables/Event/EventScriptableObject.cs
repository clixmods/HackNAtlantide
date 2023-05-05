using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Event/None")]
public class EventScriptableObject : ScriptableObject
{ 
    public event Action OnEvent;
    public void LaunchEvent()
    {
        OnEvent?.Invoke();
    }
}
