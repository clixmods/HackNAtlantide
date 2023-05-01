using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public PlayerControls _input { private set; get; }
    [SerializeField] private InputButtonScriptableObject _interact;
    [SerializeField] private InputButtonScriptableObject _attack;
    [SerializeField] private InputButtonScriptableObject _dash;
    [SerializeField] private InputVectorScriptableObject _move;
    [SerializeField] private InputVectorScriptableObject _switchFocus;
    [SerializeField] private InputButtonScriptableObject _focus;
    [SerializeField] private InputButtonScriptableObject _pause;
    [SerializeField] private InputActionIcon _actionIcon;
    private bool _isGamepad { get; set; }
    void Awake()
    {
        _input = new PlayerControls();
    }
    private void OnEnable()
    {
        EnableGameInput();
    }
    private void OnDisable()
    {
        DisableGameInput();
    }
    public void EnableGameInput()
    {
        _input.Enable();

        //_input.InGame.Interact
        //Interact
        _input.InGame.Interact.performed += ctx => _interact.ChangeValue(true);
        _input.InGame.Interact.canceled += ctx => _interact.ChangeValue(false);

        //Dash
        _input.InGame.Dash.performed += ctx => _dash.ChangeValue(true);

        // Attack
        _input.InGame.Attack.performed += ctx => _attack.ChangeValue(true);

        //Move
        _input.InGame.Move.performed += ctx => _move.ChangeValue(_input.InGame.Move.ReadValue<Vector2>());
        _input.InGame.Move.canceled += ctx => _move.ChangeValue(Vector2.zero);
        // Focus
        _input.InGame.Focus.performed += ctx => _focus.ChangeValue(true);
        // SwitchFocus
        _input.InGame.SwitchFocus.performed += ctx => _switchFocus.ChangeValue(_input.InGame.SwitchFocus.ReadValue<Vector2>());
        _input.InGame.SwitchFocus.canceled += ctx => _switchFocus.ChangeValue(Vector2.zero);
        // Pause
        _input.InGame.Pause.performed += ctx =>  _pause.ChangeValue(true);
    }

    void DisableGameInput()
    {
        //Interact
        _input.InGame.Interact.performed -= ctx => _interact.ChangeValue(true);
        _input.InGame.Interact.canceled -= ctx => _interact.ChangeValue(false);

        //Dash
        _input.InGame.Dash.performed -= ctx => _dash.ChangeValue(true);

        // Attack
        _input.InGame.Attack.performed -= ctx => _attack.ChangeValue(true);

        //Move
        _input.InGame.Move.performed -= ctx => _move.ChangeValue(_input.InGame.Move.ReadValue<Vector2>());
        _input.InGame.Move.canceled -= ctx => _move.ChangeValue(Vector2.zero);
        
        // Focus
        _input.InGame.Focus.performed -= ctx => _focus.ChangeValue(true);
        // SwitchFocus
        _input.InGame.SwitchFocus.performed -= ctx => _switchFocus.ChangeValue(_input.InGame.SwitchFocus.ReadValue<Vector2>());
        _input.InGame.SwitchFocus.canceled -= ctx => _switchFocus.ChangeValue(Vector2.zero);
        // Pause
        _input.InGame.Pause.performed -= ctx =>  _pause.ChangeValue(true);
        
        _input.Disable();
    }
    private void Update()
    {
        //find the last Input Device used and set a bool.
        _isGamepad = IsGamepad();
     
    }

    public static bool IsGamepad()
    {
        InputDevice lastUsedDevice = null;
        float lastEventTime = 0;
        foreach (var device in InputSystem.devices)
        {
            if (device.lastUpdateTime > lastEventTime)
            {
                lastUsedDevice = device;
                lastEventTime = (float)device.lastUpdateTime;
            }
        }

        return lastUsedDevice is Gamepad;
    }
}

