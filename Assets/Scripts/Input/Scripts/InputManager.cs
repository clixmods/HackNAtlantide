using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    public static PlayerControls _input { private set; get; }
    [SerializeField] private InputButtonScriptableObject _interact;
    [SerializeField] private InputButtonScriptableObject _attack;
    [SerializeField] private InputButtonScriptableObject _dash;
    [SerializeField] private InputButtonScriptableObject _dashAttack;
    [SerializeField] private InputVectorScriptableObject _move;
    [SerializeField] private InputVectorScriptableObject _switchFocus;
    [SerializeField] private InputButtonScriptableObject _focus;
    [SerializeField] private InputButtonScriptableObject _pause;
    [SerializeField] private InputButtonScriptableObject _boussole;
    [SerializeField] private InputButtonScriptableObject _release;
    [SerializeField] private InputActionIcon _actionIcon;
    private bool _isGamepad { get; set; }



    public static event Action rebindComplete;
    public static event Action rebindCanceled;
    public static event Action<InputAction, int> rebindStarted;

    [SerializeField] List<InputActionReference> allBinding;
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance.gameObject);
        }
        Instance = this;
        DontDestroyOnLoad(this);
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
        // Attack
        _input.InGame.Attack.performed += ctx => _attack.ChangeValue(true);

        //Dash
        _input.InGame.Dash.performed += ctx => _dash.ChangeValue(true);
        _input.InGame.Dash.canceled += ctx => _dash.ChangeValue(false);

        // DashAttack
        _input.InGame.DashAttack.performed += ctx => _dashAttack.ChangeValue(true);
        _input.InGame.DashAttack.canceled += ctx => _dashAttack.ChangeValue(false);

        //Move
        _input.InGame.Move.performed += ctx => _move.ChangeValue(_input.InGame.Move.ReadValue<Vector2>());
        _input.InGame.Move.canceled += ctx => _move.ChangeValue(Vector2.zero);
        // Focus
        _input.InGame.Focus.performed += ctx => _focus.ChangeValue(true);
        _input.InGame.Focus.canceled += ctx => _focus.ChangeValue(false);
        // SwitchFocus
        _input.InGame.SwitchFocus.performed += ctx => _switchFocus.ChangeValue(_input.InGame.SwitchFocus.ReadValue<Vector2>());
        _input.InGame.SwitchFocus.canceled += ctx => _switchFocus.ChangeValue(Vector2.zero);
        // Pause
        _input.InGame.Pause.performed += ctx =>  _pause.ChangeValue(true);
        //Boussole
        _input.InGame.Boussole.performed += ctx => _boussole.ChangeValue(true);
        _input.InGame.Boussole.canceled += ctx => _boussole.ChangeValue(false);
        // Release
        _input.InGame.Release.performed += ctx => _release.ChangeValue(true);
        _input.InGame.Release.canceled += ctx => _release.ChangeValue(false);
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
        
        // DashAttack
        _input.InGame.DashAttack.performed -= ctx => _dashAttack.ChangeValue(true);

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

        //Boussole
        _input.InGame.Boussole.performed -= ctx => _boussole.ChangeValue(true);
        _input.InGame.Boussole.canceled -= ctx => _boussole.ChangeValue(false);
        // Release
        _input.InGame.Release.performed -= ctx => _release.ChangeValue(true);
        _input.InGame.Release.canceled -= ctx => _release.ChangeValue(false);

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


    public static void StartRebind(string actionName, int bindingIndex, TextMeshProUGUI statusText, bool excludeMouse)
    {
        InputAction action = _input.asset.FindAction(actionName);
        if (action == null || action.bindings.Count <= bindingIndex)
        {
            Debug.Log("Couln't find action or binding");
            return;
        }

        if (action.bindings[bindingIndex].isComposite)
        {
            var firstPartIndex = bindingIndex + 1;
            if (firstPartIndex < action.bindings.Count && action.bindings[firstPartIndex].isComposite)
            {
                DoRebind(action, bindingIndex, statusText, true, excludeMouse);
            }
        }

        else
            DoRebind(action, bindingIndex, statusText, false, excludeMouse);
    }

    private static void DoRebind(InputAction actionToRebind, int bindingIndex, TextMeshProUGUI statusText, bool allCompositeParts, bool excludeMouse)
    {
        Debug.Log("Binding");
        if (actionToRebind == null || bindingIndex < 0)
            return;

        statusText.text = $"Press a {actionToRebind.expectedControlType}";
        actionToRebind.Disable();

        var rebind = actionToRebind.PerformInteractiveRebinding(bindingIndex);
        rebind.OnComplete(operation =>
        {
            actionToRebind.Enable();
            operation.Dispose();

            if (allCompositeParts)
            {
                var nextBindingIndex = bindingIndex + 1;
                if (nextBindingIndex < actionToRebind.bindings.Count && actionToRebind.bindings[nextBindingIndex].isComposite)
                    DoRebind(actionToRebind, nextBindingIndex, statusText, allCompositeParts, excludeMouse);
            }
            SaveBindingOverride(actionToRebind);
            rebindComplete?.Invoke();
        });

        rebind.OnCancel(operation =>
        {
            actionToRebind.Enable();
            operation.Dispose();

            rebindCanceled?.Invoke();
        });
        rebind.WithCancelingThrough("<Keyboard>/escape");
        //rebind.WithControlsExcluding("<keyboard>/anyKey");

        if (excludeMouse)
        {
            rebind.WithControlsExcluding("Mouse");
        }

        rebindStarted?.Invoke(actionToRebind, bindingIndex);
        rebind.Start();
    }


    public static string GetBindingName(string actionName, int bindingIndex)
    {
        if (_input == null)
        {
            _input = new PlayerControls();
        }

        InputAction action = _input.asset.FindAction(actionName);
        return action.GetBindingDisplayString(bindingIndex);
    }

    private static void SaveBindingOverride(InputAction action)
    {
        for (int i = 0; i < action.bindings.Count; i++)
        {
            PlayerPrefs.SetString(action.actionMap + action.name + i, action.bindings[i].overridePath);
        }
    }

    public static void LoadBindingOverride(string actionName)
    {
        if (_input == null)
            _input = new PlayerControls();

        InputAction action = _input.asset.FindAction(actionName);

        for (int i = 0; i < action.bindings.Count; i++)
        {
            if (!string.IsNullOrEmpty(PlayerPrefs.GetString(action.actionMap + action.name + i, action.bindings[i].overridePath)))
            {
                action.ApplyBindingOverride(i, PlayerPrefs.GetString(action.actionMap + action.name + i, action.bindings[i].overridePath));
            }
        }
    }

    public static void ResetBinding(string actionName, int bindingIndex)
    {
        InputAction action = _input.asset.FindAction(actionName);

        if (action == null || action.bindings.Count <= bindingIndex)
        {
            print("Coulnd not find action or binding");
            return;
        }

        if (action.bindings[bindingIndex].isComposite)
        {
            for (int i = bindingIndex; i < action.bindings.Count && action.bindings[i].isComposite; i++)
            {
                action.RemoveBindingOverride(i);
            }
        }

        else
            action.RemoveBindingOverride(bindingIndex);



        SaveBindingOverride(action);
    }
}

