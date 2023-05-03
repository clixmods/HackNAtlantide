using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [Scene] [SerializeField] int environmentScene = 0;
    [Scene][SerializeField] int dependenceScene = 8;

    private void Awake()
    {
        SceneManager.LoadScene(dependenceScene, LoadSceneMode.Additive);
        var indexBuildActiveScene = SceneManager.GetActiveScene().buildIndex;
        if (SceneManager.GetSceneByBuildIndex(environmentScene) !=
            SceneManager.GetSceneByBuildIndex(indexBuildActiveScene))
        {
            SceneManager.LoadScene(environmentScene, LoadSceneMode.Additive);
        }
        else
        {
            Debug.LogError("You load in additive the same scene, infinite loop was catch.");
        }
            
    }
}
