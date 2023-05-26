using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CombatStateBehaviour : MonoBehaviour, IGameStateCallBack
{
    public UnityEvent Enable;
    public UnityEvent Disable;
    private CombatState state;
    [SerializeField] GameStateManager _gameStateManager;
    private void OnEnable()
    {
        state = new CombatState();

        _gameStateManager.RegisterCallback(this);
        _gameStateManager.ApplyState(state);
        Enable?.Invoke();
    }
    private void OnDisable()
    {
        _gameStateManager.RemoveState(state);
        _gameStateManager.UnRegisterCallback(this);
        Disable?.Invoke();
    }

    public void OnApplyGameStateOverride(GameStateOverride stateOverride)
    {

    }
}
