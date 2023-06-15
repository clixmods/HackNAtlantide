using System;
using System.Collections;
using AudioAliase;
using Loading;
using UnityEngine;
using UnityEngine.Events;

public class LoadingStateBehaviour : GameStateBehaviour<LoadingState>
{
    //public UnityEvent OnDisableState;
    private Action _action;
    private IEnumerator _coroutine;
    
    private void Awake()
    {
        LoaderBehaviour.LoaderStart += LoaderStart;
        LoaderBehaviour.LoaderEnd += LoaderEnd;
        StartCoroutine(LoadingCoroutine());
    }

    private void OnDestroy()
    {
        LoaderBehaviour.LoaderStart -= LoaderStart;
        LoaderBehaviour.LoaderEnd -= LoaderEnd;
    }

    private void LoaderEnd()
    {
        _action = null;
        _coroutine = null;
        gameObject.SetActive(false);
        //StartCoroutine(LoadingEndCoroutine());
    }
    IEnumerator LoadingEndCoroutine()
    {
        // bool workerAreDone = false;
        // while (!workerAreDone)
        // {
        //     workerAreDone = true;
        //     for (int i = 0; i < LoadingWorkerBehaviour.LoadingWorkerBehaviours.Count; i++)
        //     {
        //         var worker = LoadingWorkerBehaviour.LoadingWorkerBehaviours[i];
        //         if (!worker.WorkIsDone)
        //         {
        //             workerAreDone = false;
        //         }
        //     }
        //     
        // }
        yield return null;
    }
    private void LoaderStart(LoaderBehaviour arg1, Action arg2, IEnumerator arg3)
    {
        _action = arg2;
        _coroutine = arg3;
        gameObject.SetActive(true);
        
    }

    protected override void OnPostRegisterApplyState()
    {
        base.OnPostRegisterApplyState();
        StartCoroutine(LoadingCoroutine());
    }

    IEnumerator LoadingCoroutine()
    {
        bool workerAreDone = false;
        while (!workerAreDone)
        {
            workerAreDone = true;
            for (int i = 0; i < LoadingWorkerBehaviour.LoadingWorkerBehaviours.Count; i++)
            {
                var worker = LoadingWorkerBehaviour.LoadingWorkerBehaviours[i];
                if (!worker.WorkIsDone)
                {
                    workerAreDone = false;
                }
            }
            yield return null;
        }

        Time.timeScale = 0;
        AudioManager.PauseAllAudio();
        if (_action == null && _coroutine == null)
        {
            LoaderEnd();
            yield return null;
        }

        if (_action != null)
        {
            _action?.Invoke();
        }
        if (_coroutine != null)
        {
            StartCoroutine(_coroutine);
        }
        yield return null;
    }

    protected override void OnPostUnRegisterRemoveState()
    {
        base.OnPostUnRegisterRemoveState();
        
    }
}
