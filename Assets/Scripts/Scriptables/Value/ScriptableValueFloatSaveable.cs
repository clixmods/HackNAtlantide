using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Value/float saveable")]

public class ScriptableValueFloatSaveable : ScriptableValueFloat , ISave
{
    [SerializeField] private float defaultValue = 0;
    [SerializeField] private float defaultMaxValue = 0;
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
