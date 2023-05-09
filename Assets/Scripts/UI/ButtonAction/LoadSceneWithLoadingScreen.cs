using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneWithLoadingScreen : MonoBehaviour
{
    [SerializeField] private LoadingScreen loadingScreen;
    [Scene] [SerializeField] private int sceneToLoad;
    public void LoadScene()
    {
        loadingScreen.LoadScene(sceneToLoad);
    }

    public void SetSceneIndexToLoad(int value)
    {
        sceneToLoad = value;
    }
}
