using System;
using UnityEngine;

public interface ITargetable
{
    public Transform transform { get; }
    public bool CanBeTarget { get; }
    public void OnTarget();
    public void OnUntarget();
    
    event EventHandler OnTargeted;
    event EventHandler OnUntargeted;
}
