using UnityEngine;
using UnityEngine.Events;

public class RunTimeSTateBehaviour : MonoBehaviour,IGameStateCallBack
{
    public UnityEvent Enable;
    public UnityEvent Disable;
    private RuntimeGameState state;
    [SerializeField] GameStateManager _gameStateManager;
    private void OnEnable()
    {
        state = new RuntimeGameState();

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
