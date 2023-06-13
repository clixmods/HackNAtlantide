using System;
using System.Collections;
using Cinemachine;
using UnityEngine;
public class FocusCinemachineTargetGroup : CinemachineTargetGroup
{
    private Transform _target;
    private bool _isSwitching;
    private Transform targetFollower;
    private float _FollowerPositionInterpolation = 0;
    [Header("Settings")]
    [Tooltip("Movement speed when we switch target to an another")]
    [SerializeField] private float switchTargetMultiplier = 1f;
    private void Awake()
    {
        if (Application.isPlaying) // the parent class use attribut executealways, so I use that to execute the following instructions only in the start game
        {
            m_Targets = new CinemachineTargetGroup.Target[2];
            m_Targets[0].target = PlayerInstanceScriptableObject.Player.transform;
            m_Targets[0].weight = 1;
            targetFollower = new GameObject().transform;
            targetFollower.SetParent(transform);
            m_Targets[1].target = targetFollower;
            m_Targets[1].weight = 1;
        }
    }
    public void SwitchToTarget(Transform newTarget)
    {
        _target = newTarget;
        _FollowerPositionInterpolation = 0;
        //this.enabled = true;
    }
    
    void FixedUpdate()
    {
        if (m_UpdateMethod == UpdateMethod.FixedUpdate)
        {
            DoUpdate();CustomUpdate();
        }
    }

    void Update()
    {
        if (!Application.isPlaying || m_UpdateMethod == UpdateMethod.Update)
        {
            DoUpdate();CustomUpdate();
        }
    }

    void LateUpdate()
    {
        if (m_UpdateMethod == UpdateMethod.LateUpdate)
        {
            DoUpdate();
            CustomUpdate();
        }
    }

    private void CustomUpdate()
    {
        if ((Application.isPlaying))
        {
            var t = Time.deltaTime * switchTargetMultiplier;
            if (_target != null)
            {
                _FollowerPositionInterpolation += Time.deltaTime;
                targetFollower.position = Vector3.Lerp(targetFollower.position,_target.position , _FollowerPositionInterpolation) ;
                m_Targets[1].weight = Mathf.Clamp(m_Targets[1].weight + t, 0, 1);
            }
            else
            {
                _FollowerPositionInterpolation = 0;
                m_Targets[1].weight = Mathf.Clamp(m_Targets[1].weight - t, 0, 1);
                if (m_Targets[1].weight == 0)
                {
                    targetFollower.position = m_Targets[0].target.position;
                    //this.enabled = false;
                }
            }
           
        }
       
    }
}
