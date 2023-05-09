using UnityEngine;
public class SceneHandler : MonoBehaviour
{
    public static SceneHandler Instance;
    [SerializeField] private IntScriptableObject intScriptableObject;
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
            intScriptableObject.Value = sceneIndex;
        }
    }
}