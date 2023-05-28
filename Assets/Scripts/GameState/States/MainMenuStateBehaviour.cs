using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuStateBehaviour :  GameStateBehaviour<MainMenuState>
{
    protected override void OnApplyGameStateOverrideImplement(GameStateOverride stateOverride)
    {
        Debug.Log("Update => " + stateOverride.isPaused);
    }
}
