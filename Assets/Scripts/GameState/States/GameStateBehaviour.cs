using UnityEngine;
using UnityEngine.Events;
public abstract class GameStateBehaviour<T> : MonoBehaviour, IGameStateCallBack where T : GameState, new()
{
    public UnityEvent ApplyGameStateOverride;
    [SerializeField] protected GameStateManager _gameStateManager;
    protected T state;

    private void OnEnable()
    {
        state = new T();
        OnPreRegisterApplyState();
        _gameStateManager.RegisterCallback(this);
        _gameStateManager.ApplyState(state);
        OnPostRegisterApplyState();
    }
    private void OnDisable()
    {
        OnPreUnRegisterRemoveState();
        _gameStateManager.RemoveState(state);
        _gameStateManager.UnRegisterCallback(this);
        OnPostUnRegisterRemoveState();
    }

    protected virtual void OnPreRegisterApplyState(){}
    protected virtual void OnPostRegisterApplyState(){}
    protected virtual void OnPreUnRegisterRemoveState(){}
    protected virtual void OnPostUnRegisterRemoveState(){}
    public void OnApplyGameStateOverride(GameStateOverride stateOverride)
    {
        OnApplyGameStateOverrideImplement(stateOverride);
    }

    public void OnApplyCallback()
    {
        ApplyGameStateOverride?.Invoke();
    }

    protected virtual void OnApplyGameStateOverrideImplement(GameStateOverride stateOverride){}

}
