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
        Focus.OnFocusEnable += FocusOnOnFocusEnable;
        Focus.OnFocusDisable += FocusOnOnFocusDisable;
        Focus.OnFocusSwitch += FocusOnOnFocusSwitch;
        
    }
    
    private void FocusOnOnFocusSwitch(Transform target)
    {
        this.target = target;
    }

    private void FocusOnOnFocusDisable()
    {
        target = null;
        gameObject.SetActive(false);
    }

    private void FocusOnOnFocusEnable()
    {
        gameObject.SetActive(true);
    }

    private void Update()
    {
        // Si l'object a follow est detruit, on le delete
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
