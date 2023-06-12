using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class RuntimeGameState : GameState
{
    public override int Priority => GameStateUtility.RunTimePriority;
    public override IGameStateCallBack GameStateBehaviourInstance { get; set; }

    public override void ApplyOverride(GameStateOverride stateOverride)
    {
        stateOverride.timeScale = 1f;
        stateOverride.inputUIActive = false;
        stateOverride.canEnemyTargetPlayer = true;
        stateOverride.playerInvincible = false;
        stateOverride.showCursor = false;
    }
}
public class PauseGameState : GameState
{
    public override int Priority => GameStateUtility.PausePriority;
    public override IGameStateCallBack GameStateBehaviourInstance { get; set; }

    public override void ApplyOverride(GameStateOverride stateOverride)
    {
        stateOverride.isPaused = true;
        stateOverride.timeScale = 0f;

        stateOverride.inputPauseActive = false;
        stateOverride.inputInteractActive = false;
        stateOverride.inputMovementActive = false;
        stateOverride.inputCombatActive = false;
        stateOverride.inputDashActive = false;

        stateOverride.inputUIActive = true;
        stateOverride.showCursor = true;
    }
}
public class CinematiqueState : GameState
{
    public override int Priority => GameStateUtility.CinematiquePriority;
    public override IGameStateCallBack GameStateBehaviourInstance { get; set; }

    public override void ApplyOverride(GameStateOverride stateOverride)
    {
        stateOverride.isPaused = false;
        stateOverride.timeScale = 1f;
        stateOverride.inputPauseActive = false;
        stateOverride.inputInteractActive = false;
        stateOverride.inputMovementActive = false;
        stateOverride.inputCombatActive = false;
        stateOverride.inputDashActive = true;
        stateOverride.canEnemyTargetPlayer = false;
        stateOverride.playerInvincible = true;
    }
}
public class LoadingState : GameState
{
    public override int Priority => GameStateUtility.LoadingPriority;
    public override IGameStateCallBack GameStateBehaviourInstance { get; set; }

    public override void ApplyOverride(GameStateOverride stateOverride)
    {
        stateOverride.isPaused = false;
        stateOverride.timeScale = 1f;
        stateOverride.inputPauseActive = false;
        stateOverride.inputInteractActive = false;
        stateOverride.inputMovementActive = false;
        stateOverride.inputCombatActive = false;
        stateOverride.inputDashActive = false;
        stateOverride.canEnemyTargetPlayer = false;
        stateOverride.showCursor = true;
    }
}
public class MainMenuState : GameState
{
    public override int Priority => GameStateUtility.MainMenuPriority;
    public override IGameStateCallBack GameStateBehaviourInstance { get; set; }

    public override void ApplyOverride(GameStateOverride stateOverride)
    {
        stateOverride.isPaused = false;
        stateOverride.timeScale = 1f;
        stateOverride.inputPauseActive = false;
        stateOverride.inputUIActive = true;
        stateOverride.showCursor = true;
    }
}
public class CombatState : GameState
{
    public override int Priority => GameStateUtility.combatPriority;
    public override IGameStateCallBack GameStateBehaviourInstance { get; set; }

    public override void ApplyOverride(GameStateOverride stateOverride)
    {
        stateOverride.canEnemyTargetPlayer = true;
    }
}
public class TutoState : GameState
{
    public override int Priority => GameStateUtility.combatPriority;
    public override IGameStateCallBack GameStateBehaviourInstance { get; set; }


    public override void ApplyOverride(GameStateOverride stateOverride)
    {
        stateOverride.isPaused = false;
        stateOverride.timeScale = 1f;
        stateOverride.inputInteractActive = true;
        stateOverride.inputMovementActive = true;
        stateOverride.inputCombatActive = false;
        stateOverride.inputDashActive = false;

        stateOverride.canEnemyTargetPlayer = true;
    }
}
public class DeadState : GameState
{
    public override int Priority => GameStateUtility.DeadPriority;
    public override IGameStateCallBack GameStateBehaviourInstance { get; set; }

    public override void ApplyOverride(GameStateOverride stateOverride)
    {
        stateOverride.isPaused = false;
        stateOverride.timeScale = 0.3f;
        stateOverride.inputInteractActive = false;
        stateOverride.inputMovementActive = false;
        stateOverride.inputCombatActive = false;
        stateOverride.inputDashActive = false;
        stateOverride.inputPauseActive = false;
        stateOverride.canEnemyTargetPlayer = false;

    }
}
public interface IGameStateCallBack
{
    void OnApplyGameStateOverride(GameStateOverride stateOverride);
    void OnApplyCallback();
}

