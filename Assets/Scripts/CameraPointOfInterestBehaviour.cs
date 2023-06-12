using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Events;


public class CameraPointOfInterestBehaviour : MonoBehaviour
{
    public UnityEvent CameraDisable;
    private CinemachineVirtualCamera _virtualCamera;
    private CinemachineBrain cinemachineBrain;
    [SerializeField] private float cinematicDisableDelay = 1.30f;
    private void Awake()
    {
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();
        cinemachineBrain = FindObjectOfType<CinemachineBrain>();
        gameObject.SetActive(false);
    }
    
    [ContextMenu("Enable Camera For 2 seconds")]
    public void EnableCameraForTwoSeconds()
    {
        EnableCameraForSeconds(2);
    }
    public void EnableCameraForSeconds(float secondToShow)
    {
        gameObject.SetActive(true);
        _virtualCamera.enabled = true;
        StartCoroutine(EnableCamera(secondToShow));
    }

    private IEnumerator EnableCamera(float secondToShow)
    {
        GameStateManager.Instance.cinematicStateObject.SetActive(true);
        yield return new WaitForSeconds(secondToShow);
        Disable();
    }

    public void Disable()
    {
        StopAllCoroutines();
        StartCoroutine(DisableCoroutine());
    }

    private IEnumerator DisableCoroutine()
    {
        CameraDisable?.Invoke();
        _virtualCamera.enabled = false;
        yield return new WaitForEndOfFrame();
        while (cinemachineBrain.IsBlending)
        {
            yield return null;
        }
        GameStateManager.Instance.cinematicStateObject.SetActive(false);
        gameObject.SetActive(false);
    }
}
