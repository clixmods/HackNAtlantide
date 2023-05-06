using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class KeyBindUI : MonoBehaviour
{
    [SerializeField] InputActionReference _inputActionReference;
    [SerializeField] bool _excludeMouse = true;
    [Range(0, 10)]
    [SerializeField] int _selectedBinding;
    [SerializeField] InputBinding.DisplayStringOptions _displayStringOptions;

    [SerializeField] InputBinding _inputBinding;
    private int _bindingIndex;

    private string _actionName;


    [Header("UI FIELD")]
    [SerializeField] TextMeshProUGUI _actionText;
    [SerializeField] TextMeshProUGUI _rebindText;

    private void OnEnable()
    {
        _actionName = _inputActionReference.action.name;
        if (_inputActionReference != null)
        {
            InputManager.LoadBindingOverride(_actionName);
            GetBindingInfo();
            UpdateUI();
        }

        InputManager.rebindComplete += UpdateUI;
        InputManager.rebindCanceled += UpdateUI;
    }

    public void OnDisable()
    {
        InputManager.rebindComplete -= UpdateUI;
        InputManager.rebindCanceled -= UpdateUI;
    }

    private void OnValidate()
    {
        GetBindingInfo();
        UpdateUI();
    }
    void Start()
    {
        if (_inputActionReference != null)
        {
            InputManager.LoadBindingOverride(_actionName);
        }
    }



    void GetBindingInfo()
    {
        if (_inputActionReference.action != null)
        { _actionName = _inputActionReference.action.name; }

        if (_inputActionReference.action.bindings.Count > _selectedBinding)
        {
            _inputBinding = _inputActionReference.action.bindings[_selectedBinding];
            _bindingIndex = _selectedBinding;

        }
    }

    private void UpdateUI()
    {
        if (_actionText != null)
        {
            _actionText.text = _actionName;

            if (_inputActionReference.action.bindings[_selectedBinding].isPartOfComposite)
                _actionText.text = _inputActionReference.action.bindings[_selectedBinding].name;
        }

        if (_rebindText != null)
        {
            if (Application.isPlaying)
            {
                _rebindText.text = InputManager.GetBindingName(_actionName, _bindingIndex);
            }
            else
            {
                _rebindText.text = _inputActionReference.action.GetBindingDisplayString(_bindingIndex);
            }
        }
    }

    public void DoRebind()
    {

        InputManager.StartRebind(_actionName, _bindingIndex, _rebindText, _excludeMouse);

    }

    public void ResetBinding()
    {

        InputManager.ResetBinding(_actionName, _bindingIndex);
        UpdateUI();

    }
}
