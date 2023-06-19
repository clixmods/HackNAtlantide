using System;
using System.Collections.Generic;
using UnityEngine;

public interface IFocusable
{
    public static List<IFocusable> Focusables = new List<IFocusable>();
    public Transform transform { get; }
    public Transform focusableTransform { get; }
    public bool CanBeFocusable { get; }
    public void OnNearest();
    public void OnNoNearest();
    public void OnFocus();
    public void OnUnfocus();
    event EventHandler OnTargeted;
    event EventHandler OnUntargeted;
}
