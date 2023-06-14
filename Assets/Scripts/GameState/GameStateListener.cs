using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class GameStateListener<T> :  MonoBehaviour where T : GameState, new()
{
    public UnityEvent StateActive;
    public UnityEvent StateDesactive;
    private void Awake()
    {
        GameStateManager.OnStateChanged += StateChanged;
    }
    private void OnDestroy()
    {
        GameStateManager.OnStateChanged -= StateChanged;
    }

    protected void StateChanged(GameState newState)
    {
        if (newState is T)
        {
            StateActive?.Invoke();
        }
        else
        {
            StateDesactive?.Invoke();
        }
    }
}
