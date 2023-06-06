using System.Collections;
using System.Collections.Generic;
using Loading;
using UnityEditor;
using UnityEngine;

public class QuitBehaviour : LoaderBehaviour
{
    protected override void LoaderAction()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
        return;        
#endif
        Application.Quit();
    }

    protected override IEnumerator LoaderRoutine()
    {
        yield return null;
    }
}
