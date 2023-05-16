using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadStateBehaviour : MonoBehaviour, IGameStateCallBack
{
    private DeadState state;
    [SerializeField] GameStateManager _gameStateManager;
    private void OnEnable()
    {
        state = new DeadState();

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

    }
}
