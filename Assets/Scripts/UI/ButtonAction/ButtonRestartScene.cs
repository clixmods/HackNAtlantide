using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonRestartScene : MonoBehaviour
{
    [SerializeField] private LoadingScreen loadingScreen;
    [SerializeField] private ScriptableEvent _restartEvent;
    public void LoadScene()
    {
        _restartEvent.LaunchEvent();
        loadingScreen.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
