using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TutoStateBehaviour : MonoBehaviour, IGameStateCallBack
{
    public UnityEvent ApplyGameStateOverride;
    private TutoState state;
    [SerializeField] GameStateManager _gameStateManager;
    private void OnEnable()
    {
        state = new TutoState();

        _gameStateManager.RegisterCallback(this);
        _gameStateManager.ApplyState(state);
        
    }
    private void OnDisable()
    {
        _gameStateManager.RemoveState(state);
        _gameStateManager.UnRegisterCallback(this);
    }

    public void OnApplyGameStateOverride(GameStateOverride stateOverride)
    {
        ApplyGameStateOverride?.Invoke();
    }
}
