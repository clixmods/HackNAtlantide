using UnityEngine.Events;

public class LoadingStateBehaviour : GameStateBehaviour<LoadingState>
{
    public UnityEvent OnDisableState;

    protected override void OnPostUnRegisterRemoveState()
    {
        base.OnPostUnRegisterRemoveState();
        OnDisableState?.Invoke();
    }
}
