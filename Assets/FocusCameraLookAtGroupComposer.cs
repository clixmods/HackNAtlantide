using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class FocusCameraLookAtGroupComposer : MonoBehaviour
{
    private CinemachineVirtualCamera _cinemachineVirtual;
    private CinemachineTargetGroup _groupTargetComposer;
    private Transform _cachedFollow;
    private void Awake()
    {
        _cinemachineVirtual = GetComponent<CinemachineVirtualCamera>();
        _groupTargetComposer = FindObjectOfType<CinemachineTargetGroup>();
       
        
        Focus.OnFocusEnable += FocusOnOnFocusEnable;
        Focus.OnFocusDisable += FocusOnOnFocusDisable;
    }

    private void FocusOnOnFocusDisable()
    {
        _cinemachineVirtual.LookAt = null;
    }


    private void FocusOnOnFocusEnable()
    {
        if (_cinemachineVirtual.enabled)
        {
            _cachedFollow = _cinemachineVirtual.Follow;
            _cinemachineVirtual.LookAt = _groupTargetComposer.transform;
            _cinemachineVirtual.Follow = null;
            var groupComposer = _cinemachineVirtual.AddCinemachineComponent<CinemachineGroupComposer>();
            groupComposer.m_MinimumFOV = 17f;
            groupComposer.m_MaximumFOV = 35f;
            groupComposer.m_GroupFramingSize = 0.66f;
        }
        else
        {
            _cinemachineVirtual.LookAt = null;
            ResetPreviousFollow();
            DestroyGroupComponent();

        }
    }

    private void OnDisable()
    {
        _cinemachineVirtual.LookAt = null;
        ResetPreviousFollow();
        DestroyGroupComponent();

    }

    private void DestroyGroupComponent()
    {
        return;
        var groupComposer = _cinemachineVirtual.GetCinemachineComponent<CinemachineGroupComposer>();
        Destroy(groupComposer);
    }

    void ResetPreviousFollow()
    {
        if (_cachedFollow != null)
        {
            _cinemachineVirtual.Follow = _cachedFollow; 
        }
    }
    private void OnEnable()
    {
        if (Focus.FocusIsEnable)
        {
            _cachedFollow = _cinemachineVirtual.Follow;
            _cinemachineVirtual.LookAt = _groupTargetComposer.transform;
            _cinemachineVirtual.Follow = null;
            var groupComposer = _cinemachineVirtual.AddCinemachineComponent<CinemachineGroupComposer>();
            groupComposer.m_MinimumFOV = 17f;
            groupComposer.m_MaximumFOV = 35f;
            groupComposer.m_GroupFramingSize = 0.66f;
        }
        else
        {
            ResetPreviousFollow();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