[Serializable]
public class GameStateOverride
{
    public bool isPaused;
    public float timeScale = 1f;
    public bool inputMovementActive = true;
    public bool inputCombatActive = true;
    public bool inputDashActive = true;
    public bool inputInteractActive = true;
    public bool inputPauseActive = true;
    public bool inputUIActive = false;
    public bool canEnemyTargetPlayer = true;
    public bool playerInvincible = false;
    public bool showCursor = true;
    
    public void Reset()
    {
        isPaused = false;
        timeScale = 1f;
        inputMovementActive = true;
        inputCombatActive = true;
        inputDashActive = true;
        inputInteractActive = true;
        inputPauseActive = true;
    }
    public void Apply()
    {
        Time.timeScale = timeScale;
        if(InputManager.Instance == null)
        {
            return;
        }
        InputManager.Instance.ActiveInputCombat(inputCombatActive);
        InputManager.Instance.ActiveInputDash(inputDashActive);
        InputManager.Instance.ActiveInputInteract(inputInteractActive);
        InputManager.Instance.ActiveInputMovement(inputMovementActive);
        InputManager.Instance.ActiveInputPause(inputPauseActive);
        InputManager.Instance.SwitchInputActionMap(!inputUIActive);
        GameStateManager.Instance._canEnemyAttackPlayer.LaunchEvent(canEnemyTargetPlayer);
        GameStateManager.Instance._invinciblePlayerEvent.LaunchEvent(playerInvincible);
        Cursor.visible = showCursor;
    }
    /*IEnumerator ApplyCoroutine()
    {
        yield return new WaitForEndOfFrame();
        Apply();
    }*/
}
[Serializable]
public abstract class GameState : IComparable<GameState> 
{
    #region Global
    /// <summary>
    /// Minimum is the heighest priority
    /// </summary>
    public abstract int Priority { get; }
    public abstract IGameStateCallBack GameStateBehaviourInstance { get; set; }
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
    public GameStateOverride GameStateOverride { get { return gameStateOverride; } }

    public static GameState ActiveGameState => Instance.currentGameStates[0];

    private bool isApplicationQuit = false;

    public GameObject tutoStateObject;
    public GameObject pauseStateObject;
    public GameObject runTimeStateObject;
    public GameObject combatStateObject;
    public GameObject deadStateObject;
    public GameObject cinematicStateObject;
    public GameObject mainMenuStateObject;
    [SerializeField] ScriptableEventBool pauseEvent;
    [SerializeField] ScriptableEvent restartEvent;

    private IGameStateCallBack _lastCallBackCalled;
    public ScriptableEventBool _canEnemyAttackPlayer;
    public ScriptableEventBool _invinciblePlayerEvent;
    //------------------------
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance.gameObject);
        }
        Instance = this;
        DontDestroyOnLoad(this);
    }
    private void Start()
    {
        RefreshState();
        //Add runtimestatelater cause of orderexecutio
        StartCoroutine(RunTimeState());
    }
    private void OnEnable()
    {
        pauseEvent.OnEvent += SetPauseActive;
        restartEvent.OnEvent += gameStateOverride.Reset;
    }
    private void OnDisable()
    {
        pauseEvent.OnEvent -= SetPauseActive;
        restartEvent.OnEvent -= gameStateOverride.Reset;
    }
    //------------------------
    void SetPauseActive(bool isActive)
    {
        pauseStateObject.SetActive(isActive);
    }

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

        // if (callbacks.Count > 0   )
        // {
        //     //_lastCallBackCalled = callbacks[^1];
        //     callbacks[^1].OnApplyCallback();
        // }
        // 0 is first
        if (currentGameStates.Count > 0 && _lastCallBackCalled != currentGameStates[0].GameStateBehaviourInstance)
        {
            _lastCallBackCalled = currentGameStates[0].GameStateBehaviourInstance;
            currentGameStates[0].GameStateBehaviourInstance.OnApplyCallback();
        }
    }
    IEnumerator RunTimeState()
    {
        yield return new WaitForSeconds(0.5f);
        runTimeStateObject.SetActive(true);
    }
}
