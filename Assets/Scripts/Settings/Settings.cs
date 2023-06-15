
using UnityEngine;




public class Settings : MonoBehaviour
{
    public static Settings instance;
    [SerializeField] SettingsScriptableObject _settingsData;
    public void Start()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
        }
        instance = this;
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

