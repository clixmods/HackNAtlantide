using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class UIMenuOpenSmooth : MonoBehaviour
{
    private CanvasGroup _canvasGroup;
    private float _currentDelay;
    private float _currentStartTheOpenDelay;
    [SerializeField] private float startTheOpenInSeconds = 0;
    [SerializeField] private float openDelay;
    [SerializeField] private float closeDelay;
    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
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
        _currentDelay = 0;
        _currentStartTheOpenDelay = 0;
    }

    private void Update()
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
}
