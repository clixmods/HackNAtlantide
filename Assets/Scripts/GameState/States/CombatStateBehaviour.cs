using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatStateBehaviour : MonoBehaviour, IGameStateCallBack
{
    private CombatState state;
    [SerializeField] GameStateManager _gameStateManager;
    private void OnEnable()
    {
        state = new CombatState();

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
