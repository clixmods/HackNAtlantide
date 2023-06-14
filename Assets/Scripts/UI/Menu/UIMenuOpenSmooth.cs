using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO : Use coroutine
[RequireComponent(typeof(CanvasGroup))]
public class UIMenuOpenSmooth : MonoBehaviour
{
    private CanvasGroup _canvasGroup;
    private float _currentDelay;
    private float _currentStartTheOpenDelay;
    private UIMenu _uiMenu;
    [SerializeField] private float startTheOpenInSeconds = 0;
    [SerializeField] private float openDelay;
    [SerializeField] private float closeDelay;
    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _uiMenu = GetComponent<UIMenu>();
        _uiMenu.EventOnCloseMenu.AddListener(Stop);
        _uiMenu.EventOnOpenMenu.AddListener(EnableIt);
    }

    void Stop()
    {
        enabled = false;
    }

    void EnableIt()
    {
        enabled = true;
    }
    private void OnDestroy()
    {
        _uiMenu.EventOnCloseMenu.RemoveListener(Stop);
        _uiMenu.EventOnOpenMenu.RemoveListener(EnableIt);
    }

    private void OnDisable()
    {
        ResetValues();
    }

    private void OnEnable()
    {
        ResetValues();
    }

    private void ResetValues()
    {
        _canvasGroup.alpha = 0;
        _canvasGroup.interactable = false;
        _currentDelay = 0;
        _currentStartTheOpenDelay = 0;
    }

    private void Update()
    {
        if (_canvasGroup.alpha < 1 && !_canvasGroup.interactable )
        {
            if (_currentStartTheOpenDelay >= startTheOpenInSeconds)
            {
                _currentDelay += Time.unscaledDeltaTime;
                float value = _currentDelay / openDelay;
                _canvasGroup.alpha = value;
            }
            else
            {
                _currentStartTheOpenDelay += Time.unscaledDeltaTime;
            }
        }
        else
        {
            _canvasGroup.interactable = true;
        }
    }
}
