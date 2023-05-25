using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPointOfInterestBehaviour : MonoBehaviour
{
    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void EnableCameraForSeconds(float secondToShow)
    {
        gameObject.SetActive(true);
        StartCoroutine(EnableCamera(secondToShow));
    }

    private IEnumerator EnableCamera(float secondToShow)
    {
        GameStateManager.Instance.cinematicStateObject.SetActive(true);
        yield return new WaitForSeconds(secondToShow);
        gameObject.SetActive(false);
        GameStateManager.Instance.cinematicStateObject.SetActive(false);
    }
}
