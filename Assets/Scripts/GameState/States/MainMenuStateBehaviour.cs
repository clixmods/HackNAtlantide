using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuStateBehaviour : MonoBehaviour, IGameStateCallBack
{
    private MainMenuState state;
    [SerializeField] GameStateManager _gameStateManager;
    private void OnEnable()
    {
        state = new MainMenuState();

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
        Debug.Log("Update => " + stateOverride.isPaused);
    }
}
