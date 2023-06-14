using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MaterialIntModifier : MonoBehaviour
{
    private MaterialPropertyBlock[] _propBlocks;
    [FormerlySerializedAs("_meshRenderer")][SerializeField] private Renderer meshRenderer;
    [SerializeField] private string _intName;
    private int Amount;
    private void Awake()
    {
        Amount = Shader.PropertyToID(_intName);
        // Setup Material property block
        if (meshRenderer == null)
        {
            meshRenderer = GetComponentInChildren<Renderer>();
        }
        _propBlocks = new MaterialPropertyBlock[meshRenderer.sharedMaterials.Length];
        for (int i = 0; i < _propBlocks.Length; i++)
        {
            _propBlocks[i] = new MaterialPropertyBlock();
        }
        SetInt(0);
    }
    public void SetInt(int value)
    {
        for (int i = 0; i < _propBlocks.Length; i++)
        {
            // Get the current value of the material properties in the renderer.
            meshRenderer.GetPropertyBlock(_propBlocks[i], i);
            // Assign our new value.
            _propBlocks[i].SetInt(Amount, value);
            // Apply the edited values to the renderer.
            meshRenderer.SetPropertyBlock(_propBlocks[i], i);
        }
    }
}
