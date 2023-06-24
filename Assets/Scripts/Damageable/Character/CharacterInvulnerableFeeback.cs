using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CharacterInvulnerableFeeback : MonoBehaviour
{
    [SerializeField] private Character character;
    private MaterialPropertyBlock[] _propBlocks;
    [FormerlySerializedAs("_meshRenderer")][SerializeField] private Renderer meshRenderer;
    private static readonly int propertyInvulnerableID = Shader.PropertyToID("_IsInvulnerable");
    private void Awake()
    {

        character = GetComponent<Character>();
        if(character != null)
        {
            character.OnInvulnerable += InvulnerableFeeback;
        }
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
    }
    private void InvulnerableFeeback(bool boolean)
    {
        for (int i = 0; i < _propBlocks.Length; i++)
        {
            // Get the current value of the material properties in the renderer.
            meshRenderer.GetPropertyBlock(_propBlocks[i], i);
            // Assign our new value.
            _propBlocks[i].SetInt(propertyInvulnerableID, boolean ? 1 : 0);
            // Apply the edited values to the renderer.
            meshRenderer.SetPropertyBlock(_propBlocks[i], i);
        }
    }
}
