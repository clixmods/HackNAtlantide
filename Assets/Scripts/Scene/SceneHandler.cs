using UnityEngine;
using UnityEngine.Serialization;

public class SceneHandler : MonoBehaviour
{
    public static SceneHandler Instance;
    [SerializeField] private DataPersistentHandler _persistentHandler;
    [FormerlySerializedAs("intScriptableObject")] [SerializeField] private IntValueScriptableObject intValueScriptableObject;
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
            intValueScriptableObject.Value = sceneIndex;
            _persistentHandler.SaveAll();
        }
    }
}