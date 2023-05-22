using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Data/Value/Vector3 saveable")]
public class ScriptableValueVector3Saveable : ScriptableValueVector3 , ISave
{
    #region Save and Load
    class Vector3SaveData : SaveData
    {
        public Vector3 Value;
    }
    public void OnLoad(string data)
    {
        Vector3SaveData customSaveData = JsonUtility.FromJson<Vector3SaveData>(data);
        Value = customSaveData.Value;

    }
    public void OnSave(out SaveData saveData)
    {
        Vector3SaveData customSaveData = new Vector3SaveData();
        customSaveData.Value = Value;
        saveData = customSaveData;
    }

    public void OnReset()
    {
        Value = Vector3.zero;
    }

    #endregion
}
