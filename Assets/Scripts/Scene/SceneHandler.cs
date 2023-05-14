using UnityEngine;
using UnityEngine.Serialization;

public class SceneHandler : MonoBehaviour
{
    public static SceneHandler Instance;
    [SerializeField] private DataPersistentHandler _persistentHandler;
    [FormerlySerializedAs("intScriptableValue")] [FormerlySerializedAs("intValueScriptableObject")] [FormerlySerializedAs("intScriptableObject")] [SerializeField] private ScriptableValueInt scriptableValueInt;
    private int _lastestGameSceneCached;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
        SceneLoader.SceneLoaded += RegisterLastGameSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneLoader.SceneLoaded -= RegisterLastGameSceneLoaded;
    }

    private void RegisterLastGameSceneLoaded(int sceneIndex, SceneType sceneType)
    {
        if (sceneType == SceneType.Game)
        {
            scriptableValueInt.Value = sceneIndex;
            _persistentHandler.SaveAll();
        }
    }
}