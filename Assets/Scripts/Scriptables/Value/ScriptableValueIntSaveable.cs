using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Value/int saveable")]

public class ScriptableValueIntSaveable : ScriptableValueInt , ISave
{
    #region Save and Load
    class IntSaveData : SaveData
    {
        public int Value;
    }
    public void OnLoad(string data)
    {
        IntSaveData customSaveData = JsonUtility.FromJson<IntSaveData>(data);
        Value = customSaveData.Value;
        
    }
    public void OnSave(out SaveData saveData)
    {
        IntSaveData customSaveData = new IntSaveData();
        customSaveData.Value = Value;
        saveData = customSaveData;
    }

    public void OnReset()
    {
        Value = 0;
    }

    #endregion
}
