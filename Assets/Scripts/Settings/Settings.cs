
using UnityEngine;




public class Settings : MonoBehaviour
{
    [SerializeField] SettingsScriptableObject _settingsData;
    public void Start()
    {
        DontDestroyOnLoad(this);
        Load();
    }

    private void OnApplicationQuit()
    {
        Save();
    }
    public void Save()
    {
        DataPersistentHandler.Save(_settingsData, _settingsData.name);
    }
    public void Load()
    {
        DataPersistentHandler.Load(_settingsData, _settingsData.name);
    }
}

