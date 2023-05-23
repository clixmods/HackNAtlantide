using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public enum SceneType
{
    Game,
    Special
}
public class SceneLoader : MonoBehaviour
{
    public static UnityAction<int,SceneType> SceneLoaded;
    private int _currentSceneIndex;
    [Scene] [SerializeField] int environmentScene = 0;
    [Scene] int dependenceScene = 11;
    [SerializeField] private SceneType sceneType;
    private void Awake()
    {
        // If multiple are loaded in play, go reload the main scene completely
        if (SceneManager.sceneCount > 1)
        {
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                if (scene.buildIndex != environmentScene && scene.buildIndex != dependenceScene)
                {
                    var indexBuildActiveScene = scene.buildIndex;
                    SceneManager.LoadScene(indexBuildActiveScene, LoadSceneMode.Single);
                    return;
                }
            }
        }
       
        StartCoroutine(LoadSceneAsync());
    }
    
    IEnumerator LoadSceneAsync()
    {
        var indexBuildActiveScene = SceneManager.GetActiveScene().buildIndex;
        // If additive are already loaded, we need to close each of them
       
        _currentSceneIndex = indexBuildActiveScene;
        AsyncOperation operationDependence = SceneManager.LoadSceneAsync(dependenceScene, LoadSceneMode.Additive);
        while(!operationDependence.isDone)
        {
            float progressValue = Mathf.Clamp01(operationDependence.progress / 1);
            yield return null;
        }

        if (SceneManager.GetSceneByBuildIndex(environmentScene) !=
            SceneManager.GetSceneByBuildIndex(_currentSceneIndex))
        {
            AsyncOperation operationGameplayScene = SceneManager.LoadSceneAsync(environmentScene, LoadSceneMode.Additive);
            while(!operationGameplayScene.isDone)
            {
                float progressValue = Mathf.Clamp01(operationGameplayScene.progress / 1);
                yield return null;
            }
        }
        else
        {
            Debug.LogError("You load in additive the same scene, infinite loop was catch.");
        }

        SceneLoaded?.Invoke(_currentSceneIndex, sceneType);
    }
}
