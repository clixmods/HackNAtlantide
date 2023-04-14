using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public PlayerControls _input { private set; get; }
    [SerializeField] private InputButtonScriptableObject _interact;
    [SerializeField] private InputButtonScriptableObject _dash;
    [SerializeField] private InputVectorScriptableObject _move;
    [SerializeField] private InputActionIcon _actionIcon;
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

        //Interact
        _input.InGame.Interact.performed += ctx => _interact.ChangeValue(true);
        _input.InGame.Interact.canceled += ctx => _interact.ChangeValue(false);

        //Dash
        _input.InGame.Dash.performed += ctx => _dash.ChangeValue(true);

        //Move
        _input.InGame.Move.performed += ctx => _move.ChangeValue(_input.InGame.Move.ReadValue<Vector2>());
        _input.InGame.Move.canceled += ctx => _move.ChangeValue(Vector2.zero);
    }

    void DisableGameInput()
    {
        //Interact
        _input.InGame.Interact.performed -= ctx => _interact.ChangeValue(true);
        _input.InGame.Interact.canceled -= ctx => _interact.ChangeValue(false);

        //Dash
        _input.InGame.Dash.performed -= ctx => _dash.ChangeValue(true);

        //Move
        _input.InGame.Move.performed -= ctx => _move.ChangeValue(_input.InGame.Move.ReadValue<Vector2>());
        _input.InGame.Move.canceled -= ctx => _move.ChangeValue(Vector2.zero);

        _input.Disable();
    }
    private void Update()
    {
        //find the last Input Device used and set a bool.
        //LastInputDeviceUsed();
    }

    bool LastInputDeviceUsed()
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

