using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;


public class RuntimeGameState : GameState
{
    public override int Priority => GameStateUtility.RunTimePriority;
    public override void ApplyOverride(GameStateOverride stateOverride)
    {

    }
}
public class PauseGameState : GameState
{
    public override int Priority => GameStateUtility.PausePriority;

    public override void ApplyOverride(GameStateOverride stateOverride)
    {
        stateOverride.isPaused = true;
        stateOverride.timeScale = 0f;
    }
}
public class CinematiqueState : GameState
{
    public override int Priority => GameStateUtility.CinematiquePriority;

    public override void ApplyOverride(GameStateOverride stateOverride)
    {
        stateOverride.isPaused = false;
        stateOverride.timeScale = 1f;
    }
}
public class MainMenuState : GameState
{
    public override int Priority => GameStateUtility.MainMenuPriority;

    public override void ApplyOverride(GameStateOverride stateOverride)
    {
        stateOverride.isPaused = false;
        stateOverride.timeScale = 0f;
    }
}
public class CombatState : GameState
{
    public override int Priority => GameStateUtility.combatPriority;

    public override void ApplyOverride(GameStateOverride stateOverride)
    {
        stateOverride.isPaused = false;
        stateOverride.timeScale = 0f;
    }
}
public interface IGameStateCallBack
{
    void OnApplyGameStateOverride(GameStateOverride stateOverride);
}

[Serializable]
public class GameStateOverride
{
    public bool isPaused;
    public float timeScale = 1f;
    public void Reset()
    {
        isPaused = false;
        timeScale = 1f;
    }
    public void Apply()
    {
        Time.timeScale = timeScale;
    }
}
[Serializable]
public abstract class GameState : IComparable<GameState>
{
    #region Global
    /// <summary>
    /// Minimum is the heighest priority
    /// </summary>
    public abstract int Priority { get; }
    public GameState()
    {

    }
    #endregion

    #region Operator

    public static bool operator >(GameState a, GameState b)
    {
        return a.Priority > b.Priority;
    }
    public static bool operator <(GameState a, GameState b)
    {
        return !(a < b);
    }
    public static bool operator >=(GameState a, GameState b)
    {
        return a.Priority >= b.Priority;
    }
    public static bool operator <=(GameState a, GameState b)
    {
        return !(a <= b);
    }

    #endregion

    #region Comparer
    public int CompareTo(GameState other)
    {
        return Priority.CompareTo(other.Priority);
    }
    #endregion

    #region Override
    public abstract void ApplyOverride(GameStateOverride stateOverride);
    public override string ToString()
    {
        return GetType().Name;
    }

    #endregion
}

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;

    private List<IGameStateCallBack> callbacks = new List<IGameStateCallBack>();

    [SerializeReference] private List<GameState> currentGameStates = new List<GameState>();

    [SerializeField] private GameStateOverride gameStateOverride = new GameStateOverride();

    private bool isApplicationQuit = false;

    //------------------------
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance);
        }
        Instance = this;
        DontDestroyOnLoad(this);
    }

    //------------------------

    public void RegisterCallback(IGameStateCallBack callback)
    {
        callbacks.Add(callback);
    }

    public void UnRegisterCallback(IGameStateCallBack callback)
    {
        callbacks.Remove(callback);
    }

    //------------------------
    public void ApplyState(GameState state)
    {
        currentGameStates.Add(state);
        currentGameStates.Sort();
        RefreshState();
    }

    public bool RemoveState(GameState state)
    {
        bool value = currentGameStates.Remove(state);
        RefreshState();
        return value;

    }

    public void RemoveAllState(Type stateType)
    {
        for(int i = currentGameStates.Count; i-- > 0;)
        {
            if(currentGameStates[i].GetType() == stateType)
            {
                currentGameStates.RemoveAt(i);
            }
        }
        RefreshState();
    }

    public bool GetCurrentGameState(out GameState state)
    {
        if(currentGameStates.Count == 0)
        {
            state = null;
            return false;
        }
        state = currentGameStates[0];
        return true;
    }

    private void RefreshState()
    {
        //check if not quitting
        if(isApplicationQuit)
        {
            return;
        }

        //Reset.
        gameStateOverride.Reset();

        //State Apply
        for (int i = currentGameStates.Count; i-- > 0;)
        {
            currentGameStates[i].ApplyOverride(gameStateOverride);
        }

        //Final Apply.
        gameStateOverride.Apply();

        //CallBack.
        for (int i = callbacks.Count; i-- > 0;)
        {
            callbacks[i].OnApplyGameStateOverride(gameStateOverride);
        }

        if(GetCurrentGameState(out GameState state))
        {
            Debug.Log($"CurrentState => {state.ToString()}");
        }
        else
        {
            Debug.LogError("No active state");
        }
    }
}
