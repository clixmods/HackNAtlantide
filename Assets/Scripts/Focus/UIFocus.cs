using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFocus : MonoBehaviour
{
    private Image _image;
    private Transform target;
    private void Awake()
    {
        _image = GetComponent<Image>();
        // Event subscribe
        Focus.OnFocusEnable += FocusOnOnFocusEnable;
        Focus.OnFocusDisable += FocusOnOnFocusDisable;
        Focus.OnFocusSwitch += FocusOnOnFocusSwitch;
    }

    private void OnDestroy()
    {
        // Event unsubscribe
        Focus.OnFocusEnable -= FocusOnOnFocusEnable;
        Focus.OnFocusDisable -= FocusOnOnFocusDisable;
        Focus.OnFocusSwitch -= FocusOnOnFocusSwitch;
    }

    private void FocusOnOnFocusSwitch(ITargetable target)
    {
        this.target = target.targetableTransform;
    }

    private void FocusOnOnFocusDisable()
    {
        gameObject.SetActive(false);
    }

    private void FocusOnOnFocusEnable()
    {
        gameObject.SetActive(true);
    }

    private void Update()
    {
        if(target == null)
        {
            transform.position = new Vector3(-10000,0,-100);
            return;
        }
        Vector3 position = target.position.GetPositionInWorldToScreenPoint();
        if(target.position.IsOutOfCameraVision() )
        {
            transform.position = new Vector3(-10000,0,-100);
        }
        else
        {
            transform.position = position;
        }
    }
}
