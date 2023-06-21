
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public struct InputActionIcon
{
    public string path;
    public Sprite icon;
    public Sprite[] additionnalIcons;
    public InputActionIcon(InputBinding ibinding)
    {
        this.path = ibinding.path;
        this.icon = null;
        this.additionnalIcons = null;
    }
}
[CreateAssetMenu(order = 0,fileName = "InputActionIcons", menuName = "SurvivalGame/Inputs/InputActionIcons")]
public class InputActionIcons : ScriptableObject
{
    public InputActionAsset inputActionAsset;
    public List<InputActionIcon> inputActionIcons = new List<InputActionIcon>();

    public Dictionary<string, InputActionIcon> dictionaryInputsIcons = new Dictionary<string, InputActionIcon>();
    
    private void OnValidate()
    {
        if (inputActionAsset != null)
        {
            for (int i = 0; i < inputActionAsset.actionMaps.Count; i++)
            {
                for (int j = 0; j < inputActionAsset.actionMaps[i].actions.Count; j++)
                {
                    for (int k = 0; k <  inputActionAsset.actionMaps[i].actions[j].bindings.Count; k++)
                    {
                        if (!CheckIfIconsExistForThisBind(inputActionAsset.actionMaps[i].actions[j].bindings[k]))
                        {
                            inputActionIcons.Add(new InputActionIcon(inputActionAsset.actionMaps[i].actions[j].bindings[k]));
                        }
                    }
                }
            }
        }
        foreach (var actionIcon in inputActionIcons)
        {
            dictionaryInputsIcons[actionIcon.path] = actionIcon;
        }
    }
    private void OnEnable()
    {
        foreach (var actionIcon in inputActionIcons)
        {
            dictionaryInputsIcons[actionIcon.path] = actionIcon;
        }
    }
    private bool CheckIfIconsExistForThisBind(InputBinding nameBinding)
    {
        for (int i = 0; i < inputActionIcons.Count; i++)
        {
            if ( inputActionIcons[i].path == nameBinding.path)
            {
                return true;
            }
        }

        return false;
    }
}
