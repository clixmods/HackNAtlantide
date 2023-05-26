using UnityEngine;
using UnityEngine.Events;

public class RunTimeSTateBehaviour : MonoBehaviour,IGameStateCallBack
{
    public UnityEvent ApplyGameStateOverride;
    private RuntimeGameState state;
    [SerializeField] GameStateManager _gameStateManager;
    private void OnEnable()
    {
        state = new RuntimeGameState();

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
