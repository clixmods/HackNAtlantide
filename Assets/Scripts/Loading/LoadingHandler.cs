using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class LoadingHandler : MonoBehaviour
{
    public UnityEvent LoadingStart;
    public UnityEvent LoadingEnd;
    public static LoadingHandler Instance;
    private void Start()
    {
        if (Instance != null)
        {
            Destroy(Instance.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this);

    }
    [ContextMenu("StartLoading")]
    public void StartLoading()
    {
        StartCoroutine(LoadingCoroutine());
    }

    public void StopLoading()
    {
        LoadingEnd?.Invoke();
        //StopCoroutine(LoadingCoroutine());
    }

    IEnumerator LoadingCoroutine()
    {
        LoadingStart?.Invoke();
        bool workerAreDone = false;
        yield return null;
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

        StopLoading();
        yield return null;
    }
}