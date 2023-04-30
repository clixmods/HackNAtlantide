using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[ExecuteAlways]
public class DecalProjectorScroll : MonoBehaviour
{
    private DecalProjector _decalProjector;

    [SerializeField] private float speedScroll;

    private void Awake()
    {
        _decalProjector = GetComponent<DecalProjector>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _decalProjector.uvBias += Vector2.one * speedScroll;
    }
}
