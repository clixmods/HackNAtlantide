#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace _2DGame.Scripts.Save
{
    /// <summary>
    /// ScriptableObject compatible with DataPersistantSystem, need to be place in a Resources folder
    /// </summary>
    public abstract class ScriptableObjectSaveable : ScriptableObject, ISave
    {
#if UNITY_EDITOR
        private void OnValidate()
        {
            DataPersistentHandler.AddSaveAssetToHandler(this);
        }
#endif
        public abstract void OnLoad(string data);
        public abstract void OnSave(out SaveData saveData);
        public abstract void OnReset();
    
    }
}