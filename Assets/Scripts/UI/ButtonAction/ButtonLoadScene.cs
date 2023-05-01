using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonLoadScene : MonoBehaviour
{
    [SerializeField] private LoadingScreen loadingScreen;
    [Scene] [SerializeField] private int sceneToLoad;
    public void LoadScene()
    {
        loadingScreen.LoadScene(sceneToLoad);
    }
}
