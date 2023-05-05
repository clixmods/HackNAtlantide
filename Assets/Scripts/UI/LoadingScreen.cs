using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    public UnityEvent LoadingStart;
    public UnityEvent LoadingStop;
    public Slider LoadingBarFill;
    public float speed;


    public void LoadScene(int sceneId)
    {
        gameObject.SetActive(true);
        LoadingStart?.Invoke();
        StartCoroutine(LoadSceneAsync(sceneId));
    }

    IEnumerator LoadSceneAsync(int sceneId)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);
      
        while(!operation.isDone)
        {
            float progressValue = Mathf.Clamp01(operation.progress / speed);
            LoadingBarFill.value = progressValue;
            
            yield return null;
        }
        
        yield return new WaitForSeconds(2);
        LoadingStop?.Invoke();
    }
}