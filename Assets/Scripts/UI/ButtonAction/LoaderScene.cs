using System.Collections;
using Loading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoaderScene : LoaderBehaviour
{
    [Scene] [SerializeField] private int sceneToLoad;

    protected override void LoaderAction()
    {
       Debug.Log("Scene Load !");
    }

    protected override IEnumerator LoaderRoutine()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneToLoad);
      
        while(!operation.isDone)
        {
            yield return null;
        }
        LoaderEnd?.Invoke();
    }

    // private void LoadScene()
    // {
    //     StartCoroutine(LoadSceneAsync(sceneToLoad));
    // }
    // IEnumerator LoadSceneAsync(int sceneId)
    // {
    //     AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);
    //   
    //     while(!operation.isDone)
    //     {
    //         yield return null;
    //     }
    // }

    public void SetSceneIndexToLoad(int value)
    {
        sceneToLoad = value;
    }
}
