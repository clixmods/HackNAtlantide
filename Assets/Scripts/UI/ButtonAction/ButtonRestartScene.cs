using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonRestartScene : MonoBehaviour
{
    [SerializeField] private LoadingScreen loadingScreen;
    public void LoadScene()
    {
        loadingScreen.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
