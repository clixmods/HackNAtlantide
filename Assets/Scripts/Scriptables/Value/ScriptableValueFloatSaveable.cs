using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Value/float saveable")]

public class ScriptableValueFloatSaveable : ScriptableValueFloat , ISave
{
#if UNITY_EDITOR
    private void OnValidate()
    {
        DataPersistentHandler.AddSaveAssetToHandler(this);
    }
#endif
    #region Save and Load
    class FloatSaveData : SaveData
    {
        public float Value;
        public float MaxValue;
    }
    public void OnLoad(string data)
    {
        FloatSaveData customSaveData = JsonUtility.FromJson<FloatSaveData>(data);
        Value = customSaveData.Value;
        MaxValue = customSaveData.MaxValue;

    }
    public void OnSave(out SaveData saveData)
    {
        FloatSaveData customSaveData = new FloatSaveData();
        customSaveData.Value = Value;
        customSaveData.MaxValue = MaxValue;
        saveData = customSaveData;
    }

    public void OnReset()
    {
        Value = defaultValue;
        MaxValue = defaultMaxValue;
    }
    #endregion
}
