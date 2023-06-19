using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
    [SerializeField] private bool SmoothOnOpenCloseMenu = true;
    public UnityEvent SmoothStart;
    public UnityEvent SmoothDone;
   
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
        if (SmoothOnOpenCloseMenu)
        {
            ResetValues();
        }
    }

    private void OnEnable()
    {
        SmoothStart?.Invoke();
        if (SmoothOnOpenCloseMenu)
        {
            ResetValues();
        }
        
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
        if (SmoothOnOpenCloseMenu)
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
                if (!_canvasGroup.interactable)
                {
                    SmoothDone?.Invoke();
                    _canvasGroup.interactable   = true;
                }
             
            }
        }
    }

    public void SetSmoothOnOpenCloseMenu(bool value)
    {
        SmoothOnOpenCloseMenu = value;
    }
}
