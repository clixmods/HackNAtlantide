using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseStateBehaviour : GameStateBehaviour<PauseGameState>
{
    private PauseGameState state;
    private float timeScaleBeforePause;
    [SerializeField] List<InputScriptableObject<bool>> inputsData;
    [SerializeField] List<bool> activeInputs;
    protected override void OnPreRegisterApplyState()
    { 
        RegisterInputs();
        timeScaleBeforePause = Time.timeScale;
    }
    protected override void OnPostUnRegisterRemoveState()
    {
        Time.timeScale = timeScaleBeforePause;
        ApplyInputs();
    }

    protected override void OnApplyGameStateOverrideImplement(GameStateOverride stateOverride)
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
