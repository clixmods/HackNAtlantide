using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseStateBehaviour : MonoBehaviour, IGameStateCallBack
{
    private PauseGameState state;
    private float timeScaleBeforePause;
    [SerializeField] List<InputScriptableObject<bool>> inputsData;
    [SerializeField] List<bool> activeInputs;
    [SerializeField] GameStateManager _gameStateManager;
    private void OnEnable()
    {
        state = new PauseGameState();
        RegisterInputs();
        timeScaleBeforePause = Time.timeScale;
        _gameStateManager.RegisterCallback(this);
        _gameStateManager.ApplyState(state);
    }
    private void OnDisable()
    {
        _gameStateManager.RemoveState(state);
        _gameStateManager.UnRegisterCallback(this);
        Time.timeScale = timeScaleBeforePause;
        ApplyInputs();
    }

    public void OnApplyGameStateOverride(GameStateOverride stateOverride)
    {
        Debug.Log("Update => " + stateOverride.isPaused);
    }

    public void RegisterInputs()
    {
        activeInputs.Clear();
        for(int i = 0; i < inputsData.Count; i++)
        {
            activeInputs.Add( inputsData[i].IsActive);
        }
    }
    public void ApplyInputs()
    {
        for (int i = 0; i < inputsData.Count; i++)
        {
            inputsData[i].IsActive = activeInputs[i];
        }
    }
}
