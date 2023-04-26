using UnityEngine;

public interface ITargetable
{
    public Transform transform { get; }
    public bool CanBeTarget { get; }
    public void OnTarget();
    public void OnUntarget();
}
