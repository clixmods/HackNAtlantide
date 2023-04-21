using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public abstract class UseTrigger : Trigger
{ 
    [Header("USE")]
    [SerializeField] private InputButtonScriptableObject inputToTrigger;
    [SerializeField] private bool triggerOnce;
    /// <summary>
    /// Event when the volume is triggered by Exit
    /// </summary>
    public UnityEvent EventOnTriggerInput;
    
    protected override void TriggerEnter()
    {
        inputToTrigger.OnValueChanged +=InputToTriggerOnOnValueChanged;
    }
    protected override void TriggerExit()
    {
        inputToTrigger.OnValueChanged -= InputToTriggerOnOnValueChanged;
    }

    private void InputToTriggerOnOnValueChanged(bool inputValue)
    {
        if (inputValue)
        {
            Debug.Log("InputTrigger ", gameObject);
            EventOnTriggerInput?.Invoke();
            if (triggerOnce)
            {
                inputToTrigger.OnValueChanged -= InputToTriggerOnOnValueChanged;
                gameObject.SetActive(false);
            }
        }
    }
}
