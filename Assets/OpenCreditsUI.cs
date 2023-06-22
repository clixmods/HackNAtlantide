using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OpenCreditsUI : MonoBehaviour
{
    [SerializeField] private GameObject creditsGameObject;
    [SerializeField] private GameObject titleGameObject;
    public UnityEvent TitleOpened;
    public void OpenCredit()
    {
        creditsGameObject.SetActive(true);
    }

    public void OpenTitleMenu()
    {
        titleGameObject.SetActive(true);
        TitleOpened?.Invoke();
        GameStateManager.Instance.mainMenuStateObject.SetActive(true);
    }
}
