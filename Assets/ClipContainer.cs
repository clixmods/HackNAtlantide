using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClipContainer : MonoBehaviour
{
    [SerializeField] private Material clipMaterial;

    private void Awake()
    {
        foreach (Transform child in transform)
        {
            foreach (Transform childChild in child)
            {
                if (childChild.TryGetComponent<MeshRenderer>(out MeshRenderer meshRendererChild))
                    meshRendererChild.enabled = false;
            }
            if (child.TryGetComponent<MeshRenderer>(out MeshRenderer meshRenderer))
                meshRenderer.enabled = false;
        }
    }

    private void OnValidate()
    {
        foreach (Transform child in transform)
        {
            child.GetComponentInChildren<MeshRenderer>().sharedMaterial = clipMaterial;
        }
    }
}
