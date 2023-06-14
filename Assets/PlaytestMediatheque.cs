using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlaytestMediatheque : MonoBehaviour
{
    public UnityEvent AfterWait;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        GameStateManager.Instance.cinematicStateObject.SetActive( true);
        yield return new WaitForSeconds(10f);
        AfterWait?.Invoke();
    }
}
