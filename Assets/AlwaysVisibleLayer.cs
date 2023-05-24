using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlwaysVisibleLayer : MonoBehaviour
{
    private int _initialLayerIndex;
    private int layerAlwaysVisibleIndex = 17;
    private void Awake()
    {
        _initialLayerIndex = transform.gameObject.layer;
    }

    public void SetLayerToAlwaysVisible()
    {
        transform.gameObject.layer = layerAlwaysVisibleIndex;
    }

    public void BackToInitialLayer()
    {
        transform.gameObject.layer = _initialLayerIndex;
    }
}
