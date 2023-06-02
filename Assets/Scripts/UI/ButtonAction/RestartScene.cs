using System.Collections;
using Loading;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class RestartScene : LoaderBehaviour
{
    [SerializeField] private ScriptableEvent _restartEvent;
    [SerializeField] DataPersistentHandler _dataPersistentHandler;

    private int _sceneID;
    //Loading
    public UnityEvent LoadingStart;
    public UnityEvent LoadingStop;

    public void LoadScene()
    {
        _restartEvent.LaunchEvent();
        gameObject.SetActive(true);
        LoadingStart?.Invoke();
        _sceneID = SceneManager.GetActiveScene().buildIndex;
        //StartCoroutine();
    }

    // IEnumerator LoadSceneAsync()
    // {
    //    
    // }
    public void SceneRestart()
    {
        StartCoroutine(RestartSceneCoroutine(3f));
    }
    IEnumerator RestartSceneCoroutine(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        _dataPersistentHandler.LoadAll();
        StartLoader();
        
    }

    protected override void LoaderAction()
    {
        LoadScene();
    }

    protected override IEnumerator LoaderRoutine()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(_sceneID);

        while (!operation.isDone)
        {
            yield return null;
        }

        yield return new WaitForSeconds(2);
        LoadingStop?.Invoke();
    }
}
