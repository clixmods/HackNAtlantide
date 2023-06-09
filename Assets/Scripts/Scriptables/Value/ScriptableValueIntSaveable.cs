#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Value/int saveable")]

public class ScriptableValueIntSaveable : ScriptableValueInt , ISave
{
#if UNITY_EDITOR
    private void OnValidate()
    {
        DataPersistentHandler.AddSaveAssetToHandler(this);
    } 
#endif
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
