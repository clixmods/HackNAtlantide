using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class RestartScene : MonoBehaviour
{
    [SerializeField] private ScriptableEvent _restartEvent;
    [SerializeField] DataPersistentHandler _dataPersistentHandler;

    //Loading
    public UnityEvent LoadingStart;
    public UnityEvent LoadingStop;

    public void LoadScene()
    {
        _restartEvent.LaunchEvent();
        gameObject.SetActive(true);
        LoadingStart?.Invoke();
        StartCoroutine(LoadSceneAsync(SceneManager.GetActiveScene().buildIndex));
    }

    IEnumerator LoadSceneAsync(int sceneId)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);

        while (!operation.isDone)
        {
            yield return null;
        }

        yield return new WaitForSeconds(2);
        LoadingStop?.Invoke();
    }
    public void SceneRestart()
    {
        StartCoroutine(RestartSceneCoroutine(3f));
    }
    IEnumerator RestartSceneCoroutine(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        _dataPersistentHandler.LoadAll();
        LoadScene();
        
    }
}
