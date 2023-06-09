#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
[CreateAssetMenu(menuName = "Data/Value/bool Saveable")]
public class ScriptableValueBoolSaveable : ScriptableValueBool , ISave
{
    private void OnValidate()
    {
        DataPersistentHandler.AddSaveAssetToHandler(this);
    }

    #region Save and Load
    class BoolSaveData : SaveData
    {
        public bool Value;
    }
    public void OnLoad(string data)
    {
        BoolSaveData customSaveData = JsonUtility.FromJson<BoolSaveData>(data);
        Value = customSaveData.Value;
        
    }
    public void OnSave(out SaveData saveData)
    {
        BoolSaveData customSaveData = new BoolSaveData();
        customSaveData.Value = Value;
        saveData = customSaveData;
    }

    public void OnReset()
    {
        Value = false;
    }

    #endregion
     
}

