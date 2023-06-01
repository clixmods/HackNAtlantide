using System;
using System.Collections;
using Loading;
using UnityEngine;
using UnityEngine.Events;

public class LoadingStateBehaviour : GameStateBehaviour<LoadingState>
{
    public UnityEvent OnDisableState;
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
        Debug.Log("Yo");
    }

    IEnumerator LoadingCoroutine()
    {
        //LoadingStart?.Invoke();
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
        //
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

    // public UnityEvent LoadingStart;
    // public UnityEvent LoadingEnd;
    // public static LoadingHandler Instance;
    // private void Start()
    // {
    //     if (Instance != null)
    //     {
    //         Destroy(Instance.gameObject);
    //         return;
    //     }
    //     Instance = this;
    //     DontDestroyOnLoad(this);
    //
    // }
    // [ContextMenu("StartLoading")]
    // public void StartLoading()
    // {
    //     StartCoroutine(LoadingCoroutine());
    // }
    //
    // public void StopLoading()
    // {
    //     LoadingEnd?.Invoke();
    //     //StopCoroutine(LoadingCoroutine());
    // }
    //
    // IEnumerator LoadingCoroutine()
    // {
    //     LoadingStart?.Invoke();
    //     bool workerAreDone = false;
    //     while (!workerAreDone)
    //     {
    //         workerAreDone = true;
    //         for (int i = 0; i < LoadingWorkerBehaviour.LoadingWorkerBehaviours.Count; i++)
    //         {
    //             var worker = LoadingWorkerBehaviour.LoadingWorkerBehaviours[i];
    //             if (!worker.WorkIsDone)
    //             {
    //                 workerAreDone = false;
    //             }
    //         }
    //         yield return null;
    //     }
    //
    //     StopLoading();
    //     yield return null;
    // }
    protected override void OnPostUnRegisterRemoveState()
    {
        base.OnPostUnRegisterRemoveState();
        OnDisableState?.Invoke();
    }
}
