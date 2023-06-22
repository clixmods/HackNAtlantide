using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CinemachineTargetGroupMembers : MonoBehaviour
{
    private CinemachineTargetGroup _cinemachineTargetGroup;

    private void Awake()
    {
        _cinemachineTargetGroup = GetComponent<CinemachineTargetGroup>();
    }

    public void AddMember(Transform transformToAdd)
    {
        _cinemachineTargetGroup.AddMember(transformToAdd,1,1);
    }
}
