using System;
using System.Collections.Generic;
using UnityEngine;

public interface ITargetable
{
    public static List<ITargetable> Targetables = new List<ITargetable>();
    public Transform transform { get; }
    public Transform targetableTransform { get; }
    public bool CanBeTarget { get; }
    public void OnTarget();
    public void OnUntarget();
    
    event EventHandler OnTargeted;
    event EventHandler OnUntargeted;
}
