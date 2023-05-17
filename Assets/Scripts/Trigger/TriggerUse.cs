using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public abstract class TriggerUse : Trigger
{ 
    [Header("USE")]
    [Tooltip("Which input the trigger need to check ?")]
    [SerializeField] private InputButtonScriptableObject inputToTrigger;
    [Tooltip("Delete Trigger after use")]
    [SerializeField] private bool triggerOnce;
    /// <summary>
    /// Event when is triggered by input
    /// </summary>
    public UnityEvent EventOnTriggerInput;

    #region Methods
    protected override void TriggerEnter(Collider other)
    {
        inputToTrigger.OnValueChanged +=InputToTriggerOnOnValueChanged;
    }
    protected override void TriggerExit(Collider other)
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
    #endregion
}
