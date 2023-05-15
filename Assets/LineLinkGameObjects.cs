using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineLinkGameObjects : MonoBehaviour
{
    private LineRenderer _lineRenderer;
    public List<Transform> TransformsToLink;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3[] vectors = new Vector3[TransformsToLink.Count];
        for (int i = 0; i < TransformsToLink.Count; i++)
        {
            vectors[i] = TransformsToLink[i].position;
        }
        _lineRenderer.SetPositions(vectors);
    }
}
